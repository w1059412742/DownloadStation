# CHG-2026-002 验证记录（2026-10-02）

## 范围

本记录验证 SDD 文档、项目内技能、REQ/AC 门禁和输入快照。未启动业务 API、未写数据库、未执行 Docker/NAS/镜像推送，因此不宣称产品行为或生产发布完成。

## 技能 TDD（RED → GREEN）

| 阶段 | 实际命令/输入 | 结果 |
| --- | --- | --- |
| RED | `python .agents/skills/sdd-documentation/tests/test_check_sdd.py`，使用旧全局检查器 | 9 个测试中 4 个失败：缺少 REQ→AC、缺少 When、模糊 Then、ready 门禁均未被发现 |
| GREEN | 更新项目本地 `.agents/skills/sdd-documentation/scripts/check_sdd.py`，增加 REQ/AC/GWT/未决措辞/`--ready` 检查，再运行同一 9 个测试 | 9/9 通过 |
| 技能格式 | `PYTHONUTF8=1; quick_validate.py` 对八个项目技能（四个场景技能、两个共享 SDD 技能、TDD 和完成验证） | 八个项目技能均 `Skill is valid!`；旧 PRD/设计技能的下划线命名警告属于既有目录，不是本次新增技能 |
| 项目结构 | `python .agents/skills/sdd-documentation/scripts/check_sdd.py .` | 4 份 Spec、6 份 Change；结构/验收关联问题 0 |
| REQ 覆盖 | 对 4 份 Spec 的 84 个 REQ 与覆盖矩阵逐项比对 | Spec REQ 84、矩阵 REQ 84；缺失 0、未知 0 |
| 文档相对链接 | 仓库内 `docs/**/*.md` 的相对链接解析，并对 URL 编码路径解码 | 检查 72 个链接，缺失 0 个 |
| 后端基线构建 | `dotnet build .\03_src\DownloadStation.sln --nologo` | 成功；0 个警告、0 个错误 |
| 前端基线构建 | `npm run build`（`03_src/DownloadStation.Web`） | 成功；`vue-tsc` 和 Vite 均通过；仅有 Browserslist 数据过期提示 |
| Docker 工具可用性 | `docker --version` | 当前环境未安装 Docker CLI；Docker 构建、启动、健康、恢复和发布仍未验证 |
| 差异格式 | `git diff --check` | 通过；PowerShell 仅输出行尾换行转换警告 |
| 输入快照 | `input-snapshot.json` 与 `final-snapshot.json` | 157 个输入基线、最终 189 个输入；新增 35 个项目内技能/Change/证据文件，13 个既有文档变更，未授权业务源修改 0；3 个前次附件按日期重命名并同步链接 |

## 检查结果解释

- 四份 Spec 共 84 条 REQ、85 个 AC（CAT 24/25、DST 18/18、ADM 26/26、OPS 16/16）；每条 REQ 至少被一个 AC 显式引用；每个 AC 有 Given/When/Then。
- 这证明文档具备机器可检查的追溯结构，不证明四份 Spec 已获负责人批准，也不证明代码已满足目标。
- `--ready` 对当前四份 `draft` Spec 必须失败，这是正确门禁；只有真实确认依据和 approved 状态同时存在时才允许进入实施。
- 原有旧技能目录 `prd`、`design`、`uiux` 已保留作为历史工作资料；根 AGENTS 明确它们不替代当前 docs 和 SDD 技能。

## 未执行和限制

- 未重跑 2026-10-02 之前记录的后端/前端构建；本 Change 只修改文档、技能和检查工具。
- 未修复业务代码中已列出的父软件公开限制、扫描一键绑定、截图、看板、路径安全、哈希恢复、发布脚本等差距；这些必须在 Spec 确认后拆成独立 feature/fix/config Change。
- 未把用户“完整补齐”解释成隐式批准单平台、正文格式、认证和 Docker 生产参数；候选基线已给出稳定建议，真实批准仍由 Spec 状态控制。
- 未执行真实 Docker、群晖、备份、恢复、镜像推送或生产部署。

- 最终源快照：`final-snapshot.json`；未修改业务源代码、数据库迁移、Dockerfile、compose、发布脚本。
