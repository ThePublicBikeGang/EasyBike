using Android.App;
using Android.OS;
using GalaSoft.MvvmLight.Helpers;
using EasyBike.Droid.Helpers;
using EasyBike.ViewModels;

namespace EasyBike.Droid.Views
{
    [Activity(Label = "MainActivity")]
    public partial class MainActivity : ActivityBaseEx
    {
        private Binding _lastLoadedBinding;

        public MainViewModel Vm
        {
            get
            {
                return App.Locator.Main;
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            RefreshButton.SetCommand(
                "Click",
                Vm.GoToDownloadCitiesCommand);
        }
    }
}