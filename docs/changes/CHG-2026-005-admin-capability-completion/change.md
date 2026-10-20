# CHG-2026-005-admin-capability-completion 管理能力实现

摘要：补齐服务端鉴权/初始化边界、分类环、软件唯一性、截图相册、看板空间/Top10、草稿预览和资源清理；不改变已确认的数据模型而新增关系须有 Spec 决定。
创建日期：2026-10-02
类型：feature
关联规格：[F-ADM-001](../../specs/admin/F-ADM-001-admin-management.md)
涉及组件：相应前端、后端、数据、测试和 Docker 输入
状态：planned
交付范围：只承接已写入 Spec 的 REQ/AC；本文件不把计划写成实现证据。
目标与方案确认依据：关联 Spec 已于 2026-10-04 按当前代码实现正式确认并标记为 `approved`。本 Change 保留为后续可选的产品修复、增强或发布加固入口，当前未授权立即实施；执行时必须重新核对代码、REQ/AC 和实际证据。

## 任务

- [ ] T1：实现并验证 REQ-ADM-001-006，记录红/绿测试、消费者和文档同步。
- [ ] T2：实现并验证 REQ-ADM-001-007，记录红/绿测试、消费者和文档同步。
- [ ] T3：实现并验证 REQ-ADM-001-008，记录红/绿测试、消费者和文档同步。
- [ ] T4：实现并验证 REQ-ADM-001-017，记录红/绿测试、消费者和文档同步。
- [ ] T5：实现并验证 REQ-ADM-001-019，记录红/绿测试、消费者和文档同步。
- [ ] T6：实现并验证 REQ-ADM-001-020，记录红/绿测试、消费者和文档同步。
- [ ] T7：实现并验证 REQ-ADM-001-021，记录红/绿测试、消费者和文档同步。
- [ ] T8：实现并验证 REQ-ADM-001-023，记录红/绿测试、消费者和文档同步。
- [ ] T9：实现并验证 REQ-ADM-001-024，记录红/绿测试、消费者和文档同步。
- [ ] T10：实现并验证 REQ-ADM-001-026，记录红/绿测试、消费者和文档同步。

## 覆盖规则

本 Change 的任务清单优先列出当前调查确认的实现差距；关联 Spec 的其余 REQ 仍纳入同一范围，若现状已满足则以测试/静态核对证据关闭，不重复修改代码。完整 REQ 来源、Spec 和 Change 去向见 [覆盖矩阵](../CHG-2026-002-sdd-baseline-completion/evidence/requirements-coverage.md)。

## 实施门禁

- Spec 必须保持 `approved`；如果目标行为发生变化，先通过 specification workflow 更新并重新确认。
- 行为变化必须先写失败测试；每个任务保存实际命令和输出。
- 完成前执行 sdd-consistency-review 和 verification-before-completion。
- 未验证 Docker/NAS/恢复不得标记 verified 或发布。
