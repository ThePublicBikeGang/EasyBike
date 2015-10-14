using Android.Widget;
using EasyBike.Droid.Helpers;
using EasyBike.ViewModels;
using GalaSoft.MvvmLight.Ioc;

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

        private ContractsViewModel _vm;
        public ContractsViewModel ViewModel
        {
            get
            {
                return _vm ?? (_vm = SimpleIoc.Default.GetInstanceWithoutCaching<ContractsViewModel>());
            }
        }

    }
}