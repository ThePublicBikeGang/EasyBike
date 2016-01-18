using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Gms.Maps;
using Plugin.CurrentActivity;

namespace EasyBike.Droid.Helpers
{
    public class MapFragmentExtended : MapFragment
    {
        public View originalView;
        public TouchableWrapper touchView;

        public static MapFragmentExtended NewInstance(GoogleMapOptions options)
        {
            Bundle arguments = new Bundle();
            arguments.PutParcelable("MapOptions", options);

            MapFragmentExtended fragment = new MapFragmentExtended();
            fragment.Arguments = arguments;
            return fragment;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            touchView = new TouchableWrapper(CrossCurrentActivity.Current.Activity);
            touchView.AddView(base.OnCreateView(inflater, container, savedInstanceState));
            return touchView;
        }
    }
}