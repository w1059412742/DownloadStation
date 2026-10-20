# CHG-2026-001 DownloadStation SDD 存量接入

摘要：建立了 DownloadStation 的 SDD 项目入口、四份能力 Spec 草案、HTTP 契约参考和 Docker 运维边界。2026-10-02 复核确认需求覆盖、现状调查、REQ/AC、技能路由和 Docker 说明仍有缺口，原“完整调查”判断更正；相关目标尚未批准，接入未完成。具体发现与 53 个审查项见本记录的复核附件。
创建日期：2026-10-02
类型：docs
关联规格：[规格索引](../../specs/index.md)
涉及组件：根目录、`03_src/DownloadStation.Server`、`03_src/DownloadStation.Web`、`03_src` Docker/Compose、`01_需求`、`02_设计`、`.agents/skills`
状态：implementing
交付范围：初步调查代码可达范围及主要部署输入，生成 draft Spec 和导航；2026-10-02 复核发现覆盖缺口，补齐调查与规格工作仍未完成。未修代码、未修改数据库、未构建/推送/部署 Docker、未声称产品验收或正式接入完成。
目标与方案确认依据：用户授权将 `02_DownloadStation` 按 SDD 反向工程技能整理，并明确最终通过 Docker 发布；其余业务差异尚未逐条确认。

## 调查范围与源快照

- 仓库：`G:\99_Output\08_codex\02_DownloadStation`
- 分支：`master_261002_测试`
- HEAD：`57e28479bda434f1e9f507872ff16c53fcfb1830`
- 调查日期：2026-10-02（Asia/Shanghai）
- 工作区（2026-10-02 更正）：原调查记录称 2026-10-02 初始工作区干净，本轮没有保存的原始输出可独立核对该初始状态；文档交付后有 `README.md` 修改及 `.planning/`、`AGENTS.md`、`docs/` 未跟踪项，原“交付前无输出”表述不准确。本轮实际 Git 输出见 [复核检查记录](evidence/2026-10-02-review-checks.json)。
- 后端源：`03_src/DownloadStation.Server`；前端源：`03_src/DownloadStation.Web`。
- 运行配置：开发配置使用本地 SQLite 和相对目录；Docker 单容器使用 `/app/data`、`/data/software` 和环境变量覆盖。
- 数据库：`03_src/DownloadStation.Server/downloadstation.db` 以 SQLite `mode=ro` 只读打开；未执行迁移或写入。
- 未调查：真实群晖容器、真实镜像仓库、外部访问者、生产备份/恢复、线上性能和安全扫描。

## 覆盖与能力地图

| 能力/组件 | 实际入口及关键路径 | 契约/消费者 | 覆盖状态 | 证据与缺口 |
| --- | --- | --- | --- | --- |
| 公开目录 | `Public/SoftwaresController` → `SoftwareService.GetPagedListAsync`；`Public/CategoriesController`、`PlatformsController` | `Home.vue`、公共布局 | 已调查主要路径 | E5、E7；未做浏览器端到端验收 |
| 软件详情和公开版本 | `Public/SoftwaresController/{id}`；`DownloadController` | `public/SoftwareDetail.vue` | 已调查主要路径 | E5、E6、E7；正文渲染和隐藏版本页面证据有限 |
| 管理认证 | `AuthController` → `AuthService` → JWT | `Login.vue`、Axios 拦截器、路由守卫 | 已调查 | E3、E5、E7；未实测密钥轮换/失效 |
| 后台内容管理 | Admin categories/platforms/tags/softwares/dashboard controllers → services | 各 admin view | 已调查主要路径 | E5、E7；关联删除和异常分支未完整实测 |
| 文件扫描/绑定 | `AdminFilesController` → `FileService`/`VersionService` | `Files.vue`、版本编辑页 | 已调查主要路径 | E6、E7；路径安全和 NAS 环境未实测 |
| 版本/哈希/下载计数 | `VersionService`、`HashComputeService`、`DownloadController` | 管理版本页、公开详情页 | 已调查主要路径 | E6、E10；无行为测试和并发证据 |
| 数据模型/迁移 | `Models`、`Data/Configurations`、`Migrations` | EF Core/SQLite | 已调查静态和只读快照 | E4、E10；快照不是生产数据库证明 |
| Docker 发布 | 三个 Dockerfile、两个 compose、两个发布脚本 | Docker/群晖 Container Manager | 已调查静态 | E8、E11；未执行 Docker 构建和部署 |
| 真实生产运行 | 群晖、镜像仓库、反向代理、备份 | 外部环境 | 未调查 | 无环境授权和实测证据 |

