# 项目规格导航

执行规则见[根AGENTS.md](../../AGENTS.md)。现行八份主规格采用中文领域编号；旧编号经[编号映射](../changes/CHG-2026-003-spec-recheck/change.md#编号迁移映射)定位同一能力、REQ和AC，不另维护第二份目标。

维护目标的D1/D2依据及后续具体决定见对应主规格；`approved`仅表示目标确认。下表实测指接入记录所载2026-10-20历史观察；新增的规格差距有单独来源，不代表重新运行产品。生产环境、组件版本及启用范围缺少可靠记录，统一待核对。

| 编号 | 能力摘要与主规格 | 涉及组件 | 目标确认 | 当前目标落实情况 | 生产版本 | 依据 |
| --- | --- | --- | --- | --- | --- | --- |
| F-访问控制-001 | [认证与公开边界](access/F-访问控制-001-access.md) | SPA、全部API | approved，D1/D2 | 登录与部分拒绝实测；Draft关联及匿名seed偏差未修 | 待核对 | [E01/E05-E10/E12、R01-R06/R20-R22、B01/B02](../changes/CHG-2026-001-sdd-adoption/change.md) |
| F-软件浏览-001 | [访客检索、详情与分享](catalog/F-软件浏览-001-browse.md) | 访客SPA、查询API | approved，D1及失败摘要显示决定 | 组合过滤/缺失详情实测；首页排序切换及失败文本误作SHA256展示待修 | 待核对 | [E02-E08、R09/R10、B07-B09](../changes/CHG-2026-001-sdd-adoption/change.md)、[失败摘要显示差距](../changes/CHG-2026-004-document-consistency/change.md#哈希失败的显示决定与实现差距) |
| F-基础字典-001 | [分类、平台与标签](metadata/F-基础字典-001-metadata.md) | 字典API、编辑与筛选页面 | approved，D1 | 创建及关联实测；删除/查重仅静态 | 待核对 | [E11-E14、R07/R09、B07](../changes/CHG-2026-001-sdd-adoption/change.md) |
| F-软件档案-001 | [软件档案与内容](software/F-软件档案-001-software.md) | 软件编辑、富文本、图片 | approved，D1 | 单平台/标签创建实测；后台共享排序引用已复查，未新增HTTP排序实测 | 待核对 | [E15/E16/E21、R08/R20、B06/B07、BR-002](../changes/CHG-2026-001-sdd-adoption/change.md) |
| F-版本管理-001 | [版本、默认与下载](versions/F-版本管理-001-versions.md) | 版本API、详情、文件流 | approved，D1 | 绑定默认生命周期、拒绝及Range实测；上传/并发未实测 | 待核对 | [E18-E21、R11-R19/R26/R29/R30、B04/B07](../changes/CHG-2026-001-sdd-adoption/change.md) |
| F-软件库-001 | [软件库扫描与边界](library/F-软件库-001-library.md) | 文件管理、绑定、下载 | approved，D1/D2 | 库外scan/bind偏差实测未修；库内扫描静态 | 待核对 | [E18/E22、R23-R26、B03/B04/B09](../changes/CHG-2026-001-sdd-adoption/change.md) |
| F-文件校验-001 | [异步SHA256](hash/F-文件校验-001-hash.md) | 后台服务、版本数据 | approved，D1 | 正常摘要及缺文件Failed实测；恢复等排除 | 待核对 | [E23、R27/R28、B05](../changes/CHG-2026-001-sdd-adoption/change.md) |
| F-后台概览-001 | [后台概览](dashboard/F-后台概览-001-dashboard.md) | 统计API、看板 | approved，D1 | API可调用实测；聚合逐值未实测，版本总数页面展示仍待办 | 待核对 | [E25、R31、B04/B09、BR-001](../changes/CHG-2026-001-sdd-adoption/change.md) |

完整追溯及尚未接入范围见[接入记录](../changes/CHG-2026-001-sdd-adoption/change.md)，后续更正见[变更索引](../changes/index.md)。实现差距及浏览器、并发、恢复、NAS等验证限制仍有效；本表不证明产品验收或生产发布。
