using System.Collections.Generic;

namespace DownloadStation.Server.Dtos.Responses
{
    /// <summary>
    /// 标准分页数据封装容器。
    /// </summary>
    /// <typeparam name="T">列表内单项元素类型。</typeparam>
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
