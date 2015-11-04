using Android.App;
using Android.Widget;
using EasyBike.Droid.Helpers;

namespace EasyBike.Droid
{
  
    public partial class MainActivity : ActivityBaseEx, AppCompatActivity
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