## 现状调查与证据

| E编号 | 现状结论 | 条件/版本 | 精确来源 | 类别 | 限制与冲突 | 对应目标/人工决定 |
| --- | --- | --- | --- | --- | --- | --- |
| E1 | 仓库当前 HEAD、分支和工作区快照已固定；调查时无未提交差异 | `master_261002_测试`，HEAD `57e2847...` | Git `rev-parse`、`status --short`、2026-10-02 | 静态 | 仅代表本次工作区，不代表远程分支最新 | 接入范围固定；后续若快照变化需复查 |
| E2 | 旧 PRD 目标是 NAS 软件分发站，包含访客/管理员、分类/平台/检索、版本、扫描绑定、哈希、删除和 Docker/NAS 工作流描述 | `01_需求/...PRD.txt`，第 1-5 节 | 旧文档 | 历史 | 旧资料未按 REQ/AC 编号；平台多选、Markdown 等与代码可能冲突 | 作为候选目标输入，不自动批准 |
| E3 | 后端通过 ASP.NET Core 8 注册 SQLite、JWT Bearer、Serilog、上传限制、静态文件、控制器和启动迁移 | 当前工作区 | `03_src/DownloadStation.Server/Program.cs`、`appsettings.json` | 静态 | 默认密码和 fallback secret 在配置中；未做生产安全验证 | F-ADM-001、F-OPS-001；安全目标待确认 |
| E4 | 数据模型包含 Category、Platform、Software、SoftwareScreenshot、SoftwareVersion、Tag；Software 当前是单平台外键，版本有可见/默认/哈希状态 | 当前工作区 | `Models/*.cs`、`Data/Configurations/*.cs`、`Data/AppDbContext.cs`、`Migrations/*` | 静态 | 模型可表达的关系不等于产品批准关系 | F-CAT-001、F-DST-001、F-ADM-001；平台关系需决定 |
| E5 | 公开 API、管理 API、登录、扫描/绑定、版本上传/删除/可见性/默认项接口均可从控制器确认 | 当前工作区 | `Controllers/Public/*.cs`、`Controllers/Admin/*.cs` | 静态 | 未通过完整 HTTP 探测；异常响应和消费者兼容未全面验证 | Spec 候选契约；需补行为验收 |
| E6 | 版本上传/绑定写入文件元数据并排队 SHA-256；下载校验可见性和文件存在后返回范围文件；计数以未等待任务更新 | 当前工作区 | `Services/Implementations/VersionService.cs`、`FileService.cs`、`BackgroundServices/HashComputeService.cs`、`Controllers/Public/DownloadController.cs` | 静态 | 无并发、失败重试、路径安全和大文件实测 | F-DST-001；需确认安全和计数语义 |
| E7 | 前端为 Vue 3 SPA，公开路由 `/`、`/software/:id`、`/s/:id`，管理路由 `/login`、`/admin/*`；Axios 从 localStorage 加 Bearer | 当前工作区 | `Web/src/router/index.ts`、`src/api/http.ts`、各 views | 静态 | 未做浏览器端到端；UIUX 中部分视觉要求未核对 | F-CAT-001、F-ADM-001 |
| E8 | `Dockerfile.Synology` 构建前端并由后端托管 `wwwroot`；compose 映射 `/app/data` 和 `/data/software`；分离部署也存在 | 当前工作区 | `03_src/Dockerfile.Synology`、`docker-compose.synology.yml`、`Dockerfile.Server`、`Dockerfile.Web`、`docker-compose.yml` | 静态 | 未执行 Docker；卷权限、健康检查、反代和 TLS 未知 | F-OPS-001；需要选择正式路径 |
| E9 | `dotnet build 03_src/DownloadStation.sln --nologo` 通过，0 警告、0 错误；`npm run build` 通过；Vite 输出 Browserslist 数据过期提示 | 2026-10-02 本机 | 实际命令输出 | 实测 | 构建不证明运行、API、Docker 或业务验收 | 仅作为构建证据 |
| E10 | SQLite 只读快照存在 6 个业务表和 `SoftwareTag`/迁移历史；当前快照业务行数为 Categories 4、Platforms 4、Softwares 4、SoftwareVersions 5、Tags 1、Screenshots 0；迁移历史 4 条 | `downloadstation.db`，只读 URI | Python sqlite3 `mode=ro` 查询 | 实测 | 数据内容可能包含历史编码问题；快照不代表生产数据或迁移成功 | 作为调查证据，不作为目标确认 |
| E11 | Git 历史从 2026-03 到 2026-07，包含 Docker、自动迁移、上传限制、默认版本、排序和后台细节等提交 | 当前仓库历史 | `git log`、相关 `git show` | 历史 | 提交标题是演变线索，不能证明业务批准或当前仍有效 | 解释 E4/E6/E8；不替代人工决定 |
| E12 | 旧资料与当前代码存在目标冲突：平台多选 vs 单平台；Markdown vs Tiptap；“最新默认” vs 当前新版本强制默认；公开声明/界面要求未完全在代码中核对 | E2 与 E4-E7 对照 | 旧 PRD/UIUX、当前源码 | 推断/冲突 | 需要先确认版本和目标，不能按文件新旧自动裁决 | 进入冲突表和后续规格讨论 |

