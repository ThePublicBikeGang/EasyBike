using System.Threading.Tasks;

namespace PublicBikes.Config
{
    public interface IConfigService
    {
        Task<ConfigModel> GetConfigAsync();
        string AssetsPath { get; }
    }
}
