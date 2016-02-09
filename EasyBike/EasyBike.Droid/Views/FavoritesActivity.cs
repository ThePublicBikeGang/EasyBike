using Android.App;
using Android.OS;

using GalaSoft.MvvmLight.Ioc;

using EasyBike.Models.Favorites;
using Android.Support.V7.Widget;
using EasyBike.Droid.Helpers;
using Android.Support.V7.Widget.Helper;
using XamarinItemTouchHelper;
using Android.Widget;
using Android.Views.Animations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyBike.Droid
{
    [Activity(Label = "FavoritesActivity")]
    public class FavoritesActivity : Activity,  IOnStartDragListener
    {
        private IFavoritesService _favoritesService;

        private RecyclerView favoritesListView;
        private RecyclerView.Adapter mAdapter;
        private RecyclerView.LayoutManager _layoutManager;
        private ItemTouchHelper _itemTouchHelper;
        private TextView _placeHolder;
        private Animation _placeHolderAnimation;
        private List<Favorite> _favorites;

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Favorites);

            _favoritesService = SimpleIoc.Default.GetInstance<IFavoritesService>();
            _favorites = await _favoritesService.GetFavoritesAsync();

            favoritesListView = FindViewById<RecyclerView>(Resource.Id.FavoritesList);
            // use this setting to improve performance if you know that changes
            // in content do not change the layout size of the RecyclerView
            favoritesListView.HasFixedSize = true;
            // use a linear layout manager
            _layoutManager = new LinearLayoutManager(this);
            favoritesListView.SetLayoutManager(_layoutManager);
            // specify an adapter (see also next example)
            mAdapter = new FavoriteListAdapter(this, _favorites, this);
            favoritesListView.SetAdapter(mAdapter);
            favoritesListView.ChildViewRemoved += FavoritesListView_ChildViewRemoved;
            var callback = new SimpleItemTouchHelperCallback((IItemTouchHelperAdapter)mAdapter);
            _itemTouchHelper = new ItemTouchHelper(callback);
            _itemTouchHelper.AttachToRecyclerView(favoritesListView);

            _placeHolder = FindViewById<TextView>(Resource.Id.placeHolder);
            _placeHolderAnimation = AnimationUtils.LoadAnimation(this, Resource.Animation.placeholder);
            if(_favorites.Count == 0)
            {
                await Task.Delay(200);
                _placeHolder.StartAnimation(_placeHolderAnimation);
                _placeHolder.Visibility = Android.Views.ViewStates.Visible;
            }
        }

        private async void FavoritesListView_ChildViewRemoved(object sender, Android.Views.ViewGroup.ChildViewRemovedEventArgs e)
        {
            var itemViewHolder = (e.Child.Tag as FavoriteListAdapter.ItemViewHolder);
            await _favoritesService.RemoveFavoriteAsync(itemViewHolder.Favorite);
            if(_favorites.Count == 0)
            {
                _placeHolder.StartAnimation(_placeHolderAnimation);
                _placeHolder.Visibility = Android.Views.ViewStates.Visible;
            }
        }

        public void OnStartDrag(RecyclerView.ViewHolder viewHolder)
        {
            _itemTouchHelper.StartDrag(viewHolder);
        }
    }
}

