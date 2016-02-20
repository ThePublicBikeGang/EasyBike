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
using System.Threading.Tasks;
using EasyBike.Resources;

namespace EasyBike.Droid.Views
{
    [Activity(Label = "AboutActivity")]
    public class AboutActivity : Activity
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
            var list = FindViewById<ListView>(Resource.Id.OtherDeviceAppList);
            var layoutManager = new CustomLinearLayoutManager(this);
            layoutManager.Orientation = (int)Orientation.Vertical;
            list.Adapter = (new AboutListAdapter2(this, links));
        }

        public void List_Click(StoreLink item)
        {
            var shareIntent = new Intent(Intent.ActionSend);
            var text = StringResources.FormatShareMessage();
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
    public class CustomLinearLayoutManager : LinearLayoutManager
    {
        public CustomLinearLayoutManager(Activity activity) : base(activity)
        {
        }
        public override bool CanScrollVertically()
        {
            return false;
        }

        private int[] mMeasuredDimension = new int[2];


        public override void OnMeasure(RecyclerView.Recycler recycler, RecyclerView.State state,
                              int widthSpec, int heightSpec)
        {
            var widthMode = View.MeasureSpec.GetMode(widthSpec);
            var heightMode = View.MeasureSpec.GetMode(heightSpec);
            int widthSize = View.MeasureSpec.GetSize(widthSpec);
            int heightSize = View.MeasureSpec.GetSize(heightSpec);
            int width = 0;
            int height = 0;
            for (int i = 0; i < ItemCount; i++)
            {
                measureScrapChild(recycler, i,
                        View.MeasureSpec.MakeMeasureSpec(i, MeasureSpecMode.Unspecified),
                        View.MeasureSpec.MakeMeasureSpec(i, MeasureSpecMode.Unspecified),
                        mMeasuredDimension);

                if (Orientation == Horizontal)
                {
                    width = width + mMeasuredDimension[0];
                    if (i == 0)
                    {
                        height = mMeasuredDimension[1];
                    }
                }
                else {
                    height = height + mMeasuredDimension[1];
                    if (i == 0)
                    {
                        width = mMeasuredDimension[0];
                    }
                }
            }
            switch (widthMode)
            {
                case MeasureSpecMode.Exactly:
                    width = widthSize;
                    break;
                case MeasureSpecMode.AtMost:
                case MeasureSpecMode.Unspecified:
                    break;
            }

            switch (heightMode)
            {
                case MeasureSpecMode.Exactly:
                    height = heightSize;
                    break;
                case MeasureSpecMode.AtMost:
                case MeasureSpecMode.Unspecified:
                    break;
            }

            SetMeasuredDimension(width, height);
        }

        private void measureScrapChild(RecyclerView.Recycler recycler, int position, int widthSpec,
                                       int heightSpec, int[] measuredDimension)
        {
            View view = recycler.GetViewForPosition(position);
            if (view != null)
            {
                RecyclerView.LayoutParams p = (RecyclerView.LayoutParams)view.LayoutParameters;
                int childWidthSpec = ViewGroup.GetChildMeasureSpec(widthSpec,
                        PaddingLeft + PaddingRight, p.Width);
                int childHeightSpec = ViewGroup.GetChildMeasureSpec(heightSpec,
                        PaddingTop + PaddingBottom, p.Height);
                view.Measure(childWidthSpec, childHeightSpec);
                measuredDimension[0] = view.MeasuredWidth + p.LeftMargin + p.RightMargin;
                measuredDimension[1] = view.MeasuredHeight + p.BottomMargin + p.TopMargin;
                recycler.RecycleView(view);
            }
        }
    }




}
