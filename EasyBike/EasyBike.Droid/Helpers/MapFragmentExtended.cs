using Android.Views;
using Android.Widget;
using Android.Gms.Maps;
using Plugin.CurrentActivity;
using Android.OS;

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
            //var mapView = base.OnCreateView(inflater, container, savedInstanceState);
            // move the compass native button at the same place as in the last google map app
            // Get the button view 
            //View test = ((View)mapView.FindViewById(1).Parent);
            //System.Diagnostics.Debug.WriteLine(test);
            //View nativeCompassbutton = ((View)mapView.FindViewById(1).Parent).FindViewById(3);
            //System.Diagnostics.Debug.WriteLine("gesssssssssssssssssssssssssssss");
            //System.Diagnostics.Debug.WriteLine(nativeCompassbutton);
            //RelativeLayout.LayoutParams rlp = (RelativeLayout.LayoutParams)nativeCompassbutton.LayoutParameters;
            //rlp.AddRule(LayoutRules.AlignParentTop, 0);
            //rlp.AddRule(LayoutRules.AlignParentBottom, 1);
            //rlp.SetMargins(0, 0, 30, 30);

            // enable a way to detect when the user is actually moving the map to unstick to the current user location for instance
            touchView = new TouchableWrapper(CrossCurrentActivity.Current.Activity);
            touchView.AddView(base.OnCreateView(inflater, container, savedInstanceState));
            return touchView;
        }
    }
}