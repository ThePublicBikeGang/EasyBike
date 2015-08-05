using Android.Widget;
using PublicBikes.Droid.Helpers;

namespace PublicBikes.Droid.Views
{
    public partial class MainActivity : ActivityBaseEx
    {
        private Button _refreshButton;

        public Button RefreshButton
        {
            get
            {
                return _refreshButton
                       ?? (_refreshButton = FindViewById<Button>(Resource.Id.GoToContractView));
            }
        }
    }
}