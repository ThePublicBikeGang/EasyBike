using GalaSoft.MvvmLight;
using PublicBikes.Config;

namespace PublicBikes.ViewModels
{
    public class AboutViewModel : ViewModelBase
    {
        private readonly IConfigService _configService;

        public AboutViewModel(IConfigService configService)
        {
            _configService = configService;
            Init();
        }

        public async void Init()
        {
            _twitter = (await _configService.GetConfigAsync()).Twitter;
        }

        private string _twitter;
        public string Twitter
        {
            get { return _twitter; }
        }

    }
}