## 冲突、未知项与讨论决定

| 问题 | 不同来源及证据 | 影响 | 建议/选项 | 人的实际决定与依据 | 去向 |
| --- | --- | --- | --- | --- | --- |
| 平台单选还是多选 | E2、E4、E7 | 数据模型、API、前端筛选和迁移 | A 保持单平台；B 迁移多对多 | 尚未决定 | F-CAT-001、F-ADM-001 保持 draft |
| 正文格式 | E2、E7、设计技能 | 存储、渲染、XSS、编辑器和契约 | A Markdown 安全子集；B Tiptap HTML；C 明确转换链 | 尚未决定 | F-CAT-001、F-DST-001 |
| 默认版本 | E2、E6、E11 | 创建、隐藏、删除和公开下载顺序 | A 新版本自动默认；B 仅人工默认；C 仅可见最新默认 | 尚未决定 | F-DST-001 |
| 下载计数 | E6 | 统计准确性、排行和并发 | A 开始响应计数；B 成功传输计数；C 可靠队列 | 尚未决定 | F-CAT-001、F-DST-001 |
| 文件路径安全 | E2、E6、E8 | NAS 越权、目录穿越和容器路径 | 限制在配置根目录并规范化；拒绝越界路径 | 尚未决定 | F-DST-001、F-OPS-001 |
| 正式 Docker 路径 | E8、用户要求 | 反代、端口、卷、运维复杂度 | 单容器 Synology；或分离容器 | 用户确认 Docker；具体路径尚未决定 | F-OPS-001 |
| 生产秘密和 seed | E3、E5 | 认证安全和初始化权限 | 环境注入、移除默认秘密；seed 仅开发/受保护 | 尚未决定 | F-ADM-001、F-OPS-001 |

## 文档方案与任务

已生成：

- `README.md`：项目入口和导航；
- `AGENTS.md`：agent 路由和完成纪律；
- `docs/project.md`：边界、组件、命令、发布和限制；
- `docs/specs/`：四份能力 Spec 草案和索引；
- `docs/contracts/http-api.md`：当前 HTTP 接口导航；
- `docs/changes/`：本接入 Change 和索引；
- `.planning/sdd-adoption/`：本次调查计划、发现和进度。

