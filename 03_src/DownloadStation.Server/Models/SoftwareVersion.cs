using System;
using DownloadStation.Server.Models.Enums;

namespace DownloadStation.Server.Models
{
    /// <summary>
    /// 重中之重的业务单元 - 版本分发记录。它挂载实际在 NAS 磁盘系统上发现的大型安装包，并记录相关的统计更新。
    /// </summary>
    public class SoftwareVersion
    {
        /// <summary>
        /// 每种单机发布版的独立追踪主键。
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString("N").Substring(0, 16);

        /// <summary>
        /// 关联的外围软件描述单元。
        /// </summary>
        public string SoftwareId { get; set; } = string.Empty;

        /// <summary>
        /// 比如 v2023.1, build1884。前端依照这个字符串对外明示迭代名。
        /// </summary>
        public string VersionNumber { get; set; } = string.Empty;

        /// <summary>
        /// 承载基于 Markdown 规范格式书写的当前版本独有修改和更新概览 (Changelog)。可以为空，则不显示详细抽屉。
        /// </summary>
        public string? Changelog { get; set; }

        /// <summary>
        /// 读取扫描出的物理文件名，如 setup.exe。
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// 直接定位到 NAS 的专用 SMB 磁盘映射或应用挂载目录对应的那个长相对路径。
        /// </summary>
        public string FilePath { get; set; } = string.Empty;

        /// <summary>
        /// 孤儿文件接管入体系时直接提取占用的精确字节，提供页面空间换算需求。
        /// </summary>
        public long FileSize { get; set; } = 0;

        /// <summary>
        /// 在入库时，后台会挂接一个异步工作流专门扫文件生成的唯一指纹防止篡改。此时它可能尚处于计算过程中。
        /// </summary>
        public string? HashSHA256 { get; set; }

        /// <summary>
        /// 指挥中心通过它了解文件是被成功处理还是仍在队列。用于更新页面的“校验码查询中”这类的动态提示。
        /// </summary>
        public HashStatus HashStatus { get; set; } = HashStatus.Pending;

        /// <summary>
        /// 反向记录这个版本的受欢迎程度，为站长分析提供宏观基底。每次下载 +1。
        /// </summary>
        public int DownloadCount { get; set; } = 0;

        /// <summary>
        /// 使用数值进行软下架处理控制，当存在严重 Bug 需要紧急召回或者不再推荐时置为 0。使用 int 配合部分 ORM 支持的兼容特性（这里0隐藏，1可见）。
        /// </summary>
        public int IsVisible { get; set; } = 1;

        /// <summary>
        /// 是否作为详情页主下载按钮默认指向的版本。每个软件最多一个默认版本。
        /// </summary>
        public int IsDefault { get; set; } = 0;

        /// <summary>
        /// 往往作为最新的发布时序基准向。
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 内容更正（日志拼写修改或者校验失败重扫）所留下的更新戳。
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // 导航属性
        
        /// <summary>
        /// 溯源：指向它实际挂载的母版软件实例记录。
        /// </summary>
        public Software? Software { get; set; }
    }
}
