# CHG-2026-008 代码现状确认基线

摘要：根据负责人于 2026-10-03 的授权，将上一轮列出的未确认调查项逐项回到当前代码、配置、前端消费者、数据模型和 Git 可见输入核对；对代码已经明确的行为记录为“代码确认”，对代码明确但与目标存在差异的行为记录为“代码确认但为缺陷/差距”，对代码无法决定的外部政策或运行结果保留为“代码无法确认”。
创建日期：2026-10-03
类型：docs
关联规格：[规格索引](../../../specs/index.md)
状态：verified
涉及组件：`03_src` 当前后端、前端、数据模型、迁移、Docker 输入和发布脚本；`docs/specs`；`docs/project.md`

## 用户确认与适用边界

负责人明确要求：对于上一轮列出的“不确认项”，先检查当前代码，并以当前代码已经实现的行为作为确认依据。负责人同时确认：

- 项目定位保持为干净、无商业广告的个人软件安装包归档/分发站；删除没有测量口径的“高速”性能承诺。
- 软件与平台保持单平台关系，沿用当前 `Software.PlatformId` 单值外键和前端单选交互。
- 正式发布拓扑保持 Docker 单容器方向；是否已经完成可运行镜像、群晖部署和恢复演练仍由实际环境证据决定。

“代码确认”只确认当前实现事实，不自动证明该事实正确，也不把已发现的安全、数据完整性和发布缺陷批准为目标行为。目标 REQ/AC 与代码不一致时，差异继续由现有 feature/fix/config Change 承接。

## 判定规则

| 状态 | 含义 | 文档处理 |
| --- | --- | --- |
| 代码确认 | 当前静态调用链、模型、消费者或配置已经明确表达该行为 | 作为现状基线写入 Spec/project；需要运行环境的部分仍不写成已验证 |
| 代码确认但为缺陷/差距 | 当前代码行为可以明确读取，但违反目标、安全边界或流程完整性 | 记录真实行为和证据；保留目标 REQ，不把差异改写成正常规则 |
| 用户确认 | 负责人已明确作出产品/拓扑决定，代码与决定一致或已有承接 | 写入基线决策；实施和发布仍需独立证据 |
| 代码无法确认 | 需要真实 Docker/NAS、备份恢复、域名/TLS、保留周期、负载或负责人运营政策 | 保留为外部确认项，不能用静态代码推断 |

## 53 项逐项核对矩阵

### 公开目录与详情（C01–C19）

