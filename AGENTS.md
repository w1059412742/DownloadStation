# DownloadStation Agent 工作入口

本文件是项目级常驻规则和导航入口，保持短而稳定。业务目标写在 `docs/specs/`，一次变更及其证据写在 `docs/changes/`，具体工作流、模板、脚本和场景细则写在 `.agents/skills/`。不要把业务需求正文或完整流程复制到本文件。

## 开始任何工作

1. 读取本文件、`README.md`、`docs/project.md`、相关 Spec、当前 Change，并检查工作区、分支、HEAD 和运行环境。
2. 选择一个主场景技能：新能力/新阶段用 `sdd-specification-workflow`；已批准实施用 `sdd-implementation-workflow`；缺陷用 `sdd-bugfix-workflow`；存量接入用 `sdd-reverse-engineering-workflow`。
3. 创建或修改项目文档时，必须读取并执行 `sdd-documentation`；交付前必须读取并执行 `sdd-consistency-review` 和 `verification-before-completion`。
4. 只有 `approved Spec + planned Change` 才能进入代码实施；`draft Spec` 只能整理、审查和等待确认，不能直接实现。
5. 未有实际构建、测试、运行或发布证据，不得声称功能完成、合并完成或 Docker/NAS 已发布；未授权不得操作生产数据、NAS、镜像推送或部署。

## 单一权威来源

| 内容 | 权威位置 |
| --- | --- |
| 项目入口和快速启动 | `README.md` |
| 跨能力事实、组件边界、当前状态 | `docs/project.md` |
| 目标行为、REQ、AC 和业务边界 | `docs/specs/` |
| 单次差异、任务、验证和限制 | `docs/changes/` |
| 当前 HTTP 接口导航 | `docs/contracts/`；Controller、DTO、模型和迁移仍是实现源 |
| 可重复工作流和验证规则 | `.agents/skills/` |

## 场景路由

| 场景 | 主技能 | 最低交付物 |
| --- | --- | --- |
| 新能力或新阶段 | `sdd-specification-workflow` | draft/approved Spec、REQ/AC、决定记录、planned Change |
| 已确认实施 | `sdd-implementation-workflow` | 代码、测试、实现证据、一致性结论 |
| 缺陷修复 | `sdd-bugfix-workflow` | 复现、根因、失败测试、最小修复、回归证据 |
| 存量项目接入 | `sdd-reverse-engineering-workflow` | 能力地图、来源覆盖、证据、draft Spec、输入快照 |

主技能会要求读取共享技能；技能不会因为写入文件就自动执行，当前 agent 必须实际读取并遵守。详细步骤不在本文件重复定义。

## 状态门禁

- Spec：`draft → approved → deprecated`。`approved` 只表示目标已确认，不表示代码完成或已经发布。
- Change：`planned → implementing → verified → merged`。`verified` 只表示该 Change 的实际证据完成；合并和发布需要独立事实。
- 使用项目检查器：`python .agents/skills/sdd-documentation/scripts/check_sdd.py .`；实施前可追加 `--ready --spec F-...`。

## 项目工程原则

代码和文档修改遵循当前项目及上级 `AGENTS.md` 的最小改动、稳定性、可验证性和中文注释要求；发现规则冲突时，按更具体目录中的 `AGENTS.md` 执行。
