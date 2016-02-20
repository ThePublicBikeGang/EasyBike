using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using EasyBike.Droid.Views;
using System;

namespace EasyBike.Droid.Helpers
{
    public class AboutListAdapter2 : BaseAdapter
    {
        private readonly List<StoreLink> _items;
        readonly AboutActivity _context;

        public override int Count
        {
            get
            {
                return _items.Count;
            }
        }

        public AboutListAdapter2(AboutActivity newContext, List<StoreLink> items) : base()
        {
            _context = newContext;
            _items = items;
        }

        public override Java.Lang.Object GetItem(int position)
        {
            throw new NotImplementedException();
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = _context.LayoutInflater.Inflate(Resource.Layout.ShareLinkItem, null);
            var item = _items[position];
            view.FindViewById<ImageView>(Resource.Id.ShareLinkImage).SetImageResource(item.ImageRessourceId);
            view.FindViewById<TextView>(Resource.Id.ShareLinkText).Text = item.Text;
            view.SetOnClickListener(new ItemClickListener(_context, item));
            return view;
        }
    }

    /// <summary>
    /// Unused
    /// </summary>
    public class AboutListAdapter : RecyclerView.Adapter
    {
        private readonly List<StoreLink> _items;
        readonly AboutActivity _context;

        public AboutListAdapter(AboutActivity newContext, List<StoreLink> items) : base()
        {
            _context = newContext;
            _items = items;
            totalItemsHeight = 0;
        }

        public override int ItemCount
        {
            get
            {
                return _items.Count;
            }
        }
        static int totalItemsHeight;
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
            holder.Text.Text = item.Text;

            holder.ItemView.SetOnClickListener(new ItemClickListener(_context, item));
            holder.ItemView.Measure(0, 0);


        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var view = _context.LayoutInflater.Inflate(Resource.Layout.ShareLinkItem, null);
            return new ShareLinkViewHolder(view);
        }

        /**
         * Sets RecyclerView height dynamically based on the height of the items.   
         *
         * @param recyclerView to be resized
         * @return true if the listView is successfully resized, false otherwise
         */
        public static bool SetListViewHeightBasedOnItems(RecyclerView recyclerView)
        {

            // Set list height.
            var param = recyclerView.LayoutParameters;
            param.Height = totalItemsHeight;
            recyclerView.LayoutParameters = param;
            recyclerView.RequestLayout();
            return true;
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