任务：

- [x] T1：固定范围、分支、HEAD、工作区和旧资料输入。
- [x] T2：调查后端、前端、数据、Docker、Git 和数据库只读快照。
- [x] T3：形成 project、draft Spec、契约参考、接入 Change 和索引。
- [ ] T4：负责人逐项确认冲突、目标行为和正式 Docker 路径；确认后将独立能力 Spec 标为 `approved`。
- [ ] T5：对已确认差距分别创建 feature/fix/config Change；不要在接入 Change 中顺便修代码。
- [ ] T6：执行 Docker 构建、临时卷启动、关键 API/浏览器探测和恢复演练，并把结果写入对应 Change。

## 实施结果与计划偏差

2026-10-02 实际交付为文档接入草案，未修改业务代码、测试、迁移、数据库数据或 Docker 发布脚本。原交付将其称为“完整调查 + 可追溯草案 + 明确阻塞项”。2026-10-02 复核更正：“完整调查”判断不成立，遗漏不仅来自未确认目标和运行环境，还来自调查、需求覆盖、验收口径及技能路由本身的不足。当前应称为“初步调查和 SDD 草案，补齐工作未完成”；不是产品验收，也不是正式发布。

## 实际验证

审查模式：现状基线
代码快照：HEAD `57e28479bda434f1e9f507872ff16c53fcfb1830`；交付前复查工作区应只包含本次文档文件和 `.planning/sdd-adoption`。
规格基线：四份 `draft` Spec，未声明 `approved`。
环境与日期：Windows 本机，2026-10-02，.NET SDK、Node/npm、Python 可用；未执行 Docker/群晖。

| 检查 | 命令/人工步骤 | 实际结果 | 证据与限制 |
| --- | --- | --- | --- |
| Git 快照 | `git status --short`、`git rev-parse HEAD`、`git branch --show-current` | 通过 | E1；只代表调查工作区 |
| 后端构建 | `dotnet build 03_src/DownloadStation.sln --nologo` | 通过，0 警告、0 错误 | E9；不证明运行和业务行为 |
| 前端构建 | `npm run build`（`03_src/DownloadStation.Web`） | 通过；有 Browserslist 过期提示 | E9；不证明浏览器行为 |
| SQLite 只读检查 | `sqlite3 mode=ro` 查询表、列、行数和迁移历史 | 通过读取 | E10；未写数据库、未证明生产状态 |
| Docker 可用性/Compose 配置 | `docker --version` | 未执行：当前环境找不到 `docker` 命令 | 未执行 Docker 构建、Compose 解析、镜像推送和群晖部署；需在具备 Docker 的发布环境补做 |
| HTTP/浏览器端到端 | 未执行 | 未执行 | 未启动应用，避免自动迁移和文件副作用 |
| 群晖部署/恢复 | 未执行 | 未执行 | 无真实 NAS 和备份演练输入 |

### 一致性结论

| 对象/编号 | 目标或调查结论 | 实际证据 | 差异分类 | 处理决定 | 所需复查 | 状态 |
| --- | --- | --- | --- | --- | --- | --- |
| F-CAT-001 | 公开列表排除草稿、关键词/分类/平台/排序和公开详情入口可从代码定位 | E5、E7、E12 | 目标未确认；平台和正文存在冲突 | 保持 draft；负责人确认后再实施 | 规格审查 + 公开 API/浏览器验收 | 待确认 |
| F-DST-001 | 版本绑定、扫描、哈希、下载、默认项和删除路径可从代码定位 | E6、E10、E12 | 计数、路径安全、默认规则目标未确认 | 保持 draft；拆分后续 feature/fix/config Change | 现状基线复查 + 行为/并发/安全测试 | 待确认 |
| F-ADM-001 | JWT、Admin CRUD、localStorage 路由守卫和匿名 seed 可从代码定位 | E3、E5、E7 | 默认秘密、seed、管理模型和删除语义未确认 | 保持 draft；先确认安全和管理目标 | 认证、权限、上传和关联删除测试 | 待确认 |
| F-OPS-001 | 单容器和分离容器两条 Docker 路径存在，用户要求 Docker 发布 | E8、E11、用户请求 | 正式路径、秘密、备份、健康检查和回滚未确认 | 单容器作为候选；未执行 Docker/群晖 | 选择路径后构建、启动、迁移和恢复演练 | 待确认 |
| 文档入口 | README、AGENTS、project、Spec、契约、Change 已建立单向导航 | `check_sdd.py` 结构检查通过；相对链接检查通过；`git diff --check` 通过 | 尚无负责人批准目标 | 保留草案状态，后续按确认更新索引 | 人工审查目标/证据/状态；后续确认后再复查 | 结构已验证，目标待确认 |

