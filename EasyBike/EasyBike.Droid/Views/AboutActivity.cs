using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace EasyBike.Droid.Views
{
    [Activity(Label = "AboutActivity")]
    public partial class AboutActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.About);

            // TODO delete?
            //Resource.UpdateIdValues().aboutTitle = "TESTETSTE";
//            FindViewById<TextView>(Resource.Id.textReviewRate).SetOnClickListener();
            //FindViewById<TextView>(Resource.Id.textView1).Text = "BLABLALBLAA";
        }
    }
}
