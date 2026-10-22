# CHG-2026-001 下载站 SDD 接入

历史记录说明：本文保存接入调查及当时续记；下文“当前”“待提交”等措辞均以相应记录时点为准，不作为现在的执行指令。后续提交事实见[CHG-003审后记录](../CHG-2026-003-spec-recheck/change.md#审后记录)，后续规格更正见[CHG-004](../CHG-2026-004-document-consistency/change.md)；其他变更从[变更索引](../index.md)定位。

摘要：沿用D1/D2及原固定源码整理八项维护规格和证据。2026-10-22第3轮对CHG-003固定输入的独立结论支持当前八项接入范围verified；保留前两轮及完整性保证撤回、BR-001/002具体关闭和原S2、全部实现待办，不追认旧保证或宣称代码修复。

创建日期：2026-10-20
记录版本：2026-10-22审后续记（原v5及补查历史保留；现行主规格及映射见CHG-003）
类型：docs
关联规格：[规格导航](../../specs/index.md)，F-ACC/CAT/MET/SW/VER/FIL/HSH/DSH-001，首次分配编号，无既有编号覆盖
涉及组件：本仓库 Vue SPA、ASP.NET Core API、SQLite 定义、文件管理和哈希任务
状态：verified
交付范围：已声明八项维护能力、固定源码与配置及证据边界内的正式接入记录；不覆盖全部产品行为、未调查范围或代码修复。已按队长安排完成本地提交，提交后记录收尾仍仅作本地提交，实际提交及父提交哈希见DOWN-1平台回执
目标与方案确认依据：2026-10-20 用户评论 `01a159cc-a87d-7c44-b4d2-9c789c2639ac` 明确同意前次评论 `01a1594e-8cba-72cf-846d-08bffb2d86f0` 的两项推荐；具体覆盖与排除项见D1/D2。它不是业务修复方案或实现授权。

## 当前完整性更正

2026-10-22审后续记：当前八项维护能力的调查可靠性及正式接入条件已通过[第1批第3轮独立基线审查](../CHG-2026-003-spec-recheck/evidence/baseline-review-b1-r3.md)，本记录据此将适用接入范围记为verified。固定v1包、HEAD及源码边界、DC-001/002关闭、累计3轮、来源哈希和限制统一见[审后记录](../CHG-2026-003-spec-recheck/change.md#审后记录)。本次仅保存事实及导航，不改下述历史或八份已审主规格；本地提交另待安排，整体Issue保持in_progress，B01—B10及未验证范围不变。

以下两次续记和更正保留当时事实；其中“待复查”“implementing”“累计2轮”以本次第3轮记录更新当前适用性，不恢复第2轮已撤回的完整性保证。

2026-10-22续记：当前复核见[CHG-2026-003](../CHG-2026-003-spec-recheck/change.md)。中文编号及全部REQ/AC同尾号的[迁移映射](../CHG-2026-003-spec-recheck/change.md#编号迁移映射)是旧引用到现行主规格的入口；本文历史编号、调查、任务及两份审查原文不倒写。当前主规格链接更新为新路径，不表示旧报告审过新正文。已有用户提交/合并、根运行时注入及第3轮已派但无结论的事实以CHG-003固定快照为准；下文“本轮先不提交”等为2026-10-21当时安排，本次仍按DOWN-13交审后另行安排，不启动DOWN-2至DOWN-11。

2026-10-21，原审查者评论`01a15e65-24f6-7a4d-8d3a-888676fc88d4`明确撤回第2轮报告第8行“必需文档检查满足本次审查条件”和第84行“接入审查范围内已无阻塞”的完整性保证；当时S0—S3均0仅是已发现问题的历史记录，不能继续表示交付无缺项。完整更正原文及来源见[本次补查](../CHG-2026-002-delivery-completeness/change.md)。

本记录当前恢复implementing，接入完整性及正式接入条件待独立复查；原verified不能继续作为当前放行依据。D1/D2、BR-001/002具体关闭和原S2、已完成源码核对及原日期构建/观察证据不因此自动失效。第1批累计仍为2轮，该说明不构成新正式审查。此前的遗漏及另名文件处置已由作者更正，项目规则落实在[根AGENTS.md](../../../AGENTS.md)非自动管理部分，本轮先不提交。

下列v1—v5过程及原第2轮结论均保留其发生时的历史，凡“必需检查完成”“正式接入通过”“verified”及当时下一步表述均须结合本节更正读取，不表示当前条件已经满足。原始v5由既有提交、固定包和原清单定位；不改写两份独立审查报告。

## 调查范围与源快照

### 授权与来源

| 来源 | 内容 | 适用性 |
| --- | --- | --- |
| DOWN-1 `01a15934-7105-72aa-8e7d-98bc7d25097c` | 把已完成阶段的下载站结合已有文档接入 SDD | 调查、文档整理授权；不推定全部代码行为均被批准 |
| 队长派工 `01a15936-1f3f-78cf-a17b-27c0ded8a018` | 固定快照、回传能力及目标依据；直接找人处理缺口；不修代码、push 或提前提交 | 本轮职责及交付范围；过程先记平台，后转事实文档 |
| 用户评论 `01a15940-3611-75bc-9c0b-7fdffa169c74` | 使用已 pull 的本地仓库；简称 Multica | 目录及分支命名依据，不是业务目标批准 |
| 恢复派工 `01a15940-ce9e-72dc-b9d0-6c3a517fac13` | 本地 in_place；核对真实规则、基准、改动后建立任务分支；沿用未正式审查的范围 | 本轮恢复及事实文档整理的上下文 |
| 旧 PRD、设计资料和 Git | 描述早期目标及实现演变 | 历史资料和静态证据；尚无已核实的适用认可范围 |
| 用户确认 `01a159cc-a87d-7c44-b4d2-9c789c2639ac` | 2026-10-20 22:52:27 +08:00，原话“这两条都按照你推荐的方式实行” | 对应前次完整推荐评论及调查v1，形成D1/D2；继续文档接入，不重复索要整体批准 |
| 第1轮独立审查 `01a159f2-0642-74a6-8566-89fce7542890` | 第1批第1轮，调查可靠性与正式接入条件均有保留；S2两项BR-001/002 | 原文完整保存在本记录evidence，原结论保持，作者不能代为关闭 |
| 修订派工 `01a159f3-cbb9-7c7d-841b-740d693bbcf8` | 原作者修订两项、沿用D1/D2和批次轮次；本轮不提交，Issue保持in_progress | 本次v3修订及返交范围，不授权代码修复或自动发起复查 |
| 第2轮独立复查 `01a15a4b-ac18-7d21-b688-8d96d7753c4d` | 2026-10-21，第1批第2轮，调查可靠性及正式接入条件通过；BR-001/002已复查关闭，接入产物未关闭S0—S3均0 | 适用v3已声明范围、证据类型及配置边界，原文完整保存在evidence；不代表实现符合目标 |
| 最终整理派工 `01a15a4f-129c-7053-b263-a34ca0b7cac4` | 仅整理第2轮事实、问题状态及必要导航，按现状基线门禁记录适用范围verified；本轮不提交，Issue继续in_progress | v4写入授权，不授权业务/规格实质改动；实质输入变化须交回协调受影响复查 |
| T6提交派工 `01a15a58-42fb-7528-b202-3df5d27b1294` | 队长已核对v4包、17文件清单和工作区一致，安排原作者仅本地提交；允许提交后事实收尾，Issue继续in_progress | 2026-10-21本轮写入和提交授权；仅清单内任务文件，不push、合并或纳入无关输入，不预写成功 |

v1/v2来源读取及用户确认日期为2026-10-20；v3修订日期见下文。接入v1前的平台记录只完成仓库同步阻塞核对，没有业务调查或文档；v1形成本文静态调查，v2补充实际证据与用户决定，v3在同一来源和快照上处理审查反馈，v4于2026-10-21记录第2轮正式结论及其适用状态，不新增审查轮次。

### 本轮真实目标决定

确认人：w1059412742（平台member `4e9b6c66-2a01-48de-ad25-ae5cf8fdac8a`）。来源为上述用户确认，所指推荐原文保存在前次最终评论；对应调查v1附件SHA-256为 `9F6FB37C97E338711A3A2764D28B03E42CE20DB3F51249CA4F6BBE6D0B318BD7`。

- D1：以 `master@57e28479bda434f1e9f507872ff16c53fcfb1830` 及仓库默认配置为维护基线输入，只采用已核实且没有未决例外的行为。包含JWT、软件单平台、多标签、HTML介绍、分享、全量版本和新增版本成为默认。旧PRD未完成能力保留后续待办，不算已实现；部署、实测未知和列明缺陷不因本决定成为已认可行为。
- D2：正确业务边界为“下架软件及关联版本不可经公共接口访问、管理写操作需要认证、软件文件操作限定在指定软件库”。现代码差距保留后续修复待办；计数、恢复、删除、输入校验等其余问题继续单列，未一并批准其现状。

D1是本轮采用现状，不追认历史批准；D2是本轮确认的正确目标，不声称实现已满足。原P1及P2三项边界的目标依据阻塞已解除；其他问题作为排除项及待办保留，不扩写未确认修复细节。v2已向队长交接并由独立基线审查者接收；第1轮确认D1/D2成立，v3仅按其范围纠正文档，第2轮确认仍适用；v4不新增业务决定。Issue按最终整理派工保持in_progress，交接评论同步最终材料给队长。

### Git 与工作区

- 仓库根：`G:/99_Output/08_codex/02_DownloadStation`；origin 为 `https://github.com/w1059412742/DownloadStation.git`。
- 调查开始及v4交审时的原基准：`master`、HEAD及本地`origin/master`均为`57e28479bda434f1e9f507872ff16c53fcfb1830`。未fetch，不能据本地引用断言远端当前状态。T6后任务分支HEAD因本地文档提交前进，源码基准及master/origin/master保持原值，实际提交见平台回执。
- 首次写入前已核对 `git status --short --branch`、`git diff --stat`、`git diff --cached --stat`、`git branch -vv`。原已跟踪文件与暂存区无差异。
- 原未跟踪输入：`AGENTS.md`、`.rar`、`.multica/daemon_task_context.json`、`.multica/project/resources.json`。运行时 AGENTS 已读取；归档内容未展开，不纳入业务事实依据。没有删除或暂存这些文件。
- `master_261002_测试@5a925c0` 也是既有分支，未切换、合并或作为本次源码基准，其行为未调查。
- `git check-ref-format --branch AI-Multica/master-20261020-sdd-adoption` 成功。默认沙箱分支创建因 `.git/index.lock` 权限失败，经工具权限升级后创建成功。
- 写入分支：`AI-Multica/master-20261020-sdd-adoption`，起点为上述完整HEAD。v1～v4交审时未提交、push或合并；v5按T6安排完成本地文档提交及记录收尾，不push或合并。

### 相关输入内容标识

已跟踪源码在无差异的上述 HEAD 定位。以下为初始读取的 SHA-256；密钥和密码值不进入记录。

| 路径 | SHA-256 |
| --- | --- |
| `AGENTS.md` | `D5FDB9EFA98A054DD45E0A8E20C3E645F00C37A6142A8E11ED018A7A6D9530C5` |
| `.rar` | `E3D6C0C590956942CACD07A3F0D477273FF70EACEBA516CA8632D547364E5B62` |
| `.multica/project/resources.json` | `97904FD0497E259568AC4435924268EE1741BD9415EC85691FEBAB2937246AF3` |
| `.multica/daemon_task_context.json` | `29FFE3AD3B9F7A217C3BF10329BB9D825E4616D5350F839A22E8E015130764E1` |
| `01_需求/个人专属软件下载站需求规格说明书 (PRD).txt` | `9DF33085915D40A68C6E922D33F54B18F3D782F832D97C476EA56E29677D204F` |
| `.agents/skills/prd/SKILL.md` | `3E2C041AD8A38CBBBA599628A7539B2811F48E1E716BA0BE9D9415C5BC161C60` |
| `.agents/skills/design/SKILL.md` | `A3EAECAA0F36F3CFD70F20EC1161B6B83FA12CEAD46A50BCA7057300574214EF` |
| `03_src/DownloadStation.Server/appsettings.json` | `523EE30C0F4DDEC0FB82100BF94F9E2617FE31B4834C3277AC7026E7B30F15F5` |
| `03_src/DownloadStation.Server/appsettings.Development.json` | `73F95F9E0CEB205FC1C4DC50C07697FCFA29D7087868C2AEF1D504CB38C771EC` |

默认 JSON 中软件库为 `./SoftwareStorage`、图片目录为 `./uploads`、JWT 时长为 24 小时；已配置密码和密钥，但值不公开。实际环境变量、命令行、运行目录和部署覆盖未知；本轮未使用这些凭据登录。

SQLite 的 `.db`、`.db-wal`、`.db-shm` 是已跟踪文件，未打开或迁移。它们的存在不证明生产部署或当前 schema。命令和环境入口见[项目说明](../../project.md)。

## 覆盖与能力地图

“部分”表示已核对静态路径，部分接口有R编号隔离实测，但所列分支仍有缺口，不表示目标整体实现通过。第2轮审查通过的接入范围为下文追溯表的八项维护能力，限已声明证据、配置及排除项；部署、占位和未调查行不据此升级为已接入业务能力。各能力的目标、落实及生产状态见[规格导航](../../specs/index.md)。

| 能力/组件 | 实际入口及关键路径 | 契约/消费者 | 覆盖状态 | 证据与缺口 |
| --- | --- | --- | --- | --- |
| 公共检索 | `/` → Home → public SoftwaresController → SoftwareService → SQLite | 软件列表、分类树、平台 DTO；Home | 部分 | E02-E04、R01/R09/R20；首页无最新/最热切换控件，旧目标保留B09；排序大小写分支仅静态，深层分类展示、非法分页、请求乱序未实测 |
| 详情、分享、版本下载 | `/software/:id`、`/s/:id` → 详情及版本 API → 文件流 | SoftwareDetailResponse、VersionResponse；公共详情页 | 部分 | E05-E08、R10/R14/R17-R22/R26；分享页面及计数未实测 |
| 登录与后台保护 | `/login` → AuthController → AuthService；路由守卫及 Bearer | 登录 DTO、admin_token、Axios；所有后台页面 | 部分 | E01、E09-E10、R02-R06；正确/错误登录和匿名管理拒绝已测；过期及浏览器未实测 |
| 分类、平台、标签 | `/admin/categories`、`/admin/tags` → 各 Controller/Service → SQLite | 请求/响应 DTO；软件编辑、列表、访客过滤 | 部分 | E11-E14；深层环、并发重名和删除约束未实测 |
| 软件档案和图片 | `/admin/softwares`、`/admin/softwares/:id` → SoftwareService、upload-image | 软件 DTO、富文本组件；公开列表与详情 | 部分 | E15-E17；输入校验、存储失败、HTML 内容及图片实际加载未实测 |
| 版本及默认版本 | 编辑页上传、手工路径、编辑、显示/隐藏、设默认、删除 → VersionService | 版本 DTO；公共下载及哈希任务 | 部分 | E18-E21、R11-R19/R29/R30；绑定默认切换已测；上传、并发、共享文件及回滚未实测 |
| 扫描与 NAS 绑定 | `/admin/files` → scan；编辑页手工路径 → bind | UnboundFileResponse、FileBindRequest | 部分 | E22、R23-R26；越界及缺文件偏差已测；库内递归、缺目录和权限错误未实测；SMB未调查 |
| 哈希任务 | Program 注册 → HashComputeService → 文件与 SQLite | HashStatus、SHA256；版本页面 | 部分 | E23、R27/R28；Done摘要及缺文件Failed已测；崩溃恢复、多实例、停机未实测 |
| 看板 | `/admin/dashboard` → statistics → DashboardService | 匿名对象统计字段；Dashboard | 部分 | E25、R31；接口可调用已测，聚合逐值与失败未实测；版本总数页面展示、Top10和存储量未实现，保留B09 |
| 部署及迁移 | Dockerfile、compose、publish；启动 Migrate | SQLite 迁移、存储卷、静态文件 | 部分 | E01、E19、E24；隔离新库API可用及本地构建有证据，原库schema、真实部署及恢复未验证 |
| 模板端点及占位组件 | MapControllers → `/WeatherForecast`；HelloWorld 未找到业务引用 | 天气随机结果；外部消费者未知 | 部分 | E26；天气端点可静态定位，未作业务能力批准 |
| 实际 NAS、生产、其他分支及外部客户端 | 未获得实机、部署版本与外部调用清单 | 未知 | 未调查 | 不从源码快照推定上线行为 |
| UI 视觉一致性、DOCX 原件差异 | 旧 UIUX 文本仅作定位；DOCX 未与文本比对 | 未知 | 未调查 | 不能宣称旧文档全量一致性已核对 |

## 现状调查与证据

以下路径以仓库根为起点。S 代表 `03_src/DownloadStation.Server`，W 代表 `03_src/DownloadStation.Web`。E01-E26 除明确标为历史者外，均为上述 HEAD 上的静态证据，不是运行成功或目标批准。

| E编号 | 现状结论 | 条件/版本 | 精确来源 | 类别 | 限制与冲突 | v1事项定位（v2处置见后表） |
| --- | --- | --- | --- | --- | --- | --- |
| E01 | 注入 scoped 服务和 SQLite；注册哈希后台服务；上传静态目录先于鉴权；启动调用 Migrate，异常日志后继续 | CreateBuilder 默认配置及实际覆盖未知 | `S/Program.cs`：服务注册、UseStaticFiles、UseAuthentication、Database.Migrate | 静态 | 启动有建目录和写库副作用；迁移成功未知 | 待目标依据 |
| E02 | 公开列表只查 Published；关键词匹配名称/简介；分类含内存递归后代；平台单选；公开接口不传 tagId | `includeDrafts=false` | `S/Controllers/Public/SoftwaresController.cs:GetPaged`；`S/Services/Implementations/SoftwareService.cs:GetPagedListAsync,GetDescendantCategoryIds` | 静态 | 分类环和查询边界未运行 | 待目标依据 |
| E03 | sortBy先经ToLower归一化，结果为popular（含POPULAR/Popular）时按总下载量再创建时间倒序；缺省/null、空串及其他结果按更新时间再创建时间倒序；分页直接Skip/Take，未见最大50检查 | 有效查询参数；公共及后台共用服务 | `S/Services/Implementations/SoftwareService.cs:GetPagedListAsync:71-78`；public/admin `GetPaged` | 静态，v3依BR-002纠正还原 | 旧设计声明pageSize最大50；大小写参数未另做HTTP实测，排序稳定性和非法分页未实测 | P1/P2、BR-002 |
| E04 | Home 默认 popular、每页15，回车检索、平台/两层分类切换和加载更多；并发响应无取消/序列控制 | SPA 首页 | `W/src/views/public/Home.vue:filters,fetchSoftwares,setParentCategory,setSubCategory,loadMore` | 静态 | API 递归不等于页面可逐层选择；失败只写控制台，更多页失败后页码未回退 | P1/P2 |
| E05 | 公开详情不存在或 Draft 返回404；分享模式复用详情，复制 `/s/:id`，隐藏部分导航；介绍直接 v-html | 单次 mounted 加载 | `S/Controllers/Public/SoftwaresController.cs:GetById`；`W/src/views/public/SoftwareDetail.vue:fetchData,handleShare`；`AppHeader.vue` | 静态 | 同组件切换参数未见 watch；HTML 来源及安全边界未实测 | P1/P2 |
| E06 | 公共版本查询只按 softwareId 和 IsVisible=1，未核对软件发布状态；响应含 FilePath | 获取版本列表 | `S/Controllers/Public/DownloadController.cs:GetVersions`；`VersionService.cs:GetBySoftwareIdAsync,MapToResponse` | 静态 | 与旧 PRD“下架关联版本完全隐藏”冲突；物理路径进入公开DTO | P2，排除直接采用 |
| E07 | 下载校验版本存在、归属及 IsVisible!=0，再查文件；返回 Range 物理文件流；未核对母软件状态 | 文件存在且可读 | `S/Controllers/Public/DownloadController.cs:DownloadFile` | 静态 | 0以外非法可见值未拒绝；下架软件可沿该判断进入文件分支的静态缺口 | P2，排除直接采用 |
| E08 | 计数调用未 await；服务读值后递增版本和软件计数；页面开窗即乐观+1 | 每次下载请求和按钮点击 | `DownloadController.cs:31`；`VersionService.cs:IncrementDownloadAsync`；公共详情 `triggerDownload` | 静态/推断 | 生命周期结束、数据库并发和Range/中断的统计语义未验证；不能保证每次完成下载恰好+1 | P2，后续待办 |
| E09 | 固定密码换 JWT，空配置或错误密码拒绝；签发需至少32字符密钥；过期配置解析失败回24小时；失败是HTTP200/body401 | 配置有效性影响结果 | `S/Services/Implementations/AuthService.cs:LoginAsync`；`S/Controllers/Admin/AuthController.cs:Login` | 静态 | PRD 写 Cookie，设计写 JWT；不能以设计较新自动裁决 | P1 |
| E10 | localStorage admin_token；前端守卫只检查存在；Axios附加 Bearer，HTTP401清除并跳登录；退出仅本地删除 | SPA 页面及API响应 | `W/src/router/index.ts`；`W/src/api/http.ts`；`Login.vue`、`AdminLayout.vue`、`AppHeader.vue` | 静态 | 没有服务端注销端点；AppHeader只mounted读取登录状态；浏览器实效未测 | P1/P2 |
| E11 | 分类树按 sort/创建时间递归；编辑拒绝父级为自身；删除有子节点时抛异常；Controller忽略删除false | 分类及父ID可用 | `S/Services/Implementations/CategoryService.cs`；`AdminCategoriesController.cs:DeleteCategory`；`CategoryConfiguration.cs` | 静态 | 未防间接环；父ID不合法依赖数据库；业务异常统一落500；未知ID删除响应成功 | P2，后续待办 |
| E12 | seed 明确 AllowAnonymous，分类表空时写分类、平台及示例已发布软件 | POST `/api/admin/categories/seed`，Categories为空 | `S/Controllers/Admin/AdminCategoriesController.cs:58,SeedBaseData` | 静态 | 管理写操作匿名例外，不当作通用鉴权已覆盖 | P2，排除直接采用 |
| E13 | 平台按sort/创建时间排序；名称服务查重并有唯一索引；删除平台关系 SetNull；UI管理名称颜色图标排序 | 平台服务和字典UI | `PlatformService.cs`；`PlatformConfiguration.cs`；`SoftwareConfiguration.cs`；`W/.../admin/Categories.vue` | 静态 | 并发冲突错误映射和前端图标颜色实际渲染未验证 | 待目标依据 |
| E14 | 标签按名称再创建时间排序，创建/改名服务查重；软件标签多对多；后台列表可按标签过滤，前台详情显示标签 | 标签及关联数据 | `TagService.cs`；`Models/Tag.cs`；`Migrations/20260313152542_AddTagSystem.cs`；`Tags.vue`、`Softwares.vue`、`SoftwareDetail.vue` | 静态 | 迁移Tags无名称唯一索引，应用查重有并发窗口；公开列表未开放tagId | P1/P2 |
| E15 | 软件单分类、单平台、多标签；创建默认Published；更新替换标签；同平台同名仅UI预查，失败后仍保存 | 软件编辑及CRUD | `SoftwareService.cs:CreateAsync,UpdateAsync,CheckNameExistsAsync`；请求DTO；`W/.../admin/SoftwareDetail.vue:save` | 静态 | 服务保存未强制同名唯一，PRD多平台不同；非法枚举及空值业务校验不足 | P1/P2 |
| E16 | 图标和介绍图片上传复用upload-image；请求10MB限制，GUID扩展名存储；Tiptap输出HTML，支持粘贴图片；介绍由v-html展示 | 已登录图片上传 | `AdminSoftwaresController.cs:UploadImage`；`W/src/components/common/RichEditor.vue`；public/admin详情 | 静态 | 未见服务端图片类型白名单和HTML清理；截图实体有读取映射，未找到独立相册维护/灯箱UI | P1/P2 |
| E17 | 软件删除尝试删除所有版本物理文件和截图，忽略文件异常后删数据库；图标文件未见同等清理 | DELETE 软件 | `SoftwareService.cs:DeleteAsync`；`W/.../admin/Softwares.vue:deleteSoftware` | 静态 | 文件系统和数据库非同一事务；共享路径或自定义上传根影响未测 | P2，排除直接采用 |
| E18 | 上传先校验软件存在再写文件，目录由软件名派生，重名加秒级时间；绑定路径不存在仍入库大小0；两种新增均Pending、可见、默认并清除旧默认 | 后端可访问路径 | `VersionService.cs:CreateAsync,UploadAsync,ClearDefaultVersionsAsync`；`AdminFilesController.cs:BindFile` | 静态 | 未限定根目录、未拒绝重复路径，上传写文件在DB事务前；失败遗留和覆盖风险待隔离验证 | P1/P2 |
| E19 | 默认版本唯一部分索引；迁移为每软件最新可见版补默认；创建/设默认/隐藏或删除默认采用事务更新 | 对应迁移实际应用后 | `Data/Configurations/SoftwareVersionConfiguration.cs`；`Migrations/20260704092441_AddDefaultVersionFlag.cs`；`VersionService.cs` | 静态 | 迁移文件不证明当前DB有索引；并发锁、唯一性冲突未运行 | 待目标依据 |
| E20 | 隐藏版本不可设默认；删除/隐藏默认选最新可见替代，无可见版则无默认；显示且无默认时提升；列表默认优先再创建倒序 | 有效0/1状态 | `VersionService.cs:DeleteAsync,ChangeVisibilityAsync,SetDefaultAsync,PromoteLatestVisibleVersionAsync`；public/admin详情 | 静态 | 显隐成功UI只改当前isVisible，未刷新默认标识；删除UI也未重新加载替代默认 | P1/P2 |
| E21 | 新建先存软件，再上传/绑定，第二步失败保留软件并提示部分成功；编辑可改日志、显隐、默认；页面版本删除固定physicalDelete=true，API默认false | 编辑页正常交互 | `W/.../admin/SoftwareDetail.vue:save,handleUpload,deleteVersion,submitEditVersion`；`AdminVersionsController.cs` | 静态 | 页面无“只删记录”选择；手工新建版本号校验与已存软件模式不同；物理删除失败仍成功响应 | P1/P2 |
| E22 | scan支持自定义路径、递归扫描，路径反斜杠替换后以字符串HashSet排除已绑定；自定义目录缺失返回空，默认缺失则创建；扫描页绑定按钮只alert；编辑页可手工bind | 授权管理员及文件权限 | `FileService.cs:ScanUnboundFilesAsync`；`AdminFilesController.cs`；`W/.../admin/Files.vue:openBindDialog`；admin详情 | 静态 | 路径大小写/相对绝对/别名未统一；目录拒绝/扫描中文件变化会抛错；不是完整扫描一键入库流程 | P1/P2 |
| E23 | 单循环选最早Pending，先保存Computing，再流式算哈希，成功Done、缺文件/计算异常Failed；空队列等5秒，外层错误等10秒 | HostedService活动期间 | `S/BackgroundServices/HashComputeService.cs:ExecuteAsync` | 静态 | 仅选择Pending，未找到Computing重启恢复、Failed重试入口、多实例抢占；取消和并行删除待验证 | P2，后续待办 |
| E24 | 单容器托管SPA和持久上传；分容器Web Nginx只有静态/回退，无/api及/uploads代理；发布脚本包含push，sh还先执行reset --hard、pull及用途未明的ch * | 仓库内部署定义原样使用 | `Dockerfile.Synology`、`Dockerfile.Web`、`Dockerfile.Server`、两份compose、publish脚本 | 静态 | 外部代理是否补足未知；不能把默认配置当生产配置或宣称分容器全链可用；sh会丢弃已跟踪工作区改动，本轮未执行 | P2，后续待办 |
| E25 | 看板API有软件、版本、分类、平台和版本下载计数之和；UI显示软件/下载/分类/平台四项，不映射versionCount | `/api/admin/dashboard/statistics` | `AdminDashboardController.cs`；`DashboardService.cs:GetStatisticsAsync`；`W/src/views/admin/Dashboard.vue:28-43` | 静态 | 旧PRD要求总版本/文件数页面展示，尚未实现；总存储和Top10也未找到完整聚合/页面；软件冗余计数和版本和可不一致 | P1/P2、BR-001 |
| E26 | WeatherForecast经MapControllers可定位；未见鉴权；testDelete会尝试删除首个软件，登录URL与当前不同 | 原仓库文件未修改 | `Controllers/WeatherForecastController.cs`；`W/testDelete.js` | 静态 | 没有将脚本运行或当测试通过；无外部消费者清单 | 排除业务采用 |
| H01 | HEAD新增版本的两个入口均改为清旧默认并置新默认，且加事务 | `57e2847`实际diff | `git show 57e2847 -- .../VersionService.cs` | 历史 | 当前逻辑仍在；提交不能证明历史业务批准 | P1 |
| H02 | 早先提交加入默认版本设置、相关迁移和UI、下载排序、免责声明、拖放与预览 | `0c8662f`统计及当前代码对应 | `git show 0c8662f --format=fuller --stat`；对应当前E04/E05/E20/E21 | 历史 | 本轮只核对统计及当前相关代码，未把提交标题当历史批准 | P1 |

## 冲突、未知项与讨论决定

| 问题 | 来源及影响 | 当前处置 | 人的实际决定与依据 |
| --- | --- | --- | --- |
| P1 维护目标来源 | v1未找到旧PRD的历史认可范围，仅有目录和简称确认 | v1公开等待用户决定；v2按D1采用固定快照中已核实且无例外行为 | 用户本次确认解除目标依据阻塞，正式Spec仅覆盖D1/D2 |
| P1a 认证与内容模型 | Cookie/JWT、多平台/单平台、Markdown/HTML的冲突；新增多标签和分享 | 按D1以JWT、单平台、多标签、HTML和分享作为当前维护目标；HTML安全问题仍排除 | 已确认，E05/E09/E10/E14-E16 → F-ACC/CAT/SW/MET-001 |
| P1b 版本体验和默认 | 最新三版/折叠/Markdown日志对全量/普通文本/默认优先/新增覆盖旧默认 | D1采用已核实当前版本体验；H01仍只是历史演变证据 | 已确认，E18-E21 → F-CAT/VER-001；不重认历史批准 |
| P1c 未完成的旧目标 | 首页最新/最热排序切换、看板版本总数展示、扫描页绑定占位、独立相册灯箱、Top10、存储量、仅删记录UI、深层分类导航 | D1明确保留未来待办B09；不算已实现，也不写成从来不需要；前两项为v3依BR-001补齐 | 已确认当前接入排除，后续业务细化仍须按真实决定推进 |
| P2 发布与管理边界 | 公共版本/下载未查父状态、公开物理路径；匿名seed；库外路径 | D2确认三个结果边界；R06/R21-R24补证，保留B01-B03待修 | 目标已确认，实现未修；公开物理路径披露另列B01，不捏造修复细节 |
| P2 计数、异步与文件一致性 | 未await及非原子计数、任务恢复缺口、文件/DB非原子、共享路径 | D1/D2均未批准这些现状，维持B04/B05；正式Spec注明适用条件 | 仍未修复，业务计数及恢复/删除新选择不在已确认范围；不假称获准S2暂缓 |
| P2 校验与展示一致性 | 非法输入、分类间接环、默认标记过期、错误页面不完整 | 保留B06-B08；正常有效输入及已明确拒绝规则可进入Spec，异常缺口不采用 | 未关闭，不以编译或31项观察宣称完整校验 |
| P3 部署与旧资料完整性 | 真实部署/本地DB/其他分支/DOCX/外部消费者未知 | 保留B10，隔离探测只补新库和临时运行证据 | 未调查范围仍明确，不以隔离成功覆盖生产 |

上述分组是调查事项编号，不是独立审查的问题分级或结论。没有以“采用现状”追认历史批准，也没有撤销旧目标。

## 目标、需求与证据追溯

以下八项为第2轮通过的维护基线接入范围，不是实现符合或AC全部通过清单。REQ/AC正文只维护在链接主规格；R编号表示实际观察，与AC逐项通过不是同义。每行T4负责目标表达，T5负责补证与交审，T6负责最终记录及后续本地提交。第2轮对CAT/SW/DSH及受影响依赖补查，其余范围确认输入未变后复用首轮独立证据，见完整复查原文。

| 能力与主规格 | REQ及对应同尾号AC | 目标依据 | 调查证据 | 实现差距/未补证范围 | 独立结论 |
| --- | --- | --- | --- | --- | --- |
| [F-ACC-001](../../specs/access/F-访问控制-001-access.md) | REQ/AC-ACC-001-001～004 | D1：001/002；D2：003/004 | E01/E05-E10/E12；R01-R06/R20-R22 | B01/B02；浏览器、过期/密钥错误配置未实测 | 第1批第2轮通过，复用首轮及必要依赖核对 |
| [F-CAT-001](../../specs/catalog/F-软件浏览-001-browse.md) | REQ/AC-CAT-001-001～004 | D1；公开边界依赖ACC | E02-E08；R09/R10/R17-R22 | B07-B09；BR-001/002已复查关闭；页面/剪贴板/大小写排序仅静态 | 第1批第2轮通过，BR-001/002复查 |
| [F-MET-001](../../specs/metadata/F-基础字典-001-metadata.md) | REQ/AC-MET-001-001～004 | D1 | E11-E14；R07/R09 | B07；查重、删除、失败路径仅静态 | 第1批第2轮通过，复用首轮及必要依赖核对 |
| [F-SW-001](../../specs/software/F-软件档案-001-software.md) | REQ/AC-SW-001-001～005 | D1 | E15/E16/E21；R08/R20 | B06/B07；BR-002已复查关闭；其余限制保留 | 第1批第2轮通过，BR-002复查 |
| [F-VER-001](../../specs/versions/F-版本管理-001-versions.md) | REQ/AC-VER-001-001～005 | D1；ACC/FIL依赖D2 | E06-E08/E18-E21/H01；R11-R19/R26/R29/R30 | B01/B03/B04/B07；上传、并发和失败回滚未实测 | 第1批第2轮通过，复用首轮及必要依赖核对 |
| [F-FIL-001](../../specs/library/F-软件库-001-library.md) | REQ/AC-FIL-001-001～003 | D2：001；D1：002/003 | E18/E22；R23-R26 | B03/B04/B09；库内递归、目录创建及故障仅静态 | 第1批第2轮通过，复用未变首轮证据 |
| [F-HSH-001](../../specs/hash/F-文件校验-001-hash.md) | REQ/AC-HSH-001-001～002 | D1 | E23；R27/R28 | B05；调度先后、读取异常和周期仅静态 | 第1批第2轮通过，复用未变首轮证据 |
| [F-DSH-001](../../specs/dashboard/F-后台概览-001-dashboard.md) | REQ/AC-DSH-001-001～002 | D1 | E25；R31 | B04/B09；BR-001已复查关闭；逐值及空库聚合、页面仅静态 | 第1批第2轮通过，BR-001复查 |

## 文档方案与任务

依据队长原派工整理事实、能力范围、依赖、证据、目标来源及Git回执。文档职责采用 sdd-documentation 及公共、Change、契约引用规则，来源代码/迁移保持原位置。

- [x] T1：固定实际规则、仓库、基准、未提交输入、配置与安全运行边界；见本记录快照及E01/E24/E26。
- [x] T2：沿主要入口完成第一轮静态链路调查及能力地图；动态验证、未调查内容分别保留。
- [x] T3：建立合规任务分支，整理事实记录、项目入口及导航；不包含业务目标草稿。
- [x] T4：用户已直接确认D1/D2，整理八份正式Spec、29条REQ和29条AC及覆盖导航。
- [x] T5a：在隔离副本中补核接口与文件证据，完成31项观察，保留未实测路径及限制，见下节。
- [x] T5b：队长已安排第1批首次独立审查并取得第1轮正式结论，两项S2有保留；见下文原文引用。
- [x] T5c：按派工修订BR-001/002及必要引用，回传v3材料；作者处置为已修正待复查，不代审查者关闭。
- [x] T5d：队长恢复派工后，原审查者交付本批第2轮正式结论，调查可靠性及正式接入条件通过，两项已复查关闭；累计已完成2轮，初始化失败不计轮次。
- [x] T6：v4结论记录、导航及检查交回后，队长已核对并安排仅本地提交；2026-10-21原作者逐项暂存17文件完成首个本地提交，再如实同步完成状态及必要导航。提交后收尾仍限任务文件本地提交，最终提交及父提交哈希、检查和剩余工作区状态见DOWN-1平台回执；不在文档内自引用commit哈希。

## 实施结果与计划偏差

v1新增本记录、项目说明和变更索引及README导航；v2记录真实决定，新增八份正式Spec和规格索引、隔离探测材料，更新相关导航。未修改AGENTS、旧PRD、设计资料、业务代码、测试、迁移或配置；未创建未确认目标草稿。

v3仅修改README、project、两个索引、本文和CAT/DSH/SW三份Spec，新增第1轮审查原文的只读副本。保留全部稳定F/REQ/AC、E/H/R/B及BR编号，八项能力仍为29条REQ/29条AC；不新增排序选项或界面功能。详细逐项处置见审查章节。

v4仅修改README、project、两个索引及本文，新增第2轮审查原文的完整只读副本。八份Spec、29条REQ/AC、D1/D2、E/H/R事实、probe及observations与已审v3相同；没有业务、规格或原始调查证据的实质改动。本次记录整理不产生第3轮正式结论，也不要求自动新增审查；如后续输入或提交钩子实质改变上述内容，须交回队长协调受影响复查。

仓库及目标来源阻塞已解除；v2安全隔离补证已完成本节列明范围，第2轮独立审查已通过声明的接入条件。2026-10-21最终记录与必需检查完成，按现状基线门禁记录该范围verified；v4当时未提交的事实保留于原包及平台回执，v5已按T6安排本地提交。Issue继续in_progress，待队长核对提交回执，不宣称全项目接入全部结束。覆盖表继续保留部署、其他分支和未实测分支，不以有限运行覆盖原全项目目标。

## 实际验证

审查模式：作者调查与文档自检；独立审查第1批累计2轮，第1轮有保留、第2轮通过声明范围的调查可靠性及正式接入条件。以下构建及31项观察为v2历史，v3/v4不重新宣称执行。
代码快照：`57e28479bda434f1e9f507872ff16c53fcfb1830`，已跟踪业务文件无差异。
规格基线：接入v3八份approved Spec，v4不变，来源仍为D1/D2；approved不等于实现符合或调查审查通过。
环境与日期：历史调查/构建/探测为2026-10-20；本次最终文档整理和核验为2026-10-21（Asia/Shanghai），Windows PowerShell。历史工具版本：.NET SDK9.0.100、Node v22.12.0、npm10.9.0、Python3.13.2，已安装Microsoft.AspNetCore.App8.0.11及Microsoft.NETCore.App8.0.7/8.0.11。此为原project所载历史探测移回本记录，不冒充2026-10-22重新探测。
一致性结论：第2轮原审查者确认v3在已声明范围满足调查可靠性及正式接入条件；v4仅如实同步该结果、问题状态及导航，八份Spec与原始证据不变。文档自检支持结构、引用和追溯一致，不证明代码符合全部目标；历史偏差、未调查范围和运行限制继续保留。

| 检查 | 命令或步骤 | 实际结果 | 限制 |
| --- | --- | --- | --- |
| 仓库和已有差异 | rev-parse、status、diff、diff --cached、branch -vv | 已核对基准、HEAD、空暂存区和四项未跟踪输入 | 未fetch、未确认生产版本 |
| 分支规则 | check-ref-format；从固定HEAD执行switch -c | 格式校验成功，升级工具权限后创建成功 | 不代表已提交 |
| 依赖与工具 | csproj、package.json；Node JSON.parse锁文件；工具version命令 | 得到声明/锁定版本及本机工具版本 | PowerShell5解析锁文件失败后改用Node成功；不据此断言运行加载版本 |
| 来源与历史 | PRD文本/技能、相关设计章节、log及两个相关提交 | 静态事实及来源冲突已记录，D1/D2处置来源缺口 | DOCX和UIUX全文未比对，不把历史标题作批准 |
| 后端隔离构建 | 复制git跟踪Server `.cs/.csproj`到临时目录，复用obj依赖解析缓存；`dotnet build <隔离项目> --no-restore --configuration Release --nologo` | 成功，0警告、0错误，SDK9.0.100及net8.0运行时 | 不复制仓库DB、配置或安装包；不是生产构建或发布 |
| 前端构建 | Web目录`npm run build`，执行vue-tsc和vite build | 成功，1888模块，Vite报告18.08秒 | 初次沙箱esbuild spawn EPERM失败，升级执行权限后成功；Browserslist数据过期提示保留，未升级依赖 |
| 隔离HTTP与任务 | 下述probe.ps1，新临时SQLite及临时文件，绑定loopback | 最终31项观察符合调查断言，其中6项明确观察现存偏差 | 不是29项目标AC通过；浏览器与真实NAS/生产未测 |
| v1文档检查（历史） | check_sdd.py及Node相对链接检查 | v1识别0份Spec、1份Change，结构问题0，4文档缺失链接0 | 不复用旧计数作为v2检查；v2结果见交付核验 |

### v2交付核验

- 文档技能结构检查实际识别8份规格主入口、1份变更记录，结构问题0个。
- 独立于该脚本的Node读取检查覆盖README及docs内13份Markdown：85个本地相对链接及显式锚点无缺失；29条REQ定义唯一，29条AC定义唯一且引用全部29条REQ；无行尾空白。此为作者表达与导航自检，不是独立业务审查。
- 再次核对仓库根、任务分支、HEAD/master/本地origin/master均符合本记录；`git diff --exit-code -- 03_src 01_需求 02_设计 .agents`无差异，`git diff --cached --exit-code`确认暂存区空。原4项未跟踪输入及2份appsettings哈希与快照一致，仓库DB及业务代码无改动。
- `git diff --check`退出0；README有常规LF未来转CRLF提示，无空白错误。未跟踪docs另由上述检查覆盖；README/docs为任务文档改动，尚未提交。最终附件包含完整v2正文和原始观察，不包括凭据或原数据库。

### v3修订核验

2026-10-20，沿用既有任务分支，首次修改前已重新核对真实仓库根、master/HEAD/本地origin/master、工作区及空暂存区；v2包内15文件全部与当时工作区一致。

- 结构检查实际识别8份Spec、1份Change，问题0；重新生成的Node检查覆盖14份Markdown、86个本地链接/显式锚点、29条唯一REQ及29条唯一AC，无断链、缺失引用或行尾空白。新增的第1轮审查原文纳入本次读取范围；它不被当作另一份规格。
- BR-001依据旧PRD文本第16/34行及现有页面静态核对，BR-002依据共享SoftwareService第71-78行及公共/后台Controller调用核对。未启动应用或另做排序HTTP探测；v2的构建、31项观察及第1轮独立复跑均按历史来源保留，不称为v3新实测。
- `git diff --check`成功；`git diff --exit-code -- 03_src 01_需求 02_设计 .agents`和`git diff --cached --exit-code`均无差异。HEAD与分支不变，未暂存、提交、push或合并。
- 当前作者运行的AGENTS、daemon_task_context、资源文件、`.rar`及两份appsettings哈希与本文作者初始读数一致；第1轮审查者运行时记录的运行身份文件变化仍按其原文保留，不宣称跨Agent运行期间恒定。既有协调/审查副本及本轮`.multica/adoption-revision-b1-r2/`均为运行材料，不纳入待提交项目产物。
- v3包内含16个文件；相对v2修改8个文档，新增1个第1轮审查原文副本，其余7文件字节不变。包内文件与工作区逐一核对，最终包哈希、逐文件SHA-256清单和完整实际差异以返交附件固定版本；这些核对不替代第2轮独立复查。

### v4最终记录核验

2026-10-21（Asia/Shanghai），沿用现有任务分支，写入前再次核对真实根、master/HEAD/本地origin/master、工作区和空暂存区；v3清单的16份文件全部匹配工作区。第2轮原文通过认证CLI取得，副本SHA-256与原件相同；来源和关闭决定见下文。

- 写入检查：仅记录有来源的第2轮结论及状态，未改变目标、规范强度、编号、E/H/R事实或例外；首轮结论、原S2等级、v3作者处置、D1/D2和B01—B10完整保留。共享规则仍只在原Spec定义，导航摘要引用本文，不产生第二套权威目标。
- `python <sdd-documentation技能目录>/scripts/check_sdd.py .`实际输出：识别8份规格主入口、1份变更记录，结构问题0。
- 切换verified后的首次结构检查提示缺少显式“一致性结论”验证上下文；按既有事实补齐该标签后重跑，不改写业务、规格或审查结论。下述最终检查输出对应补齐后的文件。
- `node .multica/adoption-revision-b1-r2/check-docs.cjs .`实际输出：15份Markdown、87处本地相对链接/显式锚点、29条唯一REQ及29条唯一AC，errors为空。追溯表八项能力继续关联原主规格、D1/D2、E/H/R和B待办，第2轮状态与原文一致。
- `git diff --check`退出0，仅常规LF将转CRLF提示；未跟踪docs的行尾空白及链接由上述检查覆盖。`git diff --exit-code -- 03_src 01_需求 02_设计 .agents`及`git diff --cached --exit-code`均退出0，未改源码、原需求/设计、配置及暂存区；未构建、启动服务或新增探测。
- 当前作者运行的AGENTS哈希为`95BE073508849C6C4F13C906DDB82843854FF060517C7DC4395B9A7A09D82304`，由运行时更新，与初始值及两轮审查身份文件分别保留；daemon_task_context、资源JSON、`.rar`仍与作者初始值相同。运行身份变化不作为业务证据变化，未编辑或纳入提交。
- 最终记录相对已审v3仅5份导航/记录修改及1份完整复查原文新增，原16份中的其余11份保持字节相同，包括全部八份Spec和原始探测材料。最终完整包、17份文件哈希、v3→v4实际差异及本轮实际检查输出随交接附件提供；运行材料`.multica/`不纳入提交。

v4上述检查仅支持文档结构、导航和状态来源一致；独立审查引用原审查者实际结果，不由作者自检替代。当时最终核验后仍未暂存或提交，由队长接收固定文件清单再安排；该历史不改写为提前获得授权或提前提交。

### v5 T6本地提交与记录收尾

2026-10-21，依据队长T6派工，提交前再次核对分支、真实根、HEAD/master/本地origin/master及空暂存区；v4的17份文件全部与工作区SHA-256一致。保留用户Git身份，未设置身份覆盖；`.multica/`、`.rar`和运行时AGENTS等无关输入均不纳入暂存或提交。

先将清单内17份文件逐项暂存，核对暂存路径集合及各文件经Git属性处理后的blob与工作区一致，再完成首个本地提交。提交后17份工作区SHA-256仍与v4相同，HEAD中的blob匹配输入，暂存区为空，已跟踪工作区无差异；未发现提交或钩子引起产物变化。沙箱最初拒绝创建`.git/index.lock`，通过工具权限升级后按原清单执行，没有绕过权限或清理已有文件。

在首个提交实际成功后才标记T6完成、更新导航，v5相对v4仅修改README、project、两个索引及本文的提交事实和当前状态。其余12份文件不变，包含八份Spec、两轮审查原文及probe/observations；D1/D2、E/H/R、B01—B10、BR状态和验证限制均不变，未改变业务、规格或调查证据的实质内容。非实质收尾不新增审查轮次；如后续输入实质改变则交回队长协调原审查者复查。

本轮先后执行文档结构、链接/锚点、REQ/AC唯一及覆盖、工作区与暂存空白、源码相对原基准不变及暂存归属检查。实际命令、输出、17文件清单/哈希、相对v4差异、每个commit完整哈希与父提交及最终状态随平台回执交付；提交后收尾检查与实际成功结果以该回执为准。没有新增产品构建或运行探测，不将历史31项观察改称本轮实测。

首个提交固定v4全文；本节与导航的提交后收尾仍仅本地提交任务文件，不push、合并或删分支。commit哈希不写入本文，避免自引用。最终可访问完整包与逐文件哈希保留平台附件，Issue继续in_progress，交队长核对回执后再交付。

### 隔离探测环境与原始材料

2026-10-20约23:00～23:10 +08:00，在本任务临时目录建立独立Server副本。只复制`git ls-files`定位的`.cs/.csproj`及已有obj中依赖解析缓存；不复制或使用原库、仓库appsettings或管理员凭据。构建DLL的SHA-256为`24E078CEB9FB49346E194170AE3A0B2B5B907A4AEEB9B0CCF3768B53842CFE29`，其输入仍为固定HEAD，无业务源码差异。

环境覆盖`ConnectionStrings__DefaultConnection`为新临时SQLite，`AppSettings__StorageBasePath`和`UploadBasePath`分别为临时目录，密码和JWT密钥为当次随机值，JWT有效期1小时，Production，动态空闲loopback端口。库外验证文件也在同一个临时运行目录的另一子目录内，未读取宿主其他文件。有效配置覆盖不同于仓库默认值，因此运行结果仅证明对应代码路径在该隔离配置执行，不能冒充默认部署结果。

材料：[探测脚本快照](evidence/probe.ps1)、[最终观察JSON](evidence/observations.json)。脚本是调查夹具，不是新增产品测试或产品源代码；需在有已构建`server/bin/Release/net8.0/DownloadStation.Server.dll`的隔离布局运行，不能直接从仓库Server工作目录执行。脚本不输出令牌/凭据；临时服务由同一前台脚本持有，finally读取daemon PID后停止自己启动的精确子PID，最终输出`SERVER_STOPPED True`。未保留需用户维持的服务。

第一次探测在R08停止：夹具误按`platformId`读取软件详情响应，实际契约为`platform.id`；核对DTO后仅纠正夹具字段，再用全新临时库完整运行。未改变应用、业务断言或测试以掩盖产品问题。最终运行记录为`run-883fae47b2ae4040b02ae1fc0e52b654`，31项true。

| R编号 | 触发及实际观察 | 证据性质/目标关系 |
| --- | --- | --- |
| R01-R05 | 新库公共列表空；匿名管理读取/创建401；错误密码HTTP200/body401；随机正确密码签发JWT | 运行；支持ACC部分路径，不覆盖浏览器/过期/短密钥 |
| R06 | 新库匿名POST seed后公共软件总数0→1 | 运行偏差；违反REQ-ACC-001-003，B02 |
| R07-R10 | 认证创建分类父子、平台、标签及软件；单平台和标签返回；父分类+平台+关键词共同命中；不存在详情404 | 运行；支持MET/SW/CAT部分路径 |
| R11-R16 | 两次绑定新默认覆盖旧默认；隐藏默认提升替代；隐藏版设默认拒绝、下载404；全隐藏无默认；显示后恢复默认 | 运行；支持VER顺序状态切换，不覆盖并发/页面标记 |
| R17-R19 | 文件完整字节相等；Range 0-2返回206及三个准确字节；错误父软件ID下载404 | 运行；支持VER下载，不证明计数正确 |
| R20-R22 | Draft详情404且列表排除；但公共版本仍200且含路径，下载仍200 | 运行偏差；后两项违反REQ-ACC-001-004，B01 |
| R23-R24 | 指定库外临时子目录scan200返回文件，bind接受该文件 | 运行偏差；违反REQ-FIL-001-001，B03 |
| R25-R26 | 缺失文件仍成功绑定、size=0；下载该版404 | 前项为运行偏差B04；后项支持VER缺文件拒绝，不批准缺文件绑定 |
| R27-R28 | 可读文件最终Done且SHA256与独立Get-FileHash一致；缺文件最终Failed | 运行；支持HSH，不证明处理中间态次序/恢复/多实例 |
| R29-R30 | 删除默认记录提升最新可见版本；API未传physicalDelete时保留物理文件 | 运行；仅顺序记录生命周期，物理删除失败未测 |
| R31 | 认证统计API返回200 | 运行可达；字段逐值、空库公式和页面仍仅静态 |

### 审查和交接

正式基线审查仍为第1批，累计已完成正式结论2轮，本批最多3轮。v1及v2最初交审时为0轮、v3作者修订时为1轮的历史保留于原包及评论；初始化失败没有形成结论，不计轮次。本次v4非实质记录整理不算第3轮。原审查者Agent为`b2c66b19-5892-49c9-9a86-a7e2bb95566f`，与作者独立，两轮均由该原审查者形成正式结论。

第1轮原结论：调查可靠性“有保留”；正式接入条件“有保留，尚未满足无保留接入条件”。接入产物未关闭数量S0=0、S1=0、S2=2、S3=0，两项已证实且原状态待处理，未获暂缓。原文为[第1批第1轮完整审查](evidence/baseline-review-b1-r1.md)，平台评论`01a159f2-0642-74a6-8566-89fce7542890`，附件ID`01a159f2-01bc-702c-be93-a63caed55c57`，文件SHA-256为`4EA29EB9F2B4AF581D1978B103ABA26FBA0EB7D697CD78121306135F5DCF6575`。副本与认证下载原件字节相同，未改写审查者结论。

该轮输入为v2包SHA-256 `173479FDE909CAABA49A5CBE70B93E808D1D94C41F54C0230EADF1A056D30E0C`，Change SHA-256 `0983E2CFA22700C097E51CB44971770D71550249BDC23D1E649BACE0FC8B5B11`。v3首次修改前再次将包内15文件与工作区逐字节哈希核对，全部一致。审查者报告独立构建及复跑31项观察成功，限制与历史缺陷等级详见原文，不把该报告改写成作者v3新实测。

### 第1轮问题的作者处置

2026-10-20，依据队长修订派工、D1现状采用及旧目标保留授权，处理如下。这是v3交审时的历史处置：当时两项均仍计入未关闭S2，处置状态为“已修正待复查”；作者没有自行判定关闭，也没有S2暂缓决定。2026-10-21的实际关闭来源另见下节，不改写当时记录。

| 稳定编号 | 第1轮等级/类型/确定性 | 具体修订及依据 | 待原审查者核对 |
| --- | --- | --- | --- |
| BR-001 | S2，记录缺口，已证实 | B09、P1c、覆盖表补齐首页最新/最热切换及看板版本总数页面展示；CAT范围、DSH范围及REQ/AC-DSH-001-002、规格索引明确保留旧目标。依据PRD文本:16/:34、Home.vue完整模板与filters:212、Dashboard.vue:28-43 | 两项未算已实现或取消；API支持sortBy/versionCount与UI入口明确区分；无新功能实施 |
| BR-002 | S2，规格还原不符，已证实 | E03及REQ/AC-CAT-001-002按SoftwareService.GetPagedListAsync:71的ToLower后比较还原；涵盖popular/POPULAR/Popular及缺省/null/空串/其他值。REQ/AC-SW-001-002引用同一权威规则，不在后台复制另一套排序 | 公共/后台共享归一化和回退分支，默认首页参数保持不变；未新增排序选项，未另做HTTP排序实测 |

历史实现缺陷与接入文档问题分开：第1轮对B01/B02/B03各保留S1、B04中的R25子项为S2，均待后续处理；不把B04其余复合问题一并降级，也不将接入产物S0/S1为0理解为产品无重大缺陷。

v3交接历史：包含README、project、两个索引、八份完整Spec、本文、原证据夹具/观察JSON及第1轮审查原文；源码/契约仍在原位置。当时Issue保持in_progress、Change保持implementing，第2轮尚未安排或产生正式结论，未提交。v3包SHA-256为`81728EA2DD117C9CD550ED1FC394BA9F3AFAF3F2B73193F2B01928E5FB348F83`，清单为`2808DABAB170A850895EF595C9A2E71FFDE050C835A3AA78B5A9337B5487D3D0`，Change为`E65882240CA1407BDFA866667BBC073C03BA83C099940ECCF5D4A18F6F2F70FC`；最终记录未覆盖该已审输入。

### 第2轮正式复查及最终接入记录

2026-10-21（Asia/Shanghai），原审查者对v3形成正式结论：调查可靠性“通过，限已声明的调查范围、证据类型和配置边界”；正式接入条件“通过本轮独立基线审查”。接入调查及文档未关闭S0=0、S1=0、S2=0、S3=0。完整原文为[第1批第2轮独立基线复查](evidence/baseline-review-b1-r2.md)，平台结果评论`01a15a4b-ac18-7d21-b688-8d96d7753c4d`，附件ID`01a15a4b-ab35-76a7-9125-96cb5f2f1896`，SHA-256 `8B4763A68CDA322F2D7159773DF49A4BC47ED680BEB6941F1A622519C70047CB`。通过认证CLI下载并逐字节保存，不改写原文末尾当时Change为implementing等历史事实。

| 稳定编号 | 保留的原等级/确定性 | 第2轮独立状态与依据 | 未关闭的实现事项 |
| --- | --- | --- | --- |
| BR-001 | S2，记录缺口，已证实 | 已复查关闭；原审查者核对PRD:16/:34、Home/Dashboard及服务，确认B09/P1c、CAT/DSH及导航已区分API/UI和旧目标待办 | 两项UI功能仍在B09，关闭的是接入记录遗漏，不是功能实施 |
| BR-002 | S2，规格还原不符，已证实 | 已复查关闭；原审查者核对共享服务ToLower分支、两个Controller及CAT/SW的REQ/AC，确认归一化、回退和单一权威引用 | 大小写HTTP及多页排序未实测，不把静态复查当作新运行证据 |

第2轮复用了未变首轮范围的独立构建及31项观察，保留原日期和运行标识；本轮作者仅核对报告、输入哈希、记录与导航，不将其称为作者新实测。D1/D2继续适用，未新增人工决定或S2暂缓。B01/B02/B03各S1、B04中R25子项S2继续未修复，其余B项及全部验证限制不变；这些不等同本次接入产物的未关闭问题数量。

按队长最终整理派工，2026-10-21声明范围的必需写入检查完成，记录现状基线verified。该状态只表示固定快照和目标依据下接入调查、文档及独立基线检查成立，不证明历史实现符合Spec、29条AC已通过、产品验收、合并或发布；不扩大到覆盖表中的未调查/未验证范围。v4当时待队长安排本地提交；现按T6派工完成本地提交和记录收尾，Issue继续in_progress，提交回执交队长核对。

### 未执行项与限制

后端隔离Release构建、前端构建及上述HTTP探测已执行；其余仍未执行：浏览器交互、原始DB/schema与历史迁移核对、真实NAS/部署/恢复、容器、并发压力、文件写入故障/物理删除故障和崩溃恢复。没有执行testDelete、发布脚本或远端操作。没有产品安全性、性能或可靠性达标结论，也没有产品验收要求或结果。

本次没有获得S2暂缓或其他例外放行决定。已知历史实现问题不必在接入时全部修复，但目标依据、重要路径证据和独立审查缺口不能以此豁免。

## 后续待办

以下B编号是待办定位，不是独立审查等级；均未修复、未以S2暂缓放行。历史缺陷不自动阻塞证据充分的基线接入；关键调查缺口仍须由审查核对是否阻塞相应范围，不以“后续待办”豁免。

| 待办 | 范围/影响与依据 | 当前边界及下一动作 |
| --- | --- | --- |
| B01 | 下架版本查询/下载绕过父状态，VersionResponse暴露FilePath；E06/E07、R21/R22 | D2已明确拒绝下架关联访问，移交后续修复；物理路径披露另待处理，不采用为公共目标 |
| B02 | 匿名seed可写分类、平台及样例软件；E12、R06 | D2已明确管理写认证，保留待修，不是允许的公开初始化功能 |
| B03 | scan/bind缺根目录限制已复现，上传文件名/软件名和读取/删除边界静态可疑；E18/E22、R23/R24 | D2确认库内操作目标；后续修复须覆盖全链，越界上传/删除未探测 |
| B04 | 下载计数生命周期/并发/Range语义、文件先写后DB、共享路径、缺文件绑定、删除吞异常；E08/E17/E18/E21、R25/R30 | 排除采用这些缺陷；计数语义、文件异常恢复及删除新决策由后续负责方直接讨论，不声称已解决 |
| B05 | Computing崩溃后不恢复、Failed无重试、多实例无抢占；E23 | 只整理已证实单实例持续运行计算；恢复/重试机制后续细化 |
| B06 | 图片扩展/内容白名单与HTML清理未见；E05/E16 | D1采用HTML格式不批准不安全内容；后续核实处理策略，未进行安全攻击测试 |
| B07 | 非法分页/枚举、同名唯一性、分类间接环、缺失分类删除成功、默认标记过期及API/页面错误状态不足；E03/E04/E11/E14/E15/E20/E21 | 正常及已明确拒绝规则进入Spec；异常和并发缺口保持未定，不把无错误提示/错误500设为目标 |
| B08 | 同组件详情参数无watch、首页响应乱序及加载更多失败页码；E04/E05 | 静态疑点待浏览器隔离复核，未作为可接受行为 |
| B09 | 旧目标未完成：首页最新/最热排序切换（PRD文本:16，Home.vue固定popular且无控件）、看板版本总数展示（PRD文本:34，Dashboard.vue未映射API的versionCount），以及扫描页一键绑定、独立相册/灯箱、Top10、总存储、仅删记录UI、深层分类导航/折叠等 | D1明确保留未来待办，不算已实现或已取消；前两项依BR-001补齐，未扩展为本轮实现；新行为另按后续规格流程 |
| B10 | 生产配置/版本、NAS、旧DB、测试分支、DOCX、外部消费者和分容器代理；E24/E26及覆盖表 | 未调查，后续按授权核对；当前Spec不能作为实际部署版本证明 |

下一步：队长核对T6实际提交回执、最终完整包、17文件哈希及相对v4的非实质收尾差异后再交付。第1批累计正式审查2轮，非实质整理不自动新增轮次；若实质输入变化则交回协调受影响复查。本轮仅本地提交任务内容，没有push、合并或删除分支。

[正式Spec入口](../../specs/index.md)与本文已应用并按T6安排本地提交到任务分支，尚未合并回master。源码快照、目标认可、文档检查、独立基线审查、实现符合性、人工验收和生产上线分别判断。
