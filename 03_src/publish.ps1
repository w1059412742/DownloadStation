# DownloadStation 一键发布与推送脚本 (Windows PowerShell)

# ========== 配置区域 ==========
# 请在此处填写您的私有仓库地址，例如: registry.cn-hangzhou.aliyuncs.com/myrepo
$REGISTRY = "dockerhub.heitang.top:8443"
$IMAGE_NAME = "softstation"

# 获取动态版本号
$VERSION = Read-Host "请输入发布版本号 (例如 1.0.1)"
if ([string]::IsNullOrWhiteSpace($VERSION)) {
    Write-Host "⚠️ 版本号不能为空，脚本退出。" -ForegroundColor Yellow
    exit 1
}

# 完整的镜像全称
$VERSION_TAG = "$REGISTRY/$IMAGE_NAME:$VERSION"
$LATEST_TAG = "$REGISTRY/$IMAGE_NAME:latest"

# ========== 脚本逻辑 ==========

# 1. 确保在 03_src 目录下运行
$ScriptPath = Split-Path -Parent $MyInvocation.MyCommand.Definition
Set-Location $ScriptPath

Write-Host "--- 🚀 开始构建 Docker 镜像: $VERSION_TAG ---" -ForegroundColor Cyan

# 2. 执行 Docker 构建
docker build -t $VERSION_TAG -f Dockerfile.Synology .

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ 构建失败，请检查 Dockerfile 或网络连接。" -ForegroundColor Red
    exit $LASTEXITCODE
}

# 4. 打上 latest 标签
Write-Host " --- 🏷️ 正在通过 $VERSION 制作 latest 标签 ---"
docker tag $VERSION_TAG $LATEST_TAG

Write-Host "--- ✅ 构建与打标成功，准备推送... ---" -ForegroundColor Green

# 3. 执行 Docker 推送
Write-Host " --- ⬆️ 推送版本号标签: $VERSION_TAG ---"
docker push $VERSION_TAG

Write-Host " --- ⬆️ 推送 latest 标签: $LATEST_TAG ---"
docker push $LATEST_TAG

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ 推送失败，请确保已执行 docker login 登录到私有仓库。" -ForegroundColor Red
    exit $LASTEXITCODE
}

Write-Host "--- 🎉 一键发布完成！ ---" -ForegroundColor Cyan
