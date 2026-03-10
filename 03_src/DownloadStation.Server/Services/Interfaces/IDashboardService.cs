using System.Threading.Tasks;

namespace DownloadStation.Server.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<object> GetStatisticsAsync();
    }
}
