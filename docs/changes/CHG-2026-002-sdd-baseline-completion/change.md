# CHG-2026-002 DownloadStation SDD 基线补齐与执行机制落地

摘要：承接 CHG-2026-001 的复核缺口，完整整理原 PRD/UIUX、代码新增能力和跨端/文件/Docker 边界，重写可判定的四份规格，落地项目内场景技能、共享规则与检查工具。该工作落实 SDD 体系；产品功能的当前差距独立记录，不用修改目标来掩盖未实现项。
创建日期：2026-10-02
类型：docs
关联规格：[规格索引](../../specs/index.md)
涉及组件：README、AGENTS、docs、.agents/skills、.planning/sdd-adoption
状态：verified
交付范围：全仓库文档与 SDD 执行机制；基线覆盖全部来源条款，当前业务实现以证据核对。真实生产 Docker/NAS 发布不属于文档接入操作。
目标与方案确认依据：用户要求“完全补齐，完整实现，正规，正式，实现，不要再有遗漏”；已确认采用既定 SDD 思想和 Docker 发布。尚无对平台关系、正文/会话等具体新取舍的文件差异批准，不虚构该批准。

## 调查范围与源快照

分支：master_261002_测试。HEAD：57e28479bda434f1e9f507872ff16c53fcfb1830。输入包括 140 个受跟踪文件、上一轮未提交文档和审查证据；基线以本次输入快照为准。

## 文档方案与任务

- [x] T1：固定当前输入，逐条处置原 PRD/UIUX，核对全部端、模块、接口、状态与消费者；证据保存到 evidence。
- [x] T2：补齐 F-CAT-001、F-DST-001、F-ADM-001、F-OPS-001 的具体规则、边界和 AC；保留已有编号，不复制权威定义。
- [x] T3：准确补齐 project、当前 HTTP 契约、规格导航、实现差距与集中业务取舍；具体技术规则可判定，建议与批准区分。
- [x] T4：将四个场景技能、两个共享 SDD 技能以及 TDD/完成验证技能放入项目；AGENTS 明确强制路由、共享技能执行、TDD 和完成门禁，旧技能改为历史入口。
- [x] T5：针对原审查暴露的漏检建立检查工具回归用例；实际记录 RED/GREEN、负例与默认/实施门禁差异。
- [x] T6：按文档与现状基线模式完成一致性复核，保存实际检查与源快照复查；具体目标确认后才标 approved。

任务关联：T1–T3 对应四份 Spec 的全部 REQ/AC 与来源覆盖矩阵；T4–T5 是公共执行机制检查；T6 对应本次文档/基线完整性和真实状态。

## 实际验证

审查模式：现状基线 + 文档；不使用文档检查代替产品测试。
代码快照：上述 HEAD 与本次未提交输入哈希。
规格基线：四份目标规格，内容补齐后保存本次差异/哈希，正式批准依据按真实审阅更新。
环境与日期：Windows，本轮当地记录为 2026-10-02（Asia/Shanghai）。

本 Change 的文档与技能维护工作已完成并由下方检查验证。本 Change 不宣称业务功能或 Docker 运行验收完成。

## 交付结果与实际验证

本 Change 的文档和执行机制已经按本仓库范围补齐并验证，状态为 `verified`。这表示本次 SDD 基线维护的文件、编号、链接、REQ/AC 门禁和技能格式检查完成；不表示业务代码、Docker 镜像、生产部署或四份 Spec 已获业务批准。

| 检查 | 实际结果 | 证据 |
| --- | --- | --- |
| 来源覆盖 | 84 条候选 REQ、85 个 AC；PRD/UIUX/代码/Docker 主题逐条进入覆盖矩阵 | [requirements-coverage.md](evidence/requirements-coverage.md) |
| Spec 结构 | 4 份 Spec，全部 draft；无重复编号；每条 REQ 有 AC；AC 有完整 GWT | [check_sdd.py](../../../.agents/skills/sdd-documentation/scripts/check_sdd.py)、[validation.md](evidence/validation.md) |
| Change 结构 | 6 份 Change，摘要、类型、关联规格、状态和索引完整 | `check_sdd.py` 实际输出 |
| 本地技能 | 8 个项目内技能均通过 skill quick validation | [validation.md](evidence/validation.md) |
| 门禁测试 | RED 旧检查器 4 项漏检；GREEN 新检查器 9/9 通过 | [validator-red.json](evidence/validator-red.json)、[validator-green.json](evidence/validator-green.json) |
| 输入基线 | 初始 157 个文件、最终 189 个文件的 SHA-256 已保存；新增和变更范围均在项目文档/技能/证据目录，业务源未改 | [input-snapshot.json](evidence/input-snapshot.json)、[final-snapshot.json](evidence/final-snapshot.json) |
| Git 差异格式 | `git diff --check` 通过 | [validation.md](evidence/validation.md) |

## 一致性结论

| 对象 | 结论 | 状态 |
| --- | --- | --- |
| 文档职责 | README、AGENTS、project、Spec、contract、Change、evidence 各有单一职责，无新增平行需求目录 | 已验证 |
| 来源承接 | 原 PRD/UIUX、当前端/后端/数据/Docker 主题均有 Spec 或明确差距去向 | 已验证为候选基线 |
| REQ/AC | 需求原子性、显式覆盖、GWT 和未决措辞由检查器门禁；仍需人工审阅业务正确性 | 结构已验证 |
| 技能路由 | 四个场景技能、两个共享技能以及 TDD/完成验证技能落到项目内，AGENTS 明确调用关系 | 已验证 |
| 代码一致性 | 现有实现与候选目标仍有公开边界、相册、看板、路径、哈希和 Docker 差距 | 未实现，转后续 Change |
| 产品批准 | 四份 Spec 仍为 draft；没有伪造负责人逐条确认 | 未完成，等待真实确认 |
| 生产发布 | 未执行 Docker/NAS/推送/恢复 | 未验证 |

## 后续独立 Change

1. [CHG-2026-003 公开体验实现](../CHG-2026-003-public-experience-completion/change.md)：公开详情/版本/下载边界、截图相册、正文/日志安全渲染、历史折叠、主题和代码复制。
2. [CHG-2026-004 分发安全与恢复](../CHG-2026-004-distribution-safety-resilience/change.md)：父软件 Published 限制、路径规范化、下载计数原子性、哈希 Computing 恢复、物理删除失败和扫描绑定。
3. [CHG-2026-005 管理能力实现](../CHG-2026-005-admin-capability-completion/change.md)：看板空间/Top10、后台草稿预览、截图相册、服务端唯一性和资源清理。
4. [CHG-2026-006 Docker 发布加固](../CHG-2026-006-docker-release-hardening/change.md)：PowerShell/bash 发布脚本、单容器健康检查、秘密注入、.dockerignore、卷/备份/恢复/回滚和实际 Docker 验收。
5. `approval` 不作为 Change 类型；每项业务决定通过 Spec 的真实确认依据记录，确认后才把相关 Spec 改为 approved。

## 未执行项与限制

本 Change 没有修改业务代码、数据库迁移、测试项目、Dockerfile、compose、发布脚本或生产数据。业务代码的当前缺陷已经全部进入对应 Spec 的差距和后续 Change 分类；此处不把“文档完整”写成“功能已完成”。