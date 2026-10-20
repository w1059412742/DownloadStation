# CHG-2026-008 DownloadStation 代码现状确认基线

摘要：将 CHG-2026-001 中列出的 53 个未决调查项逐项回到当前代码核对，并把代码已经表达的行为写成可追溯的现状基线。负责人明确接受单平台、无商业广告的归档站定位和 Docker 单容器方向；其余事项按代码确认、代码确认但为缺陷/差距、用户确认、代码无法确认四类处理。
创建日期：2026-10-03
类型：docs
关联规格：[规格索引](../../specs/index.md)
涉及组件：`03_src` 后端、前端、模型、迁移、Docker 输入与发布脚本；`docs/project.md`；四份 Spec
状态：verified

## 目标

- 把上一轮 53 个调查项逐项连接到当前实现证据。
- 解决“代码已经决定但文档仍写成待确认”的表达问题。
- 保留代码与目标 REQ/AC 的差异，不把缺陷批准成正常行为。
- 明确哪些事项必须由 Docker/NAS/生产或负责人运营政策决定，避免继续让 agent 猜测。

## 依据和范围

本次依据负责人于 2026-10-03 的明确要求，以工作区当前代码为现状确认依据。静态证据包含控制器、服务、DTO、模型、EF 配置、前端消费者、Dockerfile、compose、发布脚本和已有审查记录。没有启动 API、浏览器、Docker 或 NAS，因此本 Change 不宣称运行、发布或恢复已经通过。

上一轮审查矩阵中的调查项全部覆盖：C01–C19、A01–A14、D01–D10、U01–U04、O01–O06。详细证据见 [代码确认矩阵](evidence/code-confirmation-matrix.md)。

## 任务与结果

- [x] T1：固定负责人“以当前代码实现作为确认依据”的判定规则。
- [x] T2：逐项核对 53 个调查项的控制器、服务、模型、消费者和 Docker 输入。
- [x] T3：记录代码确认项、代码缺陷/差距项和代码无法确认项，保留证据入口。
- [x] T4：将单平台、无商业广告归档站和单容器方向写入项目基线；删除“高速”无指标承诺。
- [x] T5：更新 project、Spec 导航和 Change 索引，使 agent 能从同一证据文件读取确认结果。
- [x] T6：执行 SDD 结构、链接、格式和本次文档差异检查。

## 结果边界

本 Change 只维护文档和证据，不修改 `03_src` 业务代码、数据库、Docker 配置、发布脚本或运行数据。代码与目标不一致的项仍由 CHG-2026-003 至 CHG-2026-006 或后续 bug/config Change 承接。

Spec 的 `draft` 状态继续表示目标行为尚未全部完成批准和实现验收；本 Change 的 `verified` 只表示代码现状确认文档和检查完成。代码确认不是产品验收，也不是 Docker/NAS 发布证明。

## 代码快照

分支：`master_261002_测试`；HEAD：`57e28479bda434f1e9f507872ff16c53fcfb1830`。本 Change 未修改 `03_src` 业务代码、数据库、Dockerfile、compose 或发布脚本；工作区包含此前 SDD 文档/技能改动和本 Change 文档。

## 规格基线

四份 Spec 仍为 `draft`，但增加了“代码现状确认：已确认（2026-10-03）”和 CHG-2026-008 矩阵引用。`draft` 继续表示目标行为仍有未实现差距，不表示代码调查未完成。

## 环境

Windows、PowerShell、Python 3.13；执行了项目 SDD 检查、Markdown 相对链接检查和 `git diff --check`。当前环境没有 Docker CLI，因此未进行 Docker/NAS/浏览器/API 运行验证。

## 实际验证

| 检查 | 实际结果 | 说明 |
| --- | --- | --- |
| 53 项覆盖 | 通过 | 矩阵覆盖 C01–C19、A01–A14、D01–D10、U01–U04、O01–O06，共 53 项 |
| Spec/Change 结构 | 通过 | `python .agents/skills/sdd-documentation/scripts/check_sdd.py .`：4 份 Spec、8 份 Change、结构问题 0 |
| 检查器回归 | 通过 | `python .agents/skills/sdd-documentation/tests/test_check_sdd.py`：9/9 通过 |
| Markdown 相对链接 | 通过 | `docs/**/*.md` 相对链接检查：新增/受影响文档无缺失目标 |
| Git 差异格式 | 通过 | `git diff --check` 通过 |
| 详细验证记录 | [validation.md](evidence/validation.md) | 含 53 项计数、Parser 现状和未执行边界 |
| API/浏览器/Docker/NAS | 未执行 | 代码确认 Change 不把静态证据写成运行或发布通过 |

## 一致性结论

代码现状、用户明确决定、目标 REQ/AC 和运行环境证据已经分层记录。单平台、无商业广告归档站和单容器方向已从“待确认建议”提升为用户/代码确认基线；公开父状态、路径安全、哈希恢复、资源清理、看板、截图、脚本和 Docker 运行等差异没有被改写成已完成。四份 Spec 的目标状态继续由后续实施和验收 Change 决定。

## 未执行项与限制

本 Change 没有实现产品功能，也没有执行 API、浏览器、Docker、NAS、镜像推送、备份恢复或生产部署。正式卷路径、权限、TLS、RPO/RTO、负载指标、运维保留策略和发布结果必须在对应环境中单独确认。
