using Android.App;
using Android.OS;
using Android.Widget;
using Android.Text.Method;
using Android.Database;
using Android.Views;
using System;
using Java.Lang;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using EasyBike.Droid.Helpers;
using Android.Content;

namespace EasyBike.Droid.Views
{
    [Activity(Label = "AboutActivity")]
    public partial class AboutActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.About);

            // TODO cf. https://forums.xamarin.com/discussion/comment/61781/#Comment_61781
            // Use a ViewModel to prevent the use of this loop ?
            LinearLayout aboutMainLayout = FindViewById<LinearLayout>(Resource.Id.aboutMainLayout);
            // Loop over the layout looking for TextView
            for (int i = 0; i < aboutMainLayout.ChildCount; i++)
            {
                if (aboutMainLayout.GetChildAt(i) is TextView)
                {
                    TextView tv = (TextView)aboutMainLayout.GetChildAt(i);
                    // This is to make the "a href" clickable and it opens a web browser
                    tv.MovementMethod = LinkMovementMethod.Instance;
                }
            }

            List<StoreLink> links = new List<StoreLink>()
            {
                  new StoreLink() {Text =Resources.GetString(Resource.String.aboutWinPhone),
                 ImageRessourceId =  Resource.Drawable.ic_wpstore}
            };
            var list = FindViewById<RecyclerView>(Resource.Id.OtherDeviceAppList);
            list.HasFixedSize = true;
            var layoutManager = new LinearLayoutManager(this);
            layoutManager.Orientation = (int)Orientation.Vertical;
            list.SetLayoutManager(layoutManager);
            list.SetAdapter(new AboutListAdapter(this, links));
        }

        public void List_Click(StoreLink item)
        {
            var shareIntent = new Intent(Intent.ActionSend);
            var text = string.Empty;
            if (item.Text.Contains("Windows"))
            {
                text = "Hey! Check out EasyBike!\r\nWindows Phone 10: https://www.microsoft.com/store/apps/9wzdncrdkng9 \r\nWindows Phone 8.1: http://windowsphone.com/s?appid=191ef96d-e185-47d1-80a3-377ebfefa325";
            }
            shareIntent.PutExtra(Intent.ExtraText, text);
            shareIntent.SetType("text/plain");
            StartActivity(Intent.CreateChooser(shareIntent, Resources.GetString(Resource.String.share)));
        }
    }
    public class StoreLink
    {
        public string Text { get; set; }
        public int ImageRessourceId { get; set; }
    }





}
