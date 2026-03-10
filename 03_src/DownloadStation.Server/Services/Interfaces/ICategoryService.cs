using DownloadStation.Server.Dtos.Requests;
using DownloadStation.Server.Dtos.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DownloadStation.Server.Services.Interfaces
{
    /// <summary>
    /// 分类服务抽象层：提供标准化的多级分类树结构读取以及维护方案。
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// 提取系统中目前活跃的全量分类，并按照 ParentId 以及 SortOrder 自行组装为前端所需的树状数据结构。
        /// </summary>
        Task<List<CategoryTreeResponse>> GetTreeAsync();

        /// <summary>
        /// 录入一个全新的分类记录。
        /// </summary>
        Task<CategoryTreeResponse> CreateAsync(CategoryCreateRequest request);

        /// <summary>
        /// 更新目前存在的某个分类核心资料。
        /// </summary>
        Task<CategoryTreeResponse?> UpdateAsync(string id, CategoryUpdateRequest request);

        /// <summary>
        /// 强力擦除某条分类数据。如有子级应酌情报错或做级联处理。
        /// </summary>
        Task<bool> DeleteAsync(string id);
    }
}
