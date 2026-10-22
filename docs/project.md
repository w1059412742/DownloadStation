# 项目说明

## 目标与范围

本项目是个人软件安装包管理和分发站，包含访客页面、单管理员后台、文件存储与异步 SHA256 计算。
正式维护目标与实现差距见[规格导航](specs/index.md)，目标确认的真实来源在对应主规格和接入记录维护。接入范围、调查限制和历史审查结论均从原记录定位，不以目标确认推断产品验收或部署。

调查覆盖、证据、目标来源、例外、未调查内容与后续待办统一维护在[接入记录](changes/CHG-2026-001-sdd-adoption/change.md)，这里不复制能力清单。

## 系统边界与组件

| 组件 | 实际入口 | 数据和依赖 |
| --- | --- | --- |
| 浏览器 SPA | [路由](../03_src/DownloadStation.Web/src/router/index.ts)、[入口](../03_src/DownloadStation.Web/src/main.ts) | Vue、Vue Router、Axios；访客页和后台共用一个应用 |
| HTTP 服务 | [Program.cs](../03_src/DownloadStation.Server/Program.cs)、`Controllers/` | ASP.NET Core Controller、JWT Bearer、作用域业务服务 |
| 数据 | [AppDbContext](../03_src/DownloadStation.Server/Data/AppDbContext.cs)、`Data/Configurations/`、`Migrations/` | EF Core SQLite；迁移定义不代表已执行 |
| 文件 | `FileService`、`VersionService`、`DownloadController` | 默认软件库、可选扫描路径、上传目录、物理文件流 |
| 后台任务 | [HashComputeService](../03_src/DownloadStation.Server/BackgroundServices/HashComputeService.cs) | 从数据库选择 Pending 版本，创建独立作用域计算哈希 |
| 部署材料 | `03_src/Dockerfile.*`、`03_src/docker-compose*.yml`、`03_src/publish.*` | 分容器和单容器两套定义；实际部署版本、挂载和环境覆盖未知 |

SMB 是人工把文件放入存储目录的外部流程，本仓库未见 SMB 服务端实现。可确认的 HTTP 消费者为仓库内 Vue 页面和富文本组件；外部客户端、NAS 实机与反向代理运行配置未核对。

## 工程事实与契约来源

维护基线源码为 `master@57e28479bda434f1e9f507872ff16c53fcfb1830`；后续文档版本与任务分支在对应 Change 固定。源码版本不是部署版本声明。

后端目标 `net8.0`，EF Core SQLite 和 JWT Bearer 引用 `8.0.13`。前端锁文件解析结果为 Vue `3.5.30`、Vue Router `5.0.3`、Vite `7.3.1`、Axios `1.13.6`、Tiptap Vue `3.20.1`。这些是声明或锁定版本，未验证运行加载版本。

HTTP 契约的当前可核对来源是 `Controllers/`、`Dtos/Requests/` 和 `Dtos/Responses/`；Swagger 在 Development 中由代码生成，未启动生成或手工修改产物。数据库结构来源为模型、Fluent 配置及迁移，实际数据库结构和迁移历史未检查。旧设计中的 API 表仅作历史资料，不能覆盖代码中已有差异。

`SoftwareStatus` 声明 Draft=0（下架/草稿）、Published=1；`HashStatus` 声明 Pending=0、Computing=1、Done=2、Failed=3。页面中的“已验证”只以 HashStatus=2 为显示条件，不能据此认定安装包安全或产品已验收。安装包软件库为StorageBasePath；图片目录为独立UploadBasePath。

## 编号与术语

