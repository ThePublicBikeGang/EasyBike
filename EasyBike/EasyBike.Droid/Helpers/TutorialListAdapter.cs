using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using EasyBike.Droid.Views;

namespace EasyBike.Droid.Helpers
{
    public class TutorialListAdapter : RecyclerView.Adapter
    {

        private readonly List<TutorialItem> _items;
        private readonly Activity _context;

        public TutorialListAdapter(Activity context, List<TutorialItem> favorits)
        {
            _context = context;
            _items = favorits;
        }

        public override int ItemCount
        {
            get
            {
                return _items.Count;
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = _items[position];
            // Replace the contents of the view with that element
            var holder = viewHolder as TutorialViewHolder;
            if (item.ImageResource != 0)
            {
                holder.Image.SetImageResource(item.ImageResource);
                holder.Image.RequestLayout();
            }
            else
            {
            }

            if (!string.IsNullOrWhiteSpace(item.Title))
            {
                holder.Title.Text = item.Title;
                holder.Title.Visibility = ViewStates.Visible;
            }
            else
            {
                holder.Title.Visibility = ViewStates.Gone;
            }
            if (!string.IsNullOrWhiteSpace(item.Text))
            {
                holder.Text.Text = item.Text;
                holder.Text.Visibility = ViewStates.Visible;
            }
            else
            {
                holder.Text.Visibility = ViewStates.Gone;
            }
            if (!string.IsNullOrWhiteSpace(item.Details))
            {
                holder.Details.Text = item.Details;
                holder.Details.Visibility = ViewStates.Visible;
            }
            else
            {
                holder.Details.Visibility = ViewStates.Gone;
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var view = _context.LayoutInflater.Inflate(Resource.Layout.TutorialItem, null);
            return new TutorialViewHolder(view);
        }
    }

    // Provide a reference to the views for each data item
    // Complex data items may need more than one view per item, and
    // you provide access to all the views for a data item in a view holder
    public class TutorialViewHolder : RecyclerView.ViewHolder
    {
        // each data item is just a string in this case
        public TextView Title;
        public TextView Text;
        public TextView Details;
        public ImageView Image;
        public View _itemView;
        public TutorialViewHolder(View view) : base(view)
        {
            _itemView = view;
            Title = _itemView.FindViewById<TextView>(Resource.Id.TutorialTitle);
            Image = _itemView.FindViewById<ImageView>(Resource.Id.TutorialImage);
            Text = _itemView.FindViewById<TextView>(Resource.Id.TutorialText);
            Details = _itemView.FindViewById<TextView>(Resource.Id.TutorialDetails);
        }
    }
}