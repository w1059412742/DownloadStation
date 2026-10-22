# 个人软件下载站

个人软件安装包管理与分发应用，包含访客检索、详情与下载、单管理员后台、软件库扫描和后台 SHA256 计算。前端为 Vue 3，后端为 ASP.NET Core 8，数据存储为 SQLite。

## 开发入口

前端位于 `03_src/DownloadStation.Web`，后端位于 `03_src/DownloadStation.Server`。[开发与验证命令](docs/project.md#开发与验证命令)说明依赖、启动方式及副作用；后端启动会迁移数据库并运行写库任务，先核对隔离配置。[发布与恢复](docs/project.md#发布与恢复)说明现有脚本风险及尚未核实的部署范围。

## 文档导航

- [项目说明](docs/project.md)：组件、公共约束、契约来源和工程命令。
- [根 AGENTS.md](AGENTS.md)：持久执行规则与场景路由。
- [维护规格与落实情况](docs/specs/index.md)：八项能力、目标确认、实现差距和生产版本依据。
- [接入调查与历史证据](docs/changes/CHG-2026-001-sdd-adoption/change.md)：D1/D2、覆盖范围及后续待办。
- [当前复核与编号映射](docs/changes/CHG-2026-003-spec-recheck/change.md)：中文编号迁移、修订依据及检查范围。
- [变更索引](docs/changes/index.md)：完整性补查和当前变更记录。

规格表示已确认目标，现有实现差距及依据见规格索引；目标确认不代表产品验收或生产发布。旧 PRD/UIUX/设计资料保留原位置，适用关系见接入记录。
