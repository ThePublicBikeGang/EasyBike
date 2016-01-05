using System;
using Android.App;
using Android.Support.V7.App;
using Android.Support.V7.View;
using Android.Widget;
using EasyBike.Droid.Helpers;

namespace EasyBike.Droid
{

    public partial class MainActivity : ActivityBaseEx, IAppCompatCallback
    {
        //private Button _refreshButton;

        //public Button RefreshButton
        //{
        //    get
        //    {
        //        return _refreshButton
        //               ?? (_refreshButton = FindViewById<Button>(Resource.Id.GoToContractView));
        //    }
        //}
        public void OnSupportActionModeFinished(ActionMode mode)
        {
            //  throw new NotImplementedException();
        }

        public void OnSupportActionModeStarted(ActionMode mode)
        {
            // throw new NotImplementedException();
        }

        public ActionMode OnWindowStartingSupportActionMode(ActionMode.ICallback callback)
        {
            return null;
        }
    }
}