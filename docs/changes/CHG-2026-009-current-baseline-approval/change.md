# CHG-2026-009 DownloadStation 当前代码基线正式确认

摘要：根据负责人于 2026-10-04 的明确要求，将四份 Spec 中未单独作出业务决定的实现行为统一按当前代码实现作为本阶段正式基线。四份 Spec 标记为 `approved`；当前已知问题保留为后续修复备注，本 Change 不修改产品代码。
创建日期：2026-10-04
类型：docs
关联规格：[F-CAT-001](../../specs/catalog/F-CAT-001-public-catalog.md)、[F-DST-001](../../specs/distribution/F-DST-001-version-distribution.md)、[F-ADM-001](../../specs/admin/F-ADM-001-admin-management.md)、[F-OPS-001](../../specs/operations/F-OPS-001-docker-deployment.md)
涉及组件：四份 Spec、`docs/project.md`、`README.md`、规格索引、Change 索引、CHG-2026-003 至 CHG-2026-006 的入口说明
状态：verified
交付范围：正式确认当前实现基线并维护可追溯文档；不修复业务代码、不修改数据库、不构建镜像、不执行 Docker/NAS 发布。
目标与方案确认依据：负责人于 2026-10-04 明确要求：此前没有单独决定的项目行为，当前统一以代码已经实现的行为为标准；后续发生行为变化或 Bug 时，再建立新的 Change 修订 Spec、契约、消费者和证据。

## 目标与目标差异

- 将 F-CAT-001、F-DST-001、F-ADM-001、F-OPS-001 从 `draft` 改为 `approved`，表示当前实现行为已被接受为本阶段正式基线。
- 保留各 Spec 中的 REQ/AC、代码现状依据和已知问题备注，避免把已发现的限制误写成已经修复。
- 将“已批准当前基线”“产品 Bug 修复”“目标行为变化”“Docker/NAS 发布”分开记录；本 Change 只完成第一项。
- `approved` 不代表所有已知问题已经修复，不代表所有产品目标已经重新实现，也不代表 Docker/NAS 已发布。

## 现状调查

CHG-2026-008 已逐项核对上一轮 53 个调查项：C01–C19、A01–A14、D01–D10、U01–U04、O01–O06。核对范围包括控制器、服务、DTO、模型、EF 配置、迁移、前端消费者、Dockerfile、compose、发布脚本和已有审查记录，详细证据见 [代码确认矩阵](../CHG-2026-008-code-confirmed-baseline/evidence/code-confirmation-matrix.md)。

本次确认沿用该静态证据，并将各 Spec 末尾的已知问题作为当前实现例外记录。未把静态调查扩写为 API、浏览器、Docker、NAS、备份恢复或生产发布证据。

## 方案与修改范围

- 更新四份 Spec 的状态、确认依据、正式现状基线说明、已知问题备注和交接条件。
- 更新 `docs/specs/index.md`、`docs/project.md`、`README.md`，使入口和导航反映 `approved` 基线。
- 新增本 Change 及验证证据，并将 CHG-2026-003 至 CHG-2026-006 改为面向已批准基线的后续可选实施入口。
- 不修改 `03_src`、数据库、Dockerfile、compose、发布脚本、测试代码或运行数据。

## 任务

- [x] T1：确认并固定“当前代码实现作为本阶段正式标准”的用户决定。
- [x] T2：将四份 Spec 的状态和确认依据更新为正式基线，并保留已知问题备注。
- [x] T3：同步项目入口、索引和后续 Change 的状态说明。
- [x] T4：执行 SDD 结构、实施门禁、检查器回归、链接和差异验证。

## 未决事项与风险

- 已知问题仍然存在于当前代码基线中，后续修复必须建立新的 Bug/feature/config Change；修复后同步 REQ/AC、契约、消费者和验证证据。
- 当前没有 Docker CLI、API、浏览器或 NAS 运行证据，因此 F-OPS-001 的 `approved` 只表示当前 Docker 输入和发布脚本行为已被接受为文档基线，不表示部署成功。
- 旧 Change 中的历史 `draft` 叙述保留为历史事实；本 Change 是后续正式确认记录，不改写旧证据。

## 实施结果与计划偏差

四份 Spec 已全部标记为 `approved`，并统一注明“代码优先基线”。已知问题集中记录在每份 Spec 末尾，CHG-2026-003 至 CHG-2026-006 已改为后续可选入口。本 Change 未产生业务代码或部署变更，实际范围与计划一致。

