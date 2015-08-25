using System.Threading.Tasks;

namespace EasyBike.Config
{
    public interface IConfigService
    {
        Task<ConfigModel> GetConfigAsync();
        string AssetsPath { get; }
    }
}