| 编号 | 对应规格 | 当前代码结论 | 证据 | 状态与处理 |
| --- | --- | --- | --- | --- |
| C01 | CAT-001-001；ADM-001-001/002 | 公开控制器没有 `[Authorize]`；后台控制器使用 `[Authorize]`；登录由全局密码换 JWT。 | `Controllers/Public/*.cs`；`Controllers/Admin/*.cs`；`AuthController.cs:28-39` | 代码确认；HTTP 401/匿名实测仍属于运行验证 |
| C02 | CAT-001-020 | 公开布局和首页没有广告位或会员/支付入口；负责人确认保持干净归档站；不保留“高速”承诺。 | `Web/src/layouts/PublicLayout.vue`；`Web/src/views/public/Home.vue` | 用户确认；广告缺席不替代后续产品审查 |
| C03 | CAT-001-003 | 服务端分类筛选递归加入全部后代；公开首页只把当前选中父分类的直接 `children` 显示为下一层。 | `SoftwareService.cs:47-58,121-129`；`Home.vue:226-229,265-277` | 代码确认的分层现状；目标保留任意深度语义，UI 深度体验另行验证 |
| C04 | CAT-001-004 | 公开请求按单个 `platformId` 等值筛选；首页维护单个平台筛选值。 | `SoftwaresController.cs:18-26`；`SoftwareService.cs:60-63`；`Home.vue:206-213,280-283` | 代码确认；与单平台用户决定一致 |
| C05 | CAT-001-005 | `sortBy=popular` 按 `TotalDownloads`、`CreatedAt` 降序；其他值按 `UpdatedAt`、`CreatedAt` 降序；首页初始参数是 `popular`。没有按 Id 的稳定第三排序。 | `SoftwareService.cs:70-79`；`Home.vue:206-213` | 代码确认但稳定排序目标有差距；保留 CAT-001-005 的 Id 稳定排序要求 |
| C06 | CAT-001-002 | 关键词同时匹配 Name/Summary 的 `Contains`；空白参数不加条件；服务端没有显式 Trim，也没有统一大小写规则。 | `SoftwareService.cs:39-45` | 代码确认但输入规范仍有差距；不要把数据库默认排序/校对当成产品决定 |
| C07 | CAT-001-001/006 | 列表 DTO 当前包含 Name、Summary、Description、IconPath、CategoryName、Status、TotalDownloads、UpdatedAt、Platform、Tags；首页卡片按当前模板显示其中字段。 | `Dtos/Responses/SoftwareListResponse.cs:7-20`；`SoftwareService.cs:86-110`；`Home.vue` | 代码确认；DTO 多返回字段不等于公开目标必须长期暴露，按契约 Change 管理 |
| C08 | CAT-001-008/009 | 服务端直接使用 `(page-1)*pageSize`、`Skip`、`Take`；首页通过递增 page 实现加载更多；没有服务端 page/pageSize 范围校验。 | `SoftwareService.cs:81-84`；`Home.vue:241-287` | 代码确认但非法分页保护和失败页码语义有差距 |
| C09 | CAT-001-006 | 详情服务加载分类、平台、截图、标签并返回基础字段；公开详情对不存在或 Draft 返回 404。 | `SoftwareService.cs:132-178`；`SoftwaresController.cs:29-36` | 代码确认；浏览器空字段和错误呈现仍需运行验证 |
| C10 | CAT-001-010；ADM-001-026 | Tiptap 编辑器通过 `editor.getHTML()` 保存；公开详情使用 `v-html` 展示；仓库中未发现展示端 HTML 净化。 | `RichEditor.vue:116-135,217-220`；`public/SoftwareDetail.vue:83-85` | 代码确认但存在安全差距；Tiptap HTML 是当前格式，净化目标不能被现状替代 |
| C11 | CAT-001-011；ADM-001-020 | `SoftwareScreenshot` 模型、EF 配置、详情 DTO 和按 SortOrder 查询存在；未发现独立截图上传/关联/排序控制器或完整灯箱链路。 | `Models/SoftwareScreenshot.cs`；`SoftwareService.cs:164-169`；`AdminSoftwaresController.cs`；公开/后台详情页 | 代码确认但能力不完整；截图目标继续由 CAT/ADM Change 承接 |
| C12 | CAT-001-012；DST-001-017 | 版本查询按 `IsDefault` 降序、`CreatedAt` 降序；公开页面按返回顺序展示。没有 Id 第三排序。 | `VersionService.cs:27-43`；`public/SoftwareDetail.vue` 版本循环 | 代码确认但稳定排序目标有差距 |
| C13 | CAT-001-013 | 详情页当前只显示 SHA-256 前 8 位；未找到完整值查看/复制动作。 | `public/SoftwareDetail.vue` 哈希展示区；`VersionResponse.cs:15-16` | 代码确认但公开完整哈希目标未实现 |
| C14 | CAT-001-014 | 更新日志作为普通字符串保存、后台 textarea 编辑、公开 `<p>` 文本插值；没有 Markdown 渲染或折叠状态。 | `VersionService.cs:91-156`；`public/SoftwareDetail.vue:131-133`；后台版本编辑模板 | 代码确认但安全 Markdown 目标未实现 |
| C15 | CAT-001-015 | 公开页面对返回的全部版本执行 `v-for`；未找到“只显示 3 个/展开/收起”状态。 | `public/SoftwareDetail.vue:95-160` | 代码确认但历史折叠目标未实现 |
| C16 | CAT-001-007/016；DST-001-005/012/013 | 下载校验版本存在、软件 ID 匹配、`IsVisible==1`、物理文件存在；`PhysicalFile` 开启 Range；下载计数 fire-and-forget；未检查父软件 Published；`VersionResponse` 含 FilePath。 | `DownloadController.cs:19-35`；`VersionResponse.cs:8-21`；`VersionService.cs:250-265` | 代码确认但存在公开边界、计数一致性和路径暴露差距；保留目标 REQ |
| C17 | CAT-001-016；DST-001-006/007/008 | 新建/上传版本先清除同软件默认项并将新版本设为默认；隐藏或删除默认版本后尝试提升最新可见版本；显式 SetDefault 拒绝隐藏版本。 | `VersionService.cs:52-89,159-247,267-294`；`public/SoftwareDetail.vue:206-210` | 代码确认；并发失败和无可见版本边界仍需测试 |
| C18 | CAT-001-017 | `/s/:id` 与 `/software/:id` 共用公开详情组件；复制失败时提示手工复制当前地址；没有独立分享权限。 | `router/index.ts:17-27`；`public/SoftwareDetail.vue:222-232` | 代码确认；剪贴板和 Draft 行为以运行验证为准 |
| C19 | CAT-001-018/019 | 详情模板包含免责声明；官网链接和安全 opener 属性需以页面实际模板复核，不能由免责声明推断合法性。 | `public/SoftwareDetail.vue:90-91` 及官网链接模板 | 代码确认免责声明；链接安全属性属于独立检查项 |

