using UIKit;
using Foundation;

namespace EasyBike.iOS.Views
{
    [Register("UniversalView")]
    public class UniversalView : UIView
    {
        public UniversalView()
        {
            Initialize();
        }


        void Initialize()
        {
            BackgroundColor = UIColor.Red;
        }
    }

    [Register("ContractsViewController")]
    public class ContractsViewController : UIViewController
    {
        public ContractsViewController()
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            View = new UniversalView();

            base.ViewDidLoad();

            // Perform any additional setup after loading the view
        }
    }
}