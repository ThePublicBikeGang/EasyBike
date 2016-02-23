using Android.Views;
using Android.Widget;
using Android.Gms.Maps;
using Plugin.CurrentActivity;
using Android.OS;

namespace EasyBike.Droid.Helpers
{
    public class CompassClickListener : Java.Lang.Object, View.IOnClickListener
    {
        // listen to click on compass to stop compass mode
        public void OnClick(View v)
        {
            var activity = CrossCurrentActivity.Current.Activity as MainActivity;
            if (activity != null)
            {
                activity.DisableCompass();
            }
        }
    }
    public class MapFragmentExtended : MapFragment
    {
        public TouchableWrapper touchView;

        public static MapFragmentExtended NewInstance(GoogleMapOptions options)
        {
            Bundle arguments = new Bundle();
            arguments.PutParcelable("MapOptions", options);

            MapFragmentExtended fragment = new MapFragmentExtended();
            fragment.Arguments = arguments;
            return fragment;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var mapView = base.OnCreateView(inflater, container, savedInstanceState);
            try
            {
                // tags: GoogleMapCompass (id 4)
                // GoogleMapMyLocationButton 
                var compassView = mapView.FindViewWithTag("GoogleMapCompass");
                ViewGroup parent = (ViewGroup)compassView.Parent;

                RelativeLayout.LayoutParams rlp = (RelativeLayout.LayoutParams)compassView.LayoutParameters;
                // position on top right
                rlp.AddRule(LayoutRules.AlignParentEnd);
                rlp.RemoveRule(LayoutRules.AlignParentStart);
                rlp.RightMargin = (int)LayoutHelper.ConvertDpToPixel(20);
                rlp.TopMargin = (int)LayoutHelper.ConvertDpToPixel(155);

                compassView.RequestLayout();
                // listen to click on compass to stop compass mode
                compassView.SetOnClickListener(new CompassClickListener());

                //ViewGroup parent = (ViewGroup)mapView.FindViewWithTag("GoogleMapMyLocationButton").Parent;
                //for (int i = 0, n = parent.ChildCount; i < n; i++)
                //{
                //    View view = parent.GetChildAt(i);
                //    System.Diagnostics.Debug.WriteLine(i);
                //    System.Diagnostics.Debug.WriteLine(view.Tag);
                //    RelativeLayout.LayoutParams rlp = (RelativeLayout.LayoutParams)view.LayoutParameters;
                //    // position on top right
                //    rlp.AddRule(LayoutRules.AlignParentLeft, 0);
                //    rlp.AddRule(LayoutRules.AlignParentTop,0);
                //    rlp.AddRule(LayoutRules.AlignParentRight);
                //    rlp.AddRule(LayoutRules.AlignParentBottom);
                //    rlp.RightMargin = rlp.LeftMargin;
                //    rlp.TopMargin = 25;
                //    view.RequestLayout();
                //}
            }
            catch
            {
                // ignore 
            }

            // enable a way to detect when the user is actually moving the map to unstick to the current user location for instance
            touchView = new TouchableWrapper(CrossCurrentActivity.Current.Activity);
            touchView.AddView(mapView);
            return touchView;
        }
    }
}