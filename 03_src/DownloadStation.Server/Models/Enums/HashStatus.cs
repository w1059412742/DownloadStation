namespace DownloadStation.Server.Models.Enums
{
    /// <summary>
    /// 文件哈希计算状态枚举。
    /// </summary>
    public enum HashStatus
    {
        /// <summary>
        /// 等待计算中。
        /// </summary>
        Pending = 0,

        /// <summary>
        /// 正在后台处理队列中计算。
        /// </summary>
        Computing = 1,

        /// <summary>
        /// 计算完成，哈希值已更新。
        /// </summary>
        Done = 2,

        /// <summary>
        /// 计算失败。
        /// </summary>
        Failed = 3
    }
}
