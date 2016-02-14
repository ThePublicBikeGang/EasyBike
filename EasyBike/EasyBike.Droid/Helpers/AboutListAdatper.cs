using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using EasyBike.Droid.Views;

namespace EasyBike.Droid.Helpers
{
    public class AboutListAdapter : RecyclerView.Adapter
    {
        private readonly List<StoreLink> _items;
        readonly AboutActivity _context;

        public AboutListAdapter(AboutActivity newContext, List<StoreLink> items) : base()
        {
            _context = newContext;
            _items = items;
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
            var holder = viewHolder as ShareLinkViewHolder;
            if (item.ImageRessourceId != 0)
            {
                holder.Image.SetImageResource(item.ImageRessourceId);
                holder.Image.RequestLayout();
            }
            else
            {
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
            holder.ItemView.SetOnClickListener(new ItemClickListener(_context, item));
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var view = _context.LayoutInflater.Inflate(Resource.Layout.ShareLinkItem, null);

            return new ShareLinkViewHolder(view);
        }
    }

    public class ItemClickListener : Java.Lang.Object, View.IOnClickListener
    {
        private StoreLink _item;
        private AboutActivity _context;
        public ItemClickListener(AboutActivity context, StoreLink item)
        {
            _context = context;
            _item = item;
        }

        public void OnClick(View v)
        {
            _context.List_Click(_item);
        }
    }

    // Provide a reference to the views for each data item
    // Complex data items may need more than one view per item, and
    // you provide access to all the views for a data item in a view holder
    public class ShareLinkViewHolder : RecyclerView.ViewHolder
    {
        public TextView Text;
        public ImageView Image;
        public View _itemView;
        public ShareLinkViewHolder(View view) : base(view)
        {
            _itemView = view;
            Image = _itemView.FindViewById<ImageView>(Resource.Id.ShareLinkImage);
            Text = _itemView.FindViewById<TextView>(Resource.Id.ShareLinkText);
        }
    }
}