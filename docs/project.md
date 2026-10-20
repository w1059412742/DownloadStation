# 项目说明

## 项目目标与当前阶段

DownloadStation 是运行在个人 NAS 上的软件安装包元数据、版本文件和公开下载平台，包含匿名公开前台和受保护管理员后台。SQLite 保存元数据，上传图片、日志和软件包保存于配置的持久目录，最终通过 Docker 发布。

当前阶段：**四份 SDD Spec 已按当前代码实现正式确认，状态为 approved；已知缺陷、行为变更和生产发布验证继续按独立 Change 追踪。**代码现状证据见 [CHG-2026-008 代码确认矩阵](changes/CHG-2026-008-code-confirmed-baseline/evidence/code-confirmation-matrix.md)，正式确认记录见 [CHG-2026-009](changes/CHG-2026-009-current-baseline-approval/change.md)。

## 系统边界

```text
浏览器
  └─ Vue 3 SPA（公开路由 + /login + /admin/*）
       └─ 同源 HTTP/JSON + Bearer JWT
            └─ ASP.NET Core 8 API
                 ├─ Public/Admin Controllers
                 ├─ Services：认证、目录、字典、软件、版本、文件、标签、看板
                 ├─ HashComputeService：后台 SHA-256
                 ├─ EF Core 8 + SQLite
                 ├─ /uploads：图片和静态资源
                 └─ /data/software：NAS 软件包根目录
```

| 范围 | 事实权威 | 目标权威 |
| --- | --- | --- |
| 业务行为 | 当前控制器、服务、DTO、模型、迁移、消费者 | 对应 `docs/specs/*` 的 REQ/AC |
| HTTP 接口 | Controller、Request/Response DTO、序列化配置 | Spec 引用和独立 Change |
| 数据关系 | Models、EF Config、Migrations | Spec 的状态/完整性规则 |
| Docker 运行 | Dockerfile、compose、Program.cs、发布脚本 | F-OPS-001 与发布 Change |
| 来源追溯 | 原 PRD/UIUX、Git 历史、实际命令输出 | 接入 Change/evidence |

## 文档与技能入口

- [规格索引](specs/index.md)：F-CAT-001、F-DST-001、F-ADM-001、F-OPS-001。
- [HTTP 契约参考](contracts/http-api.md)：当前接口导航；不替代控制器和 DTO。
- [变更索引](changes/index.md)：接入、补齐、代码现状确认及后续 feature/fix/config Change。
- [代码现状确认矩阵](changes/CHG-2026-008-code-confirmed-baseline/evidence/code-confirmation-matrix.md)：逐项记录 53 个调查项的当前实现、证据和差异处理。
- [Agent 规则](../AGENTS.md)：项目级常驻入口、场景路由和状态门禁；详细流程、模板、TDD 与完成验证在 `.agents/skills/`。
- 项目内技能：四个场景技能、两个共享 SDD 技能，以及 `.agents/skills/test-driven-development` 和 `.agents/skills/verification-before-completion`，共八个；后续 agent 不依赖个人全局技能也能读取同一套执行规则。
- [完整来源覆盖与验收映射](changes/CHG-2026-002-sdd-baseline-completion/evidence/requirements-coverage.md)。

## Agent 规则分层

`AGENTS.md` 只保存每次任务都必须知道的项目入口、权威来源、场景路由和状态门禁；`.agents/skills/` 保存具体场景的步骤、文档规则、模板、脚本和验证要求；`docs/` 保存本项目的事实、目标和变更证据。这样不会把通用流程复制到入口文件，也不会把业务规则塞进技能。

## 工作流

1. 存量接入：固定版本和输入 → 调查入口/调用链/消费者/状态/文件 → 生成证据和 draft Spec。
2. 规格定义：讨论目标、边界、错误、并发、恢复和验收 → agent 写 REQ/AC/Change → 本地检查 → 真实确认后 approved。
3. 实施：approved Spec + planned Change → TDD 红/绿/回归 → 同步契约和文档 → 一致性审查。
4. 缺陷：复现 → 根因 → fix Change → 失败测试 → 最小修复 → 回归 → 一致性审查。
5. 发布：镜像构建 → 临时卷启动 → 健康/API/下载/上传探测 → 重建卷 → 备份恢复/回滚 → 保存证据；任何未执行项不得写发布通过。

## 当前能力和实现状态

| 能力 | Spec | 当前代码状态 | 后续处理 |
| --- | --- | --- | --- |
| 公开目录/详情 | F-CAT-001 | 代码现状已按 C01–C19 确认；截图/哈希/历史/主题等目标仍有差距 | 按 REQ 拆 feature/fix |
| 版本/文件/下载 | F-DST-001 | 代码现状已按 D01–D10 确认；扫描绑定、路径、恢复、计数和删除仍有差距 | 按 REQ 拆 fix/feature |
| 管理后台 | F-ADM-001 | 代码现状已按 A01–A14 确认；看板、截图、唯一性、资源清理和 seed 边界仍有差距 | 按 REQ 拆 feature/fix |
| Docker | F-OPS-001 | 单容器方向和容器路径已确认；脚本、健康、恢复和实测证据仍缺失 | 独立 config/fix Change |

这些状态是当前正式基线事实，不代表已知问题已经修复，也不代表 Docker/NAS 已发布。不存在的能力不能因为模型、接口或页面文件存在而标记已完成。

## 开发与验证命令

| 用途 | 工作目录 | 命令 |
| --- | --- | --- |
| 后端构建 | `03_src` | `dotnet build .\DownloadStation.sln --nologo` |
| 前端构建 | `03_src/DownloadStation.Web` | `npm ci; npm run build` |
| SDD 文档/验收门禁 | 仓库根目录 | `python .agents/skills/sdd-documentation/scripts/check_sdd.py .` |
| SDD 实施门禁 | 仓库根目录 | `python .agents/skills/sdd-documentation/scripts/check_sdd.py . --ready --spec F-...` |
| 技能门禁回归 | 仓库根目录 | `python .agents/skills/sdd-documentation/tests/test_check_sdd.py` |
| Docker 构建 | `03_src` | `docker build -f Dockerfile.Synology -t <registry>/<image>:<version> .` |

命令是否通过必须记录到当前 Change，不能把命令示例当实际证据。

## Docker 正式边界

正式发布基线采用群晖单容器：前端构建产物由 ASP.NET Core 托管，`/app/data` 保存 SQLite、uploads 和 logs，`/data/software` 保存安装包。前后端分离输入保留为开发/实验参考，除非另有 Change 补齐 proxy、CORS、上传卷、健康检查和实测证据。

发布必须注入 JWT 密钥、管理员密码、连接字符串和目录；示例中的默认秘密不可用于生产。数据库、图片、日志、软件包、配置和镜像版本必须作为恢复集合。应用迁移失败不得继续提供看似正常的服务，发布记录必须保存不可变标签、摘要、健康检查和回滚证据。

## 明确限制

- 本项目本轮没有启动 API、写入数据库、执行 Docker、推送镜像或操作 NAS。
- 旧 PRD/UIUX 中与现代码冲突的目标已经在各 Spec 的“已知问题备注”和 CHG-2026-008 矩阵中列出；本阶段以当前代码行为作为正式基线，后续修复必须建立新的 Change。
- 编译/结构检查通过不代表行为、浏览器、并发、路径安全、恢复或生产发布通过。
- 业务代码中的差距应使用独立 feature/fix/config Change 实施，不能在文档接入 Change 中悄悄修改。