### 未执行项与限制

- 没有人的逐项目标确认，因此不能把任何 Spec 标为 `approved`，也不能把接入 Change 标为正式 `verified`/`merged`。
- 未启动 API，避免触发自动迁移、文件扫描和真实目录写入；因此未获得 HTTP、Swagger、下载、哈希后台和浏览器证据。
- 未执行 Docker 构建、镜像推送、群晖部署、反向代理、卷权限、健康检查和恢复演练。
- 旧 PRD/UIUX 和 Git 提交只作为历史/候选目标输入；冲突由负责人确认后进入正式规格。

## 接入后的待办

1. 确认平台关系、正文格式、默认版本、下载计数、文件路径安全和正式 Docker 路径。
2. 对确认的每个独立能力更新对应 Spec、REQ/AC 和 HTTP 契约；状态再从 `draft` 改为 `approved`。
3. 为已确认差距分别创建 feature/fix/config Change，按 TDD/实现工作流修改代码和测试。
4. 建立正式测试项目或可重复 API/浏览器探测；补齐公开访问、认证、文件、哈希、计数和 Docker 恢复证据。
5. 修订生产秘密注入、匿名 seed、发布脚本 `ch *`、Docker 健康检查、备份和回滚规则后，再进行群晖发布。

## 2026-10-02 复核与更正

审查模式：现状基线，附带文档语义审查。用户要求核对需求是否全部整理、是否完整落实既定 SDD 思想。

结论：两项都未完成。四份 Spec 继续保持 `draft`；本 Change 继续保持 `implementing`。没有将现有代码、历史 PRD 或本次审查建议自动批准为正式业务目标。

本轮发现扫描绑定 UI 占位、截图链局部实现、公开版本/下载缺少父软件发布限制、历史/新增需求未完整承接、REQ/AC 不足、场景与共享技能路由未完整落实，以及发布脚本解析错误和分离部署连接缺口。原能力地图和 E 表作为 2026-10-02 初步记录保留，其覆盖结论以本轮复核更正为准；原 E10 的编码问题推断本轮未证实。

- [复核报告与 53 项覆盖矩阵](evidence/2026-10-02-review.md)：逐项事实、差距、优先级与补齐顺序。
- [审查输入与 SHA-256](evidence/2026-10-02-review-inputs.json)：146 个输入文件的审查前快照，不代表每份输入均已逐行审完。
- [本轮实际检查记录](evidence/2026-10-02-review-checks.json)：命令、实际输出、环境和前后快照；历史构建不冒充本轮新测试。

补充任务：

- [x] T7：完成本轮复核，保存发现、覆盖矩阵和检查证据，纠正原完成表述。
- [ ] T8：按覆盖矩阵补齐当前事实/冲突/目标去向，细化原四份 draft Spec 的 REQ/AC 和契约参考。
- [ ] T9：落实四个场景技能、共享文档/审查技能和适用的 TDD 规则，整理旧技能的权威关系。
- [ ] T10：对补齐后的基线重新做语义一致性审查，再按 T4 确认具体目标范围。

本轮只做审查及记录更正；未启动应用、未打开数据库、未重跑前后端构建、未执行发布脚本/Docker/NAS/镜像推送，也未实施上述代码问题的修复。
