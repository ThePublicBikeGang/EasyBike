using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace EasyBike.Config
{
    public class ConfigService : IConfigService
    {
        private ConfigModel _config;
        public string AssetsPath
        {
            get;
            private set;
        }

        public ConfigService()
        {
            AssetsPath = $"{GetType().Namespace}.Assets.Flag.";
        }

        public async Task<ConfigModel> GetConfigAsync()
        {
            if (_config == null)
            {
                //var platform = Device.OnPlatform("iOS", "Droid", "WinPhone");
                //// bug using the emulator, Device.OS is 'Other'
                //if (Device.OS == TargetPlatform.Other)
                //{
                //    platform = "WinPhone";
                //}

                // you need to create your own "MyConfig.json" which will be a copy of "configExemple.json" to store your own API keys
                var configFile = "MyConfig.json";
                var resource = $"{GetType().Namespace}.{configFile}";
                var assembly = typeof(ConfigService).GetTypeInfo().Assembly;
                using (var stream = assembly.GetManifestResourceStream(resource))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        _config = JsonConvert.DeserializeObject<ConfigModel>(await reader.ReadToEndAsync().ConfigureAwait(false));
                    }
                }
            }
            return _config;
        }
    }
}
