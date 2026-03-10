namespace DownloadStation.Server.Models.Enums
{
    /// <summary>
    /// 软件发布状态枚举。
    /// </summary>
    public enum SoftwareStatus
    {
        /// <summary>
        /// 下架（草稿状态），前台不可见。
        /// </summary>
        Draft = 0,

        /// <summary>
        /// 上架状态，前台可见。
        /// </summary>
        Published = 1
    }
}
