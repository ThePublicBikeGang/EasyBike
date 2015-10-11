using Android.Widget;
using EasyBike.Droid.Helpers;

namespace EasyBike.Droid.Views
{
    public partial class ContractsActivity  : ActivityBaseEx
    {
        private ExpandableListView _contractsList;

        public ExpandableListView ContractsList
        {
            get
            {
                return _contractsList
                       ?? (_contractsList = FindViewById<ExpandableListView>(Resource.Id.ContractsList));
            }
        }

    }
}