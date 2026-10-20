# DownloadStation

个人软件安装包管理与分发平台。项目面向群晖 NAS，提供公开软件下载页面和受保护的管理后台；软件包、SQLite 数据库、上传图片和日志通过 Docker 持久化。

当前仓库已建立完整的存量项目 SDD 正式基线和项目内执行技能。`docs/specs/` 中的规格文件覆盖旧 PRD、UIUX、当前代码、消费者和 Docker 输入；2026-10-03 已完成 53 个调查项的代码现状确认（见 [代码确认矩阵](docs/changes/CHG-2026-008-code-confirmed-baseline/evidence/code-confirmation-matrix.md)）。四份 Spec 已按 2026-10-04 的用户确认，以当前代码实现作为本阶段正式基线并标记为 `approved`。各 Spec 末尾记录已知问题；后续行为变化、Bug 修复和 Docker/NAS 发布仍需独立 Change 与实际证据。

## 从这里开始

- [项目说明](docs/project.md)：系统边界、组件、命令、Docker 持久化和当前限制。
- [规格索引](docs/specs/index.md)：公开目录、版本分发、后台管理、Docker 运维四个能力范围。
- [HTTP 契约参考](docs/contracts/http-api.md)：前后端当前共享的接口入口和状态；控制器与 DTO 是契约源。
- [变更索引](docs/changes/index.md)：SDD 接入记录、正式基线、证据、已知问题和后续待办。
- [Agent 工作规则](AGENTS.md)：文档驱动开发时的读取、修改、验证和交接规则。
- [完整来源覆盖与验收映射](docs/changes/CHG-2026-002-sdd-baseline-completion/evidence/requirements-coverage.md)：84 条候选 REQ、85 个 AC 与原资料/代码主题的逐条去向。
- `.agents/skills/sdd-*`：项目内可重复执行的规格、实施、缺陷、反向整理、文档维护和一致性审查技能。

## 目录

```text
01_需求/                         旧 PRD 原件和文本导出
02_设计/                         旧 UIUX 原件和文本导出
03_src/                          前后端源代码、迁移、Docker 文件和发布脚本
.agents/skills/                  项目已有的 PRD、设计和 UIUX 技能
.planning/sdd-adoption/          本次存量接入的调查计划和工作记录
docs/                            SDD 项目文档入口
```

旧资料保留在原位置以便追溯，不与 `docs/specs/` 并行作为第二套正式规格。新需求、缺陷和实现变更从 `docs/changes/` 开始，并在确认后更新相应 Spec。

## 常用命令

后端构建：

```powershell
dotnet build .\03_src\DownloadStation.sln --nologo
```

前端构建：

```powershell
Set-Location .\03_src\DownloadStation.Web
npm install
npm run build
```

群晖单容器镜像构建（从 `03_src` 执行）：

```powershell
Set-Location .\03_src
docker build -f Dockerfile.Synology -t <registry>/<image>:<version> .
```

`03_src/publish.ps1` 和 `03_src/publish.sh` 会构建 `Dockerfile.Synology`、打 `latest` 标签并推送；执行发布前必须确认镜像仓库、版本号、Docker 登录状态和宿主机卷路径。

## SDD 使用方式

每次任务先读取 `AGENTS.md` 和对应场景技能，由 agent 讨论后维护 Spec/Change/证据。新能力先走规格流程，实施只接受 approved Spec，缺陷先复现并建立失败测试，存量范围先做代码/资料/Git/消费者反向调查。完成前运行：

```powershell
python .\.agents\skills\sdd-documentation\scripts\check_sdd.py .
python .\.agents\skills\sdd-documentation\tests\test_check_sdd.py
```

两个命令只验证项目文档门禁；业务行为、浏览器、Docker、NAS、备份和发布仍需按对应 Change 保存实际证据。
