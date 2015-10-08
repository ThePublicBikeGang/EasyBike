using Android.Widget;
using EasyBike.Droid.Helpers;

namespace EasyBike.Droid.Views
{
    public partial class ContractsActivity  : ActivityBaseEx
    {
        private ListView _contractsList;

        public ListView ContractsList
        {
            get
            {
                return _contractsList
                       ?? (_contractsList = FindViewById<ListView>(Resource.Id.ContractsList));
            }
        }

    }
}