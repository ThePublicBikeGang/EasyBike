using CoreGraphics;
using EasyBike.ViewModels;
using GalaSoft.MvvmLight.Helpers;
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

        public MainViewModel Vm
        {
            get
            {
                return Application.Locator.Main;
            }
        }


        //MLPAutoCompleteTextField test = new MLPAutoCompleteTextField();
        MapView mapView;

        public override void LoadView()
        {
            base.LoadView();
            CameraPosition camera = CameraPosition.FromCamera(latitude: 37.797865,
                                                     longitude: -122.402526,
                                                     zoom: 6);
            mapView = MapView.FromCamera(CGRect.Empty, camera);
            mapView.MyLocationEnabled = true;
            mapView.Settings.CompassButton = true;
            mapView.Settings.MyLocationButton = true;

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
            ContractButton.SetCommand("TouchDown", Vm.ShowContractsCommand);
        }

        public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}

