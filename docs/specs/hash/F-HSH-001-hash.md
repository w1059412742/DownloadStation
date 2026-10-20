# F-HSH-001 安装包异步SHA256

状态：approved
涉及组件：后台HashComputeService、版本数据库及展示
确认依据：2026-10-20 用户评论 `01a159cc-a87d-7c44-b4d2-9c789c2639ac`，采用[接入记录 D1](../../changes/CHG-2026-001-sdd-adoption/change.md#本轮真实目标决定)范围内E23正常计算及失败状态，不采用恢复缺口。

## 目标与范围

在单实例服务持续运行、库内文件在计算期间稳定的条件下，对Pending版本流式计算SHA256。不承诺崩溃后的Computing恢复、Failed重试、多实例任务抢占、停机中断和并行删除一致性；这些保留B05，不作为已批准现状。

## 依赖与适用约束

| 权威来源 | 适用内容及条件 |
| --- | --- |
| [版本](../versions/F-VER-001-versions.md) | 新版本进入Pending |
| [文件库](../library/F-FIL-001-library.md) | 被读取安装包的库内边界 |
| [项目说明](../../project.md) | 状态值；接入记录E23、R27/R28 |

## 功能规则

### REQ-HSH-001-001
服务运行且有Pending版本时，必须逐次选创建时间最早的Pending，先持久化Computing，再读取文件计算SHA256；成功后必须保存完整摘要、Done及更新时间。对相同字节内容必须得到相同SHA256，文件不因计算改变。

### REQ-HSH-001-002
所选版本的文件不存在或本次读取/计算异常时，必须将其记为Failed并继续后续循环；无Pending时必须等待5秒后继续查找，外层循环异常后等待10秒再继续。不把轮询间隔解释为完成时限。

## 状态与契约

正常路径为Pending → Computing → Done，缺文件或计算异常为Computing → Failed。任务由`Program.cs`注册，代码权威源为`BackgroundServices/HashComputeService.cs`，结果由`VersionResponse`提供给前后台详情。展示含义引用访客规格，不是恶意软件检测结果。

## 验收条件

### AC-HSH-001-001（验证 REQ-HSH-001-001）
前提：单实例持续运行，有两个创建时间不同的Pending，文件稳定可读。
操作：运行计算任务并读取状态与摘要。
结果：选择次序按最早创建，持久化Computing后得到Done，摘要等于独立SHA256计算，文件字节不变。

### AC-HSH-001-002（验证 REQ-HSH-001-002）
前提：Pending指向缺失文件或读取故障文件，其后有可读任务。
操作：运行循环，另观察空队列及外层异常分支。
结果：失败任务为Failed，后续任务仍被处理；空队列及外层异常分别按5秒和10秒间隔再查，不承诺本次范围外自动重试Failed。