### 管理认证、字典与软件管理（A01–A14）

| 编号 | 对应规格 | 当前代码结论 | 证据 | 状态与处理 |
| --- | --- | --- | --- | --- |
| A01 | ADM-001-001/002 | 全局配置密码校验；签发带 Admin 角色的 HMAC-SHA256 JWT；前端保存 `admin_token` 到 localStorage，Axios 发送 Bearer。 | `AuthService.cs:24-64`；`api/http.ts:9-14`；`Login.vue:59-73` | 代码确认；Cookie 不是当前会话机制 |
| A02 | ADM-001-002/022 | 路由守卫只检查 localStorage 是否有 token；API 收到 401 时清除 token 并跳转 `/login`；退出只清除本浏览器 token。 | `router/index.ts:89-108`；`api/http.ts:25-32`；`AdminLayout.vue:67-70` | 代码确认但前端存在性检查不等于有效性验证；目标措辞已保留 |
| A03 | ADM-001-006 | 看板服务只返回 SoftwareCount、VersionCount、CategoryCount、PlatformCount、TotalDownloads；没有 FileSize 空间字段。 | `DashboardService.cs:17-34`；`AdminDashboardController.cs:20-25` | 代码确认但空间统计目标未实现 |
| A04 | ADM-001-021 | 未发现 Top 10 查询、DTO 或页面排行组件。 | `DashboardService.cs`；`Web/src/views/admin/Dashboard.vue` | 代码确认缺失；目标保持 planned |
| A05 | ADM-001-003/008/009 | 分类树按 SortOrder/CreatedAt 构建；更新只拒绝“父级是自己”，不检查间接环、空名称或未知父级；删除有子分类时拒绝，软件关联按 EF SetNull。 | `CategoryService.cs:23-111`；`CategoryConfiguration.cs:16-26` | 代码确认但分类完整性目标未实现 |
| A06 | ADM-001-004/010/011 | 平台名称服务端精确比较并由数据库唯一索引约束；删除平台时软件 PlatformId 按 SetNull；排序字段持久化。 | `PlatformService.cs:23-109`；`PlatformConfiguration.cs:11-18`；`SoftwareConfiguration.cs:20-24` | 代码确认；大小写/空白归一化未定义为代码行为，不能额外推断 |
| A07 | ADM-001-005/017 | 有 `check-name` 预检查；创建/更新服务没有同平台唯一写入约束或数据库复合唯一索引。 | `AdminSoftwaresController.cs:84-95`；`SoftwareService.cs:181-247,314-324`；`SoftwareConfiguration.cs:26-29` | 代码确认但并发唯一性目标未实现 |
| A08 | ADM-001-005；CAT-001-004 | 软件请求只有单个 `PlatformId`；后台表单要求选择一个平台；模型是单值外键。 | `SoftwareCreateRequest.cs`；`Software.cs:74-82`；后台详情平台表单 | 用户确认且代码确认；不引入多平台关系 |
| A09 | ADM-001-007 | 图标接口限制 10 MB，按日期目录和 GUID 文件名保存并返回 `/uploads/...`；只检查空文件和扩展名，没有内容签名校验。 | `AdminSoftwaresController.cs:98-130` | 代码确认但合法图片和失败清理目标未实现 |
| A10 | ADM-001-018；CAT-001-007 | 软件状态可以直接改为任意整数枚举值；列表/详情检查父状态，公开版本列表/下载不检查父状态，也不自动隐藏版本。 | `SoftwareService.cs:295-305`；`SoftwaresController.cs:29-34`；`DownloadController.cs:38-43` | 代码确认但发布边界目标未实现 |
| A11 | ADM-001-019 | 删除软件尝试删除版本文件和截图文件，再删除记录；文件删除异常被吞掉；没有清理图标和正文内图片的可靠链路。 | `SoftwareService.cs:250-293` | 代码确认但资源清理可追溯性目标未实现 |
| A12 | ADM-001-012/013/014 | 标签按名称精确预检查；软件更新先清空关联，再加入请求中能查到的标签；未知 TagId 被静默忽略而非整次拒绝；删除标签由关系级联处理。 | `TagService.cs:26-101`；`SoftwareService.cs:199-207,232-244` | 代码确认但未知 ID、唯一性和删除契约仍有差距 |
| A13 | ADM-001-024 | 后台预览使用 `window.open('/software/{id}')` 的公开详情；Draft 会按公开控制器返回 404，不能真正预览草稿。 | `Softwares.vue:211-212`；`SoftwaresController.cs:29-34` | 代码确认但 Draft 预览目标未实现 |
| A14 | ADM-001-023 | `/api/admin/categories/seed` 明确 `[AllowAnonymous]`，空分类表时写入示例分类/平台/软件；模板 `WeatherForecastController` 仍可匿名访问。 | `AdminCategoriesController.cs:57-91`；`WeatherForecastController.cs:5-31` | 代码确认但生产安全边界未实现 |