## 实际验证

审查模式：文档维护与代码现状基线确认
代码快照：分支 `master_261002_测试`；HEAD `57e28479bda434f1e9f507872ff16c53fcfb1830`；本 Change 预期仅有文档、技能和计划文件差异，`03_src` 无差异。
规格基线：四份 Spec 的当前工作区版本；确认依据为 2026-10-04 用户明确决定，代码证据引用 CHG-2026-008。
环境与日期：Windows、PowerShell、Python 3.13.2；2026-10-04。

| 检查或范围 | 命令/人工步骤 | 实际结果 | 证据 |
| --- | --- | --- | --- |
| SDD 结构和引用 | `python .agents/skills/sdd-documentation/scripts/check_sdd.py .` | 通过；4 份 Spec、9 份 Change，结构问题 0 个 | [validation.md](evidence/validation.md) |
| approved 实施门禁 | `python .agents/skills/sdd-documentation/scripts/check_sdd.py . --ready` | 通过；4 份 Spec 均为 approved，结构问题 0 个 | [validation.md](evidence/validation.md) |
| 检查器回归 | `python .agents/skills/sdd-documentation/tests/test_check_sdd.py` | 通过；9/9 测试通过 | [validation.md](evidence/validation.md) |
| Markdown 相对链接 | `docs/**/*.md` 相对链接扫描 | 通过；100 个相对链接，缺失 0 个 | [validation.md](evidence/validation.md) |
| 差异格式 | `git diff --check` | 通过 | [validation.md](evidence/validation.md) |
| 业务代码范围 | `git diff --name-only -- 03_src` | 通过；无输出，03_src 无差异 | [validation.md](evidence/validation.md) |

### 一致性结论

当前四份 Spec 的状态、确认依据、代码现状矩阵引用、已知问题备注、项目入口和索引已经按同一决定同步。文档基线与代码现状的关系被明确为“当前实现正式标准”；任何后续行为改变都必须通过新 Change 处理。本 Change 不把文档确认扩大为产品功能验收或 Docker/NAS 发布。

| 对象/编号 | 目标或调查结论 | 实际证据 | 差异分类 | 处理决定 | 所需复查/重测 | 状态 |
| --- | --- | --- | --- | --- | --- | --- |
| F-CAT-001、F-DST-001、F-ADM-001、F-OPS-001 | 当前实现行为作为本阶段正式基线 | 四份 Spec 的代码优先确认依据；CHG-2026-008 矩阵 | 已授权的基线确认 | 状态统一为 `approved`，已知问题保留在 Spec 末尾 | 后续 Bug 或行为变更时执行对应 Change | 已完成 |
| 各 Spec 已知问题表 | 代码中已存在的限制和缺陷继续可追溯 | 各 Spec“已知问题备注”、CHG-2026-008 静态证据 | 实现缺陷 | 不在本 Change 修复，不改写成已完成 | 修复时补失败测试、回归和一致性审查 | 已记录 |
| F-OPS-001 运行/发布证据 | Docker/NAS 行为需要现场验证 | 本次没有 Docker CLI、API、浏览器或 NAS 执行结果 | 测试或证据缺口 | 保持为已知限制，不声称发布成功 | CHG-2026-006 中执行构建、启动、恢复和发布验收 | 未执行 |
| CHG-2026-003 至 CHG-2026-006 | 后续产品修复、增强和发布加固入口 | 各 Change 的 `planned` 状态和当前 `approved` Spec 引用 | 仅计划或入口差异 | 更新门禁说明，保持未实施 | 实施时重新核对代码、REQ/AC 和证据 | 已同步 |
| `03_src` | 本次只维护文档 | `git diff --name-only -- 03_src` 无输出 | 无差异 | 不修改业务代码、数据库和部署输入 | 产品实施时按对应 Change 验证 | 已核对 |

### 未执行项与限制

本 Change 未执行 API、浏览器、Docker 构建/启动、镜像推送、NAS 部署、备份恢复、回滚或生产验收；这些项目仍需在对应实施和发布 Change 中用实际证据确认。

## 发布事项

不适用。本 Change 只更新项目文档和 SDD 基线状态，没有发布、迁移、配置切换或外部消费者变更。
