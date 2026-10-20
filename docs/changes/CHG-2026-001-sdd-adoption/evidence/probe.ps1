$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.Net.Http
$probeRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$runRoot = Join-Path $probeRoot ('run-' + [Guid]::NewGuid().ToString('N'))
[IO.Directory]::CreateDirectory($runRoot) | Out-Null
$storage = Join-Path $runRoot 'storage'
$outside = Join-Path $runRoot 'outside-library'
[IO.Directory]::CreateDirectory($storage) | Out-Null
[IO.Directory]::CreateDirectory($outside) | Out-Null
$listener = New-Object Net.Sockets.TcpListener([Net.IPAddress]::Loopback, 0)
$listener.Start()
$port = $listener.LocalEndpoint.Port
$listener.Stop()
$env:ASPNETCORE_URLS = "http://127.0.0.1:$port"
$env:ASPNETCORE_ENVIRONMENT = 'Production'
$env:ConnectionStrings__DefaultConnection = 'Data Source=' + (Join-Path $runRoot 'probe.db')
$env:AppSettings__StorageBasePath = $storage
$env:AppSettings__UploadBasePath = Join-Path $runRoot 'uploads'
$env:AppSettings__AdminPassword = [Guid]::NewGuid().ToString('N')
$env:AppSettings__JwtSecret = [Guid]::NewGuid().ToString('N') + [Guid]::NewGuid().ToString('N')
$env:AppSettings__JwtExpirationHours = '1'
$client = New-Object Net.Http.HttpClient
$client.Timeout = [TimeSpan]::FromSeconds(10)
$results = New-Object 'Collections.Generic.List[object]'
$server = $null
function Check($name, $condition) {
    $results.Add([pscustomobject]@{ name = $name; observed = [bool]$condition })
    if (!$condition) { throw "Observation failed: $name" }
    Write-Output "OBSERVED $name"
}
function Request($method, $path, $body = $null, $token = $null, $range = $false) {
    $req = New-Object Net.Http.HttpRequestMessage([Net.Http.HttpMethod]::new($method), "http://127.0.0.1:$port$path")
    if ($null -ne $body) {
        $req.Content = [Net.Http.StringContent]::new(($body | ConvertTo-Json -Depth 10 -Compress), [Text.Encoding]::UTF8, 'application/json')
    }
    if ($token) { $req.Headers.Authorization = [Net.Http.Headers.AuthenticationHeaderValue]::new('Bearer', $token) }
    if ($range) { $req.Headers.Range = [Net.Http.Headers.RangeHeaderValue]::new(0, 2) }
    try {
        $resp = $client.SendAsync($req).GetAwaiter().GetResult()
        $text = $resp.Content.ReadAsStringAsync().GetAwaiter().GetResult()
        $json = $null
        try { $json = $text | ConvertFrom-Json } catch {}
        return [pscustomobject]@{ status = [int]$resp.StatusCode; json = $json; text = $text }
    } finally { $req.Dispose() }
}
function Versions($id, $token) { return @((Request GET "/api/admin/versions/software/$id" $null $token).json.data) }
try {
    $assembly = Join-Path $probeRoot 'server/bin/Release/net8.0/DownloadStation.Server.dll'
    $server = Start-Process dotnet -ArgumentList $assembly -WorkingDirectory $runRoot -WindowStyle Hidden -PassThru -RedirectStandardOutput (Join-Path $runRoot 'stdout.log') -RedirectStandardError (Join-Path $runRoot 'stderr.log')
    $ready = $false
    for ($i = 0; $i -lt 40; $i++) {
        if ($server.HasExited) { throw 'Isolated server exited before readiness' }
        try { $r = Request GET '/api/softwares'; $ready = $r.status -eq 200 } catch {}
        if ($ready) { break }
        Start-Sleep -Milliseconds 250
    }
    Check 'R01 fresh isolated database public list empty' ($ready -and $r.json.data.totalCount -eq 0)
    Check 'R02 anonymous management read denied 401' ((Request GET '/api/admin/softwares').status -eq 401)
    Check 'R03 anonymous management create denied 401' ((Request POST '/api/admin/softwares' @{name='blocked'}).status -eq 401)
    $r = Request POST '/api/auth/login' @{password='wrong-probe-password'}
    Check 'R04 incorrect password body code 401 HTTP 200' ($r.status -eq 200 -and $r.json.code -eq 401)
    $r = Request POST '/api/auth/login' @{password=$env:AppSettings__AdminPassword}
    $token = $r.json.data.token
    Check 'R05 valid isolated credential issues JWT' ($r.status -eq 200 -and $token.Length -gt 30)
    $r = Request POST '/api/admin/categories/seed'
    $seed = Request GET '/api/softwares'
    Check 'R06 GAP anonymous seed creates data in fresh database' ($r.status -eq 200 -and $seed.json.data.totalCount -eq 1)
    $category = (Request POST '/api/admin/categories' @{name='Probe Parent';sortOrder=20} $token).json.data
    $child = (Request POST '/api/admin/categories' @{name='Probe Child';parentId=$category.id;sortOrder=21} $token).json.data
    $platform = (Request POST '/api/admin/platforms' @{name='Probe Platform';colorHex='#123456';sortOrder=20} $token).json.data
    $tag = (Request POST '/api/admin/tags' @{name='Probe Tag'} $token).json.data
    Check 'R07 authenticated metadata fixtures created' ($category.id -and $child.id -and $platform.id -and $tag.id)
    $sw = (Request POST '/api/admin/softwares' @{name='SDD Probe';summary='probe keyword';description='<p>Probe HTML</p>';categoryId=$child.id;platformId=$platform.id;tagIds=@($tag.id)} $token).json.data
    $id = $sw.id
    Check 'R08 software created Published single platform with tag' ($id -and $sw.status -eq 1 -and $sw.platform.id -eq $platform.id -and $sw.tags.Count -eq 1)
    $r = Request GET ('/api/softwares?categoryId=' + $category.id + '&platformId=' + $platform.id + '&keyword=keyword')
    Check 'R09 category descendants platform keyword combined filter' ($r.json.data.totalCount -eq 1)
    Check 'R10 missing public software returns 404' ((Request GET '/api/softwares/absent').status -eq 404)
    $file1 = Join-Path $storage 'one.bin'
    $file2 = Join-Path $storage 'two.bin'
    [IO.File]::WriteAllBytes($file1, [Text.Encoding]::ASCII.GetBytes('first-probe-payload'))
    [IO.File]::WriteAllBytes($file2, [Text.Encoding]::ASCII.GetBytes('second-probe-payload'))
    $v1 = (Request POST '/api/admin/files/bind' @{softwareId=$id;versionNumber='1';filePath=$file1;changelog='first'} $token).json.data
    $v2 = (Request POST '/api/admin/files/bind' @{softwareId=$id;versionNumber='2';filePath=$file2;changelog='second'} $token).json.data
    $vs = Versions $id $token
    Check 'R11 new bind replaces previous default' ($vs.Count -eq 2 -and $vs[0].id -eq $v2.id -and @($vs | Where-Object isDefault -eq 1).Count -eq 1)
    $r = Request PATCH ('/api/admin/versions/' + $v2.id + '/visibility') 0 $token
    $vs = Versions $id $token
    Check 'R12 hide default promotes other visible version' ($r.status -eq 200 -and ($vs | Where-Object id -eq $v1.id).isDefault -eq 1)
    $r = Request PATCH ('/api/admin/versions/' + $v2.id + '/default') $null $token
    Check 'R13 hidden version rejected as default' ($r.status -ge 400)
    Check 'R14 hidden version download returns 404' ((Request GET "/api/softwares/$id/versions/$($v2.id)/download").status -eq 404)
    $null = Request PATCH ('/api/admin/versions/' + $v1.id + '/visibility') 0 $token
    Check 'R15 all hidden leaves no default' (@((Versions $id $token) | Where-Object isDefault -eq 1).Count -eq 0)
    $null = Request PATCH ('/api/admin/versions/' + $v1.id + '/visibility') 1 $token
    Check 'R16 showing version without default promotes it' (((Versions $id $token) | Where-Object id -eq $v1.id).isDefault -eq 1)
    $r = Request GET "/api/softwares/$id/versions/$($v1.id)/download"
    Check 'R17 download returns exact file content' ($r.status -eq 200 -and $r.text -eq 'first-probe-payload')
    $r = Request GET "/api/softwares/$id/versions/$($v1.id)/download" $null $null $true
    Check 'R18 Range returns 206 exact first three bytes' ($r.status -eq 206 -and $r.text -eq 'fir')
    Check 'R19 wrong software ownership download returns 404' ((Request GET "/api/softwares/absent/versions/$($v1.id)/download").status -eq 404)
    $null = Request PATCH "/api/admin/softwares/$id/status" @{status=0} $token
    Check 'R20 Draft detail returns 404 and public search empty' ((Request GET "/api/softwares/$id").status -eq 404 -and (Request GET '/api/softwares?keyword=SDD').json.data.totalCount -eq 0)
    $r = Request GET "/api/softwares/$id/versions"
    Check 'R21 GAP Draft visible versions exposed with FilePath' ($r.status -eq 200 -and $r.json.data.Count -eq 1 -and $r.json.data[0].filePath)
    Check 'R22 GAP Draft version file publicly downloadable' ((Request GET "/api/softwares/$id/versions/$($v1.id)/download").status -eq 200)
    $externalFile = Join-Path $outside 'outside.bin'
    [IO.File]::WriteAllBytes($externalFile, [Text.Encoding]::ASCII.GetBytes('outside-library-probe'))
    $r = Request GET ('/api/admin/files/scan?path=' + [Uri]::EscapeDataString($outside)) $null $token
    Check 'R23 GAP custom scan reads outside configured library' ($r.status -eq 200 -and $r.json.data.Count -eq 1)
    $v3 = (Request POST '/api/admin/files/bind' @{softwareId=$id;versionNumber='3';filePath=$externalFile} $token).json.data
    Check 'R24 GAP binding outside configured library accepted' ([bool]$v3.id)
    $missingFile = Join-Path $storage 'missing.bin'
    $v4 = (Request POST '/api/admin/files/bind' @{softwareId=$id;versionNumber='4';filePath=$missingFile} $token).json.data
    Check 'R25 GAP missing bound file accepted size zero' ($v4.id -and $v4.fileSize -eq 0)
    Check 'R26 missing physical file download returns 404' ((Request GET "/api/softwares/$id/versions/$($v4.id)/download").status -eq 404)
    $hashReady = $false
    for ($i = 0; $i -lt 32; $i++) {
        $vs = Versions $id $token
        $good = $vs | Where-Object id -eq $v1.id
        $bad = $vs | Where-Object id -eq $v4.id
        $hashReady = $good.hashStatus -eq 2 -and $bad.hashStatus -eq 3
        if ($hashReady) { break }
        Start-Sleep -Milliseconds 250
    }
    $expectedHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $file1).Hash
    Check 'R27 background SHA256 Done and exact digest' ($hashReady -and $good.hashSHA256 -eq $expectedHash)
    Check 'R28 background missing file Failed' ($bad.hashStatus -eq 3)
    $null = Request DELETE ('/api/admin/versions/' + $v4.id) $null $token
    Check 'R29 deleting default promotes latest visible version' (((Versions $id $token) | Where-Object id -eq $v3.id).isDefault -eq 1)
    $null = Request DELETE ('/api/admin/versions/' + $v3.id) $null $token
    Check 'R30 API default delete retains physical file' ([IO.File]::Exists($externalFile))
    $r = Request GET '/api/admin/dashboard/statistics' $null $token
    Check 'R31 dashboard authenticated succeeds' ($r.status -eq 200)
} finally {
    if ($server -and !$server.HasExited) {
        $daemon = multica daemon status --output json | ConvertFrom-Json
        if ($LASTEXITCODE -ne 0 -or !$daemon.pid -or $server.Id -eq $daemon.pid) { throw 'Cannot safely identify server PID for cleanup' }
        Stop-Process -Id $server.Id -ErrorAction Stop
        $server.WaitForExit()
    }
    $client.Dispose()
    $results | ConvertTo-Json -Depth 5 | Set-Content -Encoding UTF8 -LiteralPath (Join-Path $runRoot 'observations.json')
    Write-Output "PROBE_ROOT $runRoot"
    Write-Output "SERVER_STOPPED $($server.HasExited)"
}
