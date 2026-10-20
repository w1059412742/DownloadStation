# 规格索引

四份 Spec 已于 2026-10-04 按用户明确要求，以当前代码实现作为本阶段正式基线，状态均为 `approved`。代码现状证据见 [CHG-2026-008 代码确认矩阵](../changes/CHG-2026-008-code-confirmed-baseline/evidence/code-confirmation-matrix.md)，批准记录见 [CHG-2026-009 当前基线正式确认](../changes/CHG-2026-009-current-baseline-approval/change.md)。已知问题集中记录在各 Spec 末尾，后续修复通过新的 Change 处理。

| Spec | 能力 | REQ 数 | AC 数 | 状态 |
| --- | --- | ---: | ---: | --- |
| [F-CAT-001](catalog/F-CAT-001-public-catalog.md) | 公开目录、详情、版本展示和匿名访问 | 24 | 25 | approved |
| [F-DST-001](distribution/F-DST-001-version-distribution.md) | 版本、扫描、绑定、哈希、下载和恢复 | 18 | 18 | approved |
| [F-ADM-001](admin/F-ADM-001-admin-management.md) | 认证、字典、软件、截图、看板和后台 | 26 | 26 | approved |
| [F-OPS-001](operations/F-OPS-001-docker-deployment.md) | Docker 单容器构建、持久化、恢复和发布 | 16 | 16 | approved |
| **合计** | **跨端 SDD 正式基线** | **84** | **85** | **approved** |

## 责任边界

- F-CAT-001 负责访客可观察的公开行为和前端公开交互。
- F-DST-001 负责版本/物理文件生命周期、哈希、下载和路径/计数边界。
- F-ADM-001 负责管理员身份、后台写入、字典/软件/截图和统计。
- F-OPS-001 负责构建、运行、卷、秘密、迁移、恢复、回滚和发布证据。
- 共享事实只在一个权威 Spec 或 project 中定义，其他文件引用。

## 生命周期

当前基线已获确认并为 `approved`。后续行为变化、Bug 修复和发布加固仍必须建立 Change；`approved` 不表示代码已经修复所有已知问题，也不表示 Docker/NAS 已发布。