### 文件、版本与哈希（D01–D10）

| 编号 | 对应规格 | 当前代码结论 | 证据 | 状态与处理 |
| --- | --- | --- | --- | --- |
| D01 | DST-001-001/002/003/013 | 默认路径不存在时创建；自定义不存在路径返回空；扫描使用 `SearchOption.AllDirectories`；自定义路径没有根目录限制；绑定文件使用字符串路径。 | `FileService.cs:29-75` | 代码确认但路径越界/规范化目标未实现 |
| D02 | DST-001-002/003 | 扫描结果中的“关联到软件”按钮只弹出“功能尚在开发中”。 | `Web/src/views/admin/Files.vue:58-62,96-98` | 代码确认占位；不能算扫描绑定完成 |
| D03 | DST-001-001/009/016 | 软件详情页可把手工路径直接提交到 `/api/admin/files/bind`，服务端转为 `VersionCreateRequest`；没有统一根目录和文件存在校验。 | `AdminFilesController.cs:34-53`；`VersionService.cs:52-89` | 代码确认但手工绑定安全目标未实现 |
| D04 | DST-001-009/010 | 版本号、更新日志可编辑；上传/绑定都会创建版本记录并把新记录设为默认；没有重复版本号约束和强制必填的服务端校验。 | `VersionService.cs:52-156`；`AdminVersionsController.cs:28-57` | 代码确认但完整校验目标未实现 |
| D05 | DST-001-010/011/014 | 后台支持文件选择、拖拽、上传进度；服务端按软件名建目录、文件名冲突追加时间戳；先写文件后写数据库，失败清理不完整。 | `admin/SoftwareDetail.vue:219-227,567-627`；`VersionService.cs:91-142` | 代码确认但失败回滚/路径安全目标未实现 |
| D06 | DST-001-011/013 | 绑定路径不存在时仍创建版本，FileSize 为 0；后台哈希后续将其置 Failed。 | `VersionService.cs:52-89`；`HashComputeService.cs:42-58` | 代码确认但文件存在性目标未实现 |
| D07 | DST-001-015 | 哈希服务只领取 Pending；先改 Computing；不存在文件或异常时写 Failed，并把错误文本写入 HashSHA256；没有 Computing 恢复和 Failed 重算入口。 | `HashComputeService.cs:30-92` | 代码确认但恢复/重算目标未实现 |
| D08 | DST-001-007 | 可见性接口直接接收整数；隐藏默认版本后尝试提升最新可见版本；没有严格限制输入为 0/1。 | `VersionService.cs:193-224`；`AdminVersionsController.cs:70-78` | 代码确认但输入约束目标未实现 |
| D09 | DST-001-008/013 | 删除接口支持 `physicalDelete`；物理删除失败被吞掉仍继续删数据库记录；当前后台页面的删除调用默认需结合模板确认。 | `AdminVersionsController.cs:60-68`；`VersionService.cs:159-190` | 代码确认但失败报告和根目录保护目标未实现 |
| D10 | DST-001-018 | SMB 复制属于系统外步骤；系统内扫描、手工绑定、后台哈希和下载入口分别存在，但扫描结果一键绑定占位，未形成可验证闭环。 | `FileService.cs`；`Files.vue:96-98`；`AdminFilesController.cs`；`HashComputeService.cs`；`DownloadController.cs` | 代码确认现状但端到端目标未完成 |

### 界面与交互（U01–U04）

