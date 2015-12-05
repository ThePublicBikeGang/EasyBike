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
        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            
            SetContentView(Resource.Layout.About);

            //Resource.UpdateIdValues().aboutTitle = "TESTETSTE";
            //FindViewById<TextView>(Resource.Id.textView1).Text = "BLABLALBLAA";


        }




        //private View GetContractAdapter(int position, Contract contract, View convertView, View view2)
        //{
        //    // Not reusing views here
        //    convertView = LayoutInflater.Inflate(Resource.Layout.FlowerTemplate, null);

        //    var title = convertView.FindViewById<TextView>(Resource.Id.NameTextView);
        //    title.Text = contract.Name;

        //    //var image = convertView.FindViewById<ImageView>(Resource.Id.FlowerImageView);
        //    // ImageDownloader.AssignImageAsync(image, flower.Model.Image, this);

        //    return convertView;
        //}
    }
}