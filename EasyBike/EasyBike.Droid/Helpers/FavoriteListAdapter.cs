using Android.App;
using Android.Views;
using EasyBike.Models.Favorites;
using Android.Support.V7.Widget;
using Android.Widget;
using System.Collections.Generic;
using XamarinItemTouchHelper;
using Android.Content;
using Newtonsoft.Json;

namespace EasyBike.Droid.Helpers
{
    public class FavoriteListAdapter : RecyclerView.Adapter, IItemTouchHelperAdapter
    {
        // Start with first item selected
        public int SelectedItem = 0;
        public class ItemClickListener : Java.Lang.Object, View.IOnClickListener
        {
            Activity _context;
            public ItemClickListener(Activity context)
            {
                _context = context;
            }
            public void OnClick(View v)
            {
                var data = new Intent();
                data.PutExtra("favorite", JsonConvert.SerializeObject((v.Tag as ItemViewHolder).Favorite));
                _context.SetResult(Result.Ok, data);
                _context.Finish();
            }
        }

        // Provide a reference to the views for each data item
        // Complex data items may need more than one view per item, and
        // you provide access to all the views for a data item in a view holder
        public class ItemViewHolder : RecyclerView.ViewHolder, IItemTouchHelperViewHolder
        {
            // each data item is just a string in this case
            public TextView FavoriteName;
            public TextView FavoriteAdresse;
            public Favorite Favorite;
            public View _itemView;
            private Activity _context;
            public ItemViewHolder(View view, Activity context) : base(view)
            {
                _itemView = view;
                view.Tag = this;
                _context = context;
                FavoriteName = _itemView.FindViewById<TextView>(Resource.Id.FavoriteItemName);
                FavoriteAdresse = _itemView.FindViewById<TextView>(Resource.Id.FavoriteItemAddress);
                _itemView.SetOnClickListener(new ItemClickListener(_context));
            }

            public void OnItemSelected()
            {
                // _itemView.SetBackgroundColor(Color.LightGray);
            }

            public void OnItemClear()
            {
                // _itemView.SetBackgroundColor(Color.White);
            }



        }

        private readonly List<Favorite> _favorits;
        private readonly Activity _context;
        private readonly IOnStartDragListener _dragStartListener;

        public FavoriteListAdapter(Activity context, List<Favorite> favorits, IOnStartDragListener dragStartListener)
        {
            _dragStartListener = dragStartListener;
            _context = context;
            _favorits = favorits;
        }

        public override int ItemCount
        {
            get
            {
                return _favorits.Count;
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = _favorits[position];
            // Replace the contents of the view with that element
            var holder = viewHolder as ItemViewHolder;
            holder.Favorite = item;
            holder.FavoriteAdresse.Text = item.Address;
            holder.FavoriteName.Text = item.Name;
            holder.ItemView.SetOnTouchListener(new TouchListenerHelper(holder, _dragStartListener));
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var view = _context.LayoutInflater.Inflate(Resource.Layout.FavoriteItem, null);
            return new ItemViewHolder(view, _context);
        }

        public bool OnItemMove(int fromPosition, int toPosition)
        {
            return true;
            //  throw new NotImplementedException();
        }

        public void OnItemDismiss(int position)
        {
            _favorits.Remove(_favorits[position]);
            NotifyItemRemoved(position);
            // throw new NotImplementedException();
        }


        //public void OnItemClick(AdapterView parent, View view, int position, long id)
        //{
        //    //row.FindViewById<TextView>(Resource.Id.NameTextView).Text = _countries[groupPosition].Contracts[childPosition].Name;
        //    //// var contractCommandBinding = this.SetBinding(() => _countries[groupPosition].Contracts[childPosition]);
        //    //var _refreshCommandBinding = row.SetBinding(() => row.Text);
        //    //row.SetCommand(
        //    //    "Click",
        //    //    (Context as ContractsActivity).ViewModel.ContractTappedCommand, _refreshCommandBinding);
        //}
    }
}