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
           // list.Click += List_Click;
        }

        private void List_Click(object sender, EventArgs e)
        {
           
        }
    }
    public class StoreLink
    {
        public string Text { get; set; }
        public int ImageRessourceId { get; set; }
    }

  
}
