# 项目规格导航

本批维护目标保持接入v3八份主规格不变，沿用2026-10-20用户评论`01a159cc-a87d-7c44-b4d2-9c789c2639ac`的D1/D2依据。`approved`仅表示目标已确认。第1批第2轮独立基线审查的调查可靠性及正式接入条件通过，BR-001、BR-002已复查关闭，累计正式审查2轮；首轮“有保留”及原S2等级保留。2026-10-21接入v4仅同步结论、问题状态和导航，接入检查不代表实现符合目标、产品验收或发布。

| 编号 | 能力摘要与主规格 | 涉及组件 | 目标确认 | 当前目标落实情况 | 生产版本 | 依据 |
| --- | --- | --- | --- | --- | --- | --- |
| F-ACC-001 | [认证与公开边界](access/F-ACC-001-access.md) | SPA、全部API | approved，D1/D2 | 登录与部分拒绝实测；Draft关联及匿名seed偏差未修 | 待核对 | [E01/E05-E10/E12、R01-R06/R20-R22、B01/B02](../changes/CHG-2026-001-sdd-adoption/change.md) |
| F-CAT-001 | [访客检索、详情与分享](catalog/F-CAT-001-browse.md) | 访客SPA、查询API | approved，D1 | 组合过滤/缺失详情实测；排序语义文档已复查；首页排序切换仍待办 | 待核对 | [E02-E08、R09/R10、B07-B09、BR-001/002](../changes/CHG-2026-001-sdd-adoption/change.md) |
| F-MET-001 | [分类、平台与标签](metadata/F-MET-001-metadata.md) | 字典API、编辑与筛选页面 | approved，D1 | 创建及关联实测；删除/查重仅静态 | 待核对 | [E11-E14、R07/R09、B07](../changes/CHG-2026-001-sdd-adoption/change.md) |
| F-SW-001 | [软件档案与内容](software/F-SW-001-software.md) | 软件编辑、富文本、图片 | approved，D1 | 单平台/标签创建实测；后台共享排序引用已复查，未新增HTTP排序实测 | 待核对 | [E15/E16/E21、R08/R20、B06/B07、BR-002](../changes/CHG-2026-001-sdd-adoption/change.md) |
| F-VER-001 | [版本、默认与下载](versions/F-VER-001-versions.md) | 版本API、详情、文件流 | approved，D1 | 绑定默认生命周期、拒绝及Range实测；上传/并发未实测 | 待核对 | [E18-E21、R11-R19/R26/R29/R30、B04/B07](../changes/CHG-2026-001-sdd-adoption/change.md) |
| F-FIL-001 | [软件库扫描与边界](library/F-FIL-001-library.md) | 文件管理、绑定、下载 | approved，D1/D2 | 库外scan/bind偏差实测未修；库内扫描静态 | 待核对 | [E18/E22、R23-R26、B03/B04/B09](../changes/CHG-2026-001-sdd-adoption/change.md) |
| F-HSH-001 | [异步SHA256](hash/F-HSH-001-hash.md) | 后台服务、版本数据 | approved，D1 | 正常摘要及缺文件Failed实测；恢复等排除 | 待核对 | [E23、R27/R28、B05](../changes/CHG-2026-001-sdd-adoption/change.md) |
| F-DSH-001 | [后台概览](dashboard/F-DSH-001-dashboard.md) | 统计API、看板 | approved，D1 | API可调用实测；聚合逐值未实测，版本总数页面展示仍待办 | 待核对 | [E25、R31、B04/B09、BR-001](../changes/CHG-2026-001-sdd-adoption/change.md) |

完整追溯及尚未接入范围见[接入记录](../changes/CHG-2026-001-sdd-adoption/change.md)，该记录现为声明范围的接入`verified`，本地提交尚待队长安排。代码、部署材料和其他分支不会因本表出现目标或接入状态而自动成为已验证实现；生产版本及启用范围仍缺可靠记录，保持待核对。
