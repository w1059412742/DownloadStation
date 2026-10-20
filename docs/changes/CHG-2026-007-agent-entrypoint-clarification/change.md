# CHG-2026-007 Agent 入口职责收敛

摘要：将项目级 `AGENTS.md` 从详细流程清单收敛为常驻入口、权威来源、场景路由和状态门禁；把具体工作步骤、模板、脚本和验证要求明确归入项目内技能，消除入口文件与技能之间的重复。
创建日期：2026-10-03
类型：docs
关联规格：无业务行为变化；关联 [项目说明](../../project.md) 和 [Agent 入口](../../../AGENTS.md)
涉及组件：`AGENTS.md`、`docs/project.md`、`.agents/skills/` 的职责引用
状态：verified
交付范围：仅调整 SDD 执行入口和职责说明，不改变业务目标、REQ/AC、代码、数据库、接口或 Docker 发布行为。
目标与方案确认依据：用户指出 `AGENTS.md` 规则过多且混杂；按 SDD 文档分层原则收敛入口，详细规则继续由技能承载。

## 任务

- [x] T1：核对当前 `AGENTS.md`、项目说明和八个项目内技能的职责重叠。
- [x] T2：将 `AGENTS.md` 收敛为常驻入口、单一权威来源、场景路由和状态门禁。
- [x] T3：在 `docs/project.md` 写明入口、技能和项目文档的职责分层。
- [x] T4：运行项目 SDD 检查、技能格式验证、链接检查和差异检查。

## 代码快照

分支：`master_261002_测试`；HEAD：`57e28479bda434f1e9f507872ff16c53fcfb1830`；工作区仅包含项目文档、项目内技能和计划文件改动，`03_src` 无改动。

## 规格基线

本 Change 不修改四份 Spec；它只调整 Agent 入口和职责说明。业务目标仍以四份 `draft` Spec 为准，产品实现仍由 CHG-2026-003 至 CHG-2026-006 承接。

## 环境

Windows、PowerShell、Python 3.13；项目本地检查器和 `quick_validate.py` 可执行。Docker CLI 不属于本 Change 范围。

## 实际验证

- `python .agents/skills/sdd-documentation/scripts/check_sdd.py .`：4 份 Spec、7 份 Change，结构问题 0 个。
- `python .agents/skills/sdd-documentation/tests/test_check_sdd.py`：9/9 通过。
- `PYTHONUTF8=1; quick_validate.py`：8/8 项目技能通过。
- `docs/**/*.md` 相对链接：75 个，缺失 0 个。
- `git diff --check`：通过；没有业务源、数据库、Dockerfile 或发布脚本改动。

## 一致性结论

`AGENTS.md` 现在只承担项目级常驻规则；场景步骤、文档规则、TDD、实现、缺陷、反向工程、一致性审查和完成验证仍由 `.agents/skills/` 承担。业务目标和当前事实仍由 `docs/` 及其代码权威源承担。本 Change 没有改变任何 Spec 的内容或状态。

## 未执行项与限制

本 Change 不批准四份 draft Spec，不实现 CHG-2026-003 至 CHG-2026-006 的产品目标，也不执行 Docker/NAS 发布；这些范围保持原有状态。
