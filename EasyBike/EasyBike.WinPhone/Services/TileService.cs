using EasyBike.Models.Storage;
using EasyBike.Resources;
using EasyBike.Services;
using EasyBike.ViewModels;
using Windows.UI.Xaml.Controls.Maps;

namespace EasyBike.WinPhone.Services
{
    public class TileService : ITileService
    {
        private readonly ISettingsService _settingsService;
        public TileService(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public void UpdateTileOverlay(MainViewModel viewModel, bool programatic = false)
        {
            if (!programatic)
            {
                if (viewModel.SelectedTile == null)
                {
                    viewModel.SelectedTile = StaticResources.TilesList.First;
                }
                viewModel.SelectedTile = viewModel.SelectedTile.Next ?? StaticResources.TilesList.First;
            }
            MainPage.Map.TileSources.Clear();
            // if native map, just remove the previous overlay 
            if (viewModel.SelectedTile.Value.NativeMapLayer)
            {
                if (viewModel.SelectedTile.Value.Name == StaticResources.TilesHybridName)
                {
                    MainPage.Map.Style = MapStyle.AerialWithRoads;
                }
                else
                {
                    MainPage.Map.Style = MapStyle.Road;
                }
            }
            else
            {
                MainPage.Map.Style = MapStyle.None;
                var maxZoom = viewModel.SelectedTile.Value.MaxZoom;
                if (maxZoom == 0)
                {
                    maxZoom = (int)MainPage.Map.MaxZoomLevel;
                }
                
                if (MainPage.Map.ZoomLevel > maxZoom)
                {
                    MainPage.Map.ZoomLevel = maxZoom;
                }
                if (viewModel.SelectedTile.Value.Name == StaticResources.TilesOpenStreetMapName)
                {
                    var httpsource = new HttpMapTileDataSource(StaticResources.TilesMapnik);
                    var tilesource = new MapTileSource(httpsource)
                    {
                        Layer = MapTileLayer.BackgroundReplacement
                    };

                    MainPage.Map.TileSources.Add(tilesource);
                }
                else if (viewModel.SelectedTile.Value.Name == StaticResources.TilesOpenCycleMapName)
                {
                    var httpsource = new HttpMapTileDataSource(StaticResources.TilesMaquest);
                    var tilesource = new MapTileSource(httpsource)
                    {
                        Layer = MapTileLayer.BackgroundReplacement
                    };
                    MainPage.Map.TileSources.Add(tilesource);
                }
            }
            if (!programatic)
            {
                viewModel.TileName = viewModel.SelectedTile.Value.Name;
                viewModel.VisualState = "normalTileName";
                viewModel.VisualState = "showTileName";
            }
            _settingsService.MapTile = viewModel.SelectedTile.Value.Name;
        }
    }
}
