# HTTP 契约参考

状态：`derived-reference`（当前接口以控制器、Request/Response DTO、序列化配置为权威；本文不是独立 schema 源）

修改行为时先修改代码权威源，再同步本参考、对应 Spec、消费者和 Change。目标但当前不存在的接口必须标为 planned，不得写成当前可用。

## 通用响应与认证

当前 JSON 成功响应通常为：

```json
{ "code": 200, "message": "操作成功", "data": {} }
```

失败响应的 HTTP 状态、业务 code 和错误文本以控制器实现为准；不能假定所有失败同一外壳。登录错误当前可能是 HTTP 200 + body code 401，下载错误可能是文本 404。公开接口无需 JWT，Admin API 需要 Bearer JWT。

## 当前公开接口

| 方法 | 路径 | 认证 | 当前代码用途 | 目标边界 |
| --- | --- | --- | --- | --- |
| GET | `/api/categories/tree` | 无 | 获取分类树 | CAT 任意深度导航 |
| GET | `/api/platforms` | 无 | 获取平台列表 | CAT 平台过滤 |
| GET | `/api/softwares` | 无 | 分页查询已发布软件；支持 categoryId/platformId/keyword/sortBy/page/pageSize | CAT REQ-001-001 至 009 |
| GET | `/api/softwares/{id}` | 无 | 获取公开详情；草稿当前返回 404 | CAT REQ-001-006、010、011、018、019 |
| GET | `/api/softwares/{softwareId}/versions` | 无 | 查询版本；当前服务只按版本可见过滤 | CAT REQ-001-007、012 至 016；需补父状态和去 FilePath |
| GET | `/api/softwares/{softwareId}/versions/{versionId}/download` | 无 | 校验版本/软件 ID/可见性/物理文件后范围下载 | CAT REQ-001-007、016、DST REQ-001-005/012/013 |
| GET | `/s/{id}` | 无 | 前端分享路由 | CAT REQ-001-017 |

## 当前认证与后台接口

| 方法 | 路径 | 认证 | 当前代码用途 | 目标边界 |
| --- | --- | --- | --- | --- |
| POST | `/api/auth/login` | 无 | 固定配置密码换 JWT | ADM REQ-001-001/002 |
| GET | `/api/admin/dashboard/statistics` | JWT | 基础软件/版本/分类/平台/下载统计 | ADM REQ-001-006、021；当前缺空间/排行 |
| GET/POST/PUT/DELETE | `/api/admin/categories[/id]` | JWT；`seed` 当前匿名特例 | 分类树 CRUD | ADM REQ-001-003/008/009/023 |
| GET/POST/PUT/DELETE | `/api/admin/platforms[/id]` | JWT | 平台 CRUD | ADM REQ-001-004/010/011 |
| GET/POST/PUT/DELETE | `/api/admin/tags[/id]` | JWT | 标签 CRUD | ADM REQ-001-012/013/014 |
| GET | `/api/admin/softwares` | JWT | 含草稿软件分页查询 | ADM REQ-001-015 |
| GET/POST/PUT/DELETE | `/api/admin/softwares[/id]` | JWT | 软件档案 CRUD | ADM REQ-001-005/016/017/018/019 |
| PATCH | `/api/admin/softwares/{id}/status` | JWT | 修改发布状态 | ADM REQ-001-018 |
| GET | `/api/admin/softwares/check-name` | JWT | 前端同平台重名预检查 | ADM REQ-001-017；不能替代服务端约束 |
| POST | `/api/admin/softwares/upload-image` | JWT | 图片上传，控制器当前限制 10 MB | ADM REQ-001-007/026 |
| GET | `/api/admin/versions/software/{softwareId}` | JWT | 查询软件全部版本，包含隐藏项 | DST REQ-001-017 |
| POST | `/api/admin/versions/upload` | JWT | multipart 上传并创建版本 | DST REQ-001-010/011/014 |
| PUT | `/api/admin/versions/{id}` | JWT | 修改版本号和日志 | DST REQ-001-009 |
| DELETE | `/api/admin/versions/{id}?physicalDelete=false` | JWT | 删除记录，可选尝试删除物理文件 | DST REQ-001-008 |
| PATCH | `/api/admin/versions/{id}/visibility` | JWT | 修改可见性 | DST REQ-001-007 |
| PATCH | `/api/admin/versions/{id}/default` | JWT | 设置默认版本 | DST REQ-001-006/007 |
| GET | `/api/admin/files/scan?path=...` | JWT | 扫描未绑定物理文件 | DST REQ-001-002/003/013 |
| POST | `/api/admin/files/bind` | JWT | 以路径、软件、版本号和日志绑定版本 | DST REQ-001-001/003/009/013/016 |

## 当前不存在或需新建的目标接口

| 目标能力 | 归属 Spec | 当前状态 | 建议 Change |
| --- | --- | --- | --- |
| 截图相册添加/排序/移除 | CAT/ADM | 模型和读取局部存在，未找到完整写入接口 | feature |
| 看板空间与 Top10 | ADM | 当前基础统计没有该字段/排行 | feature |
| 哈希失败重算 | DST | 当前没有明确管理入口 | feature |
| 健康检查、备份、恢复探测 | OPS | 当前无完整接口/脚本证据 | config |

## 契约风险和固定边界

- 公开版本列表和下载必须共同检查父软件 Published；列表不能返回 FilePath。
- `FileBindRequest.FilePath` 和上传文件名必须在配置存储根内规范化，拒绝目录穿越和链接逃逸。
- 下载计数目标是有效响应请求计数，必须用可验证的原子更新；客户端取消不回滚。
- 认证、公开下载、后台写入和错误状态码的实际结果必须在 HTTP/浏览器测试中分别记录。
- 接口变化先更新 Spec/Change 目标，再改代码和消费者；不要直接手改本参考制造契约。
