using DownloadStation.Server.Dtos.Requests;
using DownloadStation.Server.Dtos.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DownloadStation.Server.Services.Interfaces
{
    /// <summary>
    /// 定义标签相关的业务操作规范。
    /// </summary>
    public interface ITagService
    {
        /// <summary>
        /// 获取系统中定义的全部标签。
        /// </summary>
        /// <returns>标签响应列表。</returns>
        Task<List<TagResponse>> GetAllAsync();

        /// <summary>
        /// 创建一个新的标签记录。
        /// </summary>
        /// <param name="request">标签创建请求。</param>
        /// <returns>创建后的标签详情。</returns>
        Task<TagResponse> CreateAsync(TagCreateRequest request);

        /// <summary>
        /// 修改现有的标签元数据信息。
        /// </summary>
        /// <param name="id">标签 ID。</param>
        /// <param name="request">标签更新请求。</param>
        /// <returns>修改后的标签详情，若未找到则返回 null。</returns>
        Task<TagResponse?> UpdateAsync(string id, TagUpdateRequest request);

        /// <summary>
        /// 从系统中彻底移除一个标签。
        /// </summary>
        /// <param name="id">标签 ID。</param>
        /// <returns>操作是否成功（是否存在对应标签并执行了移除）。</returns>
        Task<bool> DeleteAsync(string id);
    }
}
