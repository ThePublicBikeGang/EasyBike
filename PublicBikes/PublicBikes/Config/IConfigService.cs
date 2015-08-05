using System.Threading.Tasks;

namespace PublicBikes.Config
{
    public interface IConfigService
    {
        Task<ConfigModel> GetConfigAsync();
    }
}
