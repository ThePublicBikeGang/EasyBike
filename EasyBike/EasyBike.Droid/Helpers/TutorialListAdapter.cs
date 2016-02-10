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
            holder.Title.Text = item.Title;
            holder.Text.Text = item.Text;
            holder.Image.SetImageResource(item.ImageResource);
            holder.Image.RequestLayout();
            holder.Details.Text = item.Text;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var view = _context.LayoutInflater.Inflate(Resource.Layout.TutorialItem, null);
            return new TutorialViewHolder(view, _context);
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
        private Activity _context;
        public TutorialViewHolder(View view, Activity context) : base(view)
        {
            _itemView = view;
            _context = context;
            Title = _itemView.FindViewById<TextView>(Resource.Id.TutorialTitle);
            Image = _itemView.FindViewById<ImageView>(Resource.Id.TutorialImage);
            Text = _itemView.FindViewById<TextView>(Resource.Id.TutorialText);
            Details = _itemView.FindViewById<TextView>(Resource.Id.TutorialDetails);
        }
    }
}