能力编号为 `F-中文领域-001`，需求为 `REQ-中文领域-001-001`，验收为 `AC-中文领域-001-001`。领域名称固定为访问控制、软件浏览、基础字典、软件档案、版本管理、软件库、文件校验、后台概览；名称不随文案润色改变。八项能力、REQ/AC在各主规格唯一维护，证据与例外在Change维护；旧英文编号及其同尾号引用经[迁移映射](changes/CHG-2026-003-spec-recheck/change.md#编号迁移映射)定位，不重新分配尾号。

“软件库”指 `AppSettings:StorageBasePath` 指定的安装包目录，图片另存 `UploadBasePath`；“默认版本”是下载优先选择，不保证创建时间最新。“已验证”是现有页面对哈希Done的展示文案，不表示安全检测、实现验收或发布。

## 开发与验证命令

下表维护工程命令。前端需Node/npm及项目依赖，后端需.NET 8适用SDK/运行时与依赖；具体锁定版本见上文。直接启动原目录会写入已跟踪数据库；隔离构建与探测的历史方法和结果见[接入记录](changes/CHG-2026-001-sdd-adoption/change.md#实际验证)。

| 用途 | 工作目录 | 命令 | 条件和副作用 |
| --- | --- | --- | --- |
| 前端开发 | `03_src/DownloadStation.Web` | `npm run dev` | 需依赖已安装；Vite 监听 5173，代理 `/api` 和 `/uploads` 到 5186 |
| 前端构建 | 同上 | `npm run build` | 执行 `vue-tsc -b` 和 `vite build`，生成构建文件 |
| 后端启动 | `03_src/DownloadStation.Server` | `dotnet run` | 需适用运行时和隔离配置；会执行数据库迁移、创建上传目录并启动哈希写库任务 |
| 文档检查 | 仓库根目录 | `python <sdd-documentation技能目录>/scripts/check_sdd.py . --adoption` | 正式接入默认结构追加入口检查；只检查支持的文档结构/引用及入口基本正文，不验证业务、认可来源、运行时更新和Git归属 |

历史工具版本、隔离构建输出、31项HTTP/任务观察及未测试范围统一保存在[接入验证记录](changes/CHG-2026-001-sdd-adoption/change.md#实际验证)。历史成功不表示后续文档修订重新构建或复测。

不要把 `testDelete.js` 当作无副作用测试：其代码会尝试登录本地服务并删除查询到的首个软件，且登录地址与当前路由不同。仓库没有已定位到的自动化测试项目或前端 test 脚本。

## 发布与恢复

单容器材料将 SPA 放入 ASP.NET Core 的 `wwwroot`，数据库和上传目录使用数据卷；分容器材料由 Nginx 托管 SPA。实际采用哪套配置、是否发布以及备份恢复结果均未知。

`publish.ps1` 和 `publish.sh` 包含镜像构建及推送动作；`publish.sh` 在构建前还会执行 `git reset --hard HEAD`、`git pull`，并含未核明用途的 `ch *`。本轮均未执行，不应将该脚本作为无副作用验证命令。没有可据以声称生产上线或恢复可用的记录。部署材料中的差异见接入记录 E24。

## 规格与覆盖导航

- [根AGENTS.md项目规则](../AGENTS.md)：项目级执行边界和场景路由位于非自动管理部分，自动区块保留原文；运行时更新及Git交付边界见补查记录。
- [维护规格复核与编号映射](changes/CHG-2026-003-spec-recheck/change.md)：中文编号映射、源码及旧资料对应、历史审查来源。
- [交付完整性补查](changes/CHG-2026-002-delivery-completeness/change.md)：保留另名方案撤回、审查者更正和当时证据；当前Git事实及复查输入以新记录为准。
- [正式维护规格](specs/index.md)：八项能力目标已确认，落实状态分别登记。
- [原接入变更](changes/CHG-2026-001-sdd-adoption/change.md)：D1/D2、调查覆盖、原始证据、历史差距及后续待办。
- [变更索引](changes/index.md)。
- 旧资料：`01_需求/`、`02_设计/`、`.agents/skills/prd/SKILL.md`、`.agents/skills/design/SKILL.md`。其历史认可适用性尚未确定。

Spec的approved来源为本轮采用授权及新增边界决定，不追认旧资料的历史批准。接入检查仅限已声明的八项维护能力、固定源码和配置、证据类型及排除范围；它不代表实现符合目标、产品验收或上线。历史实现待办及浏览器、并发/恢复、原库、真实NAS和生产等限制继续保留，不宣称全项目或全部路径已验证。
