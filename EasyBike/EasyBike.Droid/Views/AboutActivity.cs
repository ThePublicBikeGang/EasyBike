using Android.App;
using Android.OS;
using Android.Widget;
using Android.Text.Method;

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
			LinearLayout aboutMainLayout = FindViewById<LinearLayout> (Resource.Id.aboutMainLayout);
			// Loop over the layout looking for TextView
			for(int i = 0; i < aboutMainLayout.ChildCount; i++) 
			{
				if (aboutMainLayout.GetChildAt(i) is TextView) 
				{
					TextView tv = (TextView) aboutMainLayout.GetChildAt (i);
					// This is to make the "a href" clickable and it opens a web browser
					tv.MovementMethod = LinkMovementMethod.Instance;
				}
			}
        }
    }
}