| 编号 | 对应规格 | 当前代码结论 | 证据 | 状态与处理 |
| --- | --- | --- | --- | --- |
| U01 | CAT-001-022 | 当前 Header 没有明暗主题切换控制；CSS 有颜色令牌和过渡样式，不等于已经实现主题切换。 | `Web/src/components/AppHeader.vue`；`style.css` | 代码确认缺失；按目标 Change 处理 |
| U02 | CAT-001-021/022；ADM-001-025 | 页面使用 Tailwind 响应式类和后台布局；没有浏览器实测 360/800/1280 视口证据。 | `Web/src/views`；`layouts`；Tailwind 配置 | 代码确认有响应式实现意图；尺寸和无溢出只能通过浏览器验证 |
| U03 | CAT-001-010/011/013/014 | 编辑器支持标题、列表、引用、代码、图片粘贴、尺寸和对齐；公开正文使用 HTML；未发现正文代码复制或截图灯箱。 | `RichEditor.vue:64-220`；`public/SoftwareDetail.vue:83-160` | 代码确认局部能力；目标展示交互未完成 |
| U04 | ADM-001-024/025 | 后台路由、登录表单提交、401 跳转和扫描页存在；扫描绑定是占位，后台预览仍走公开详情。 | `router/index.ts`；`Login.vue:9,59-73`；`Files.vue:96-98`；`Softwares.vue:211-212` | 代码确认局部能力；目标差异保留 |

### Docker、发布与恢复（O01–O06）

| 编号 | 对应规格 | 当前代码结论 | 证据 | 状态与处理 |
| --- | --- | --- | --- | --- |
| O01 | OPS-001-001/012 | `Dockerfile.Synology` 将前端构建产物复制到 ASP.NET `wwwroot`，作为单容器运行输入；分离 Web/Server 文件仍存在但缺少同源 API/uploads 代理。 | `Dockerfile.Synology:1-54`；`Dockerfile.Web:13-35`；`docker-compose.yml` | 用户确认且代码确认单容器方向；正式可运行仍需 Docker 证据 |
| O02 | OPS-001-013 | 单容器由 ASP.NET 提供 `/api`、`/uploads`、静态资源和 history fallback；分离 Nginx 配置只有 `/` fallback。 | `Program.cs:100-139`；`Dockerfile.Web:17-31` | 代码确认；分离拓扑不是正式发布路径 |
| O03 | OPS-001-002/003/009 | Synology 镜像默认 `/app/data`、`/data/software`；compose 映射 `./data`、`./software_repo`；正式 NAS 绝对路径和权限尚未确定。 | `Dockerfile.Synology:40-49`；`docker-compose.synology.yml:10-23` | 代码确认容器路径；NAS 路径/权限需现场确认 |
| O04 | OPS-001-004/010 | PowerShell 版本标签字符串在变量后直接使用冒号，历史静态 Parser 复核记录过变量引用错误；Shell 脚本仍含 `git reset --hard HEAD`、`git pull` 和 `ch *`，未经确认的破坏性/错误命令不能作为发布流程。 | `publish.ps1:15-18`；`publish.sh:10-15` | 代码确认现状但发布脚本有缺陷；未执行发布 |
| O05 | OPS-001-006/007/014 | `Program.cs` 启动时执行迁移，异常只记录日志后继续运行；仓库没有备份、恢复、回滚或 RPO/RTO 实测证据。 | `Program.cs:121-135`；`03_src` 发布输入 | 代码确认缺陷；备份保留、RPO/RTO、恢复顺序无法由代码确认 |
| O06 | OPS-001-001/004/011/015/016 | Dockerfile 使用 `npm install` 而非 `npm ci`，没有 `.dockerignore`；compose 使用 `latest`；当前环境无 Docker 命令，因此构建、启动、卷重建、健康和发布均未验证。 | `Dockerfile.Synology:5-11`；`docker-compose.synology.yml:5`；当前环境命令记录 | 代码确认输入；运行/发布结果代码无法确认 |

## 不能由当前代码确认的项目

以下内容即使代码确认矩阵完成，也不能被静态代码替代：

- Docker 镜像是否能在目标架构构建并启动，前后端、下载、上传和 history fallback 是否实际可用。
- 群晖宿主机的最终绝对卷路径、UID/GID、目录权限、外部反向代理、域名和 TLS 证书。
- SQLite/WAL、上传目录和软件包的备份一致性、保留周期、RPO、RTO、恢复演练和回滚结果。
- 生产负载下的响应时间、吞吐、并发下载计数准确性以及大文件 Range 行为。
- 内容审核、版权处理、日志保留、管理员密码轮换和运维责任人的政策决定。

这些项目不是“代码没有整理”，而是代码没有事实能力决定；进入发布或运营 Change 时必须由负责人和目标环境提供证据。

## 处理结果

- 53 个调查项均已给出代码现状结论、证据入口、对应 Spec 范围和处理状态。
- 用户明确的单平台、无广告归档站、单容器方向已写入基线；不再以“尚未确认建议”描述这三项。
- 代码现状与目标不一致的项继续保留为差距，不通过降低 REQ/AC 消除。
- 未修改 `03_src` 业务代码、数据库和 Docker 配置；本 Change 只更新文档与证据。
