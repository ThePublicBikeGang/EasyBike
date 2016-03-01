using EasyBike.ViewModels;

namespace EasyBike.Services
{
    public interface ITileService
    {
        void UpdateTileOverlay(MainViewModel viewModel, bool programatic = false);
    }
}
