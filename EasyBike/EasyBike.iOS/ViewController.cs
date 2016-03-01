using CoreGraphics;
using Google.Maps;
using System;

using UIKit;

namespace EasyBike.iOS
{
	public partial class ViewController : UIViewController
	{
		public ViewController (IntPtr handle) : base (handle)
		{
		}


        MapView mapView;

        public override void LoadView()
        {
            base.LoadView();
            CameraPosition camera = CameraPosition.FromCamera(latitude: 37.797865,
                                                     longitude: -122.402526,
                                                     zoom: 6);
            mapView = MapView.FromCamera(CGRect.Empty, camera);
            mapView.MyLocationEnabled = true;

            View = mapView;
        }
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            mapView.StartRendering();
        }

        public override void ViewWillDisappear(bool animated)
        {
            mapView.StopRendering();
            base.ViewWillDisappear(animated);
        }
        public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}

