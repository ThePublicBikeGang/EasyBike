
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using GalaSoft.MvvmLight.Ioc;

using EasyBike.Models.Favorites;

namespace EasyBike.Droid
{
    [Activity(Label = "FavoritesActivity")]
    public class FavoritesActivity : Activity
    {
        private IFavoritesService _favoritesService;

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Favorites);

            string[,] data = new string[,]{{"Bill Gates", "06 06 06 06 06"},
                {"Niels Bohr", "05 05 05 05 05"},
                {"Alexandre III de Macédoine", "04 04 04 04 04"}};

            var list = new List<IDictionary<string, object>>();
            for (int i=0; i< data.GetLength(0); i++) {
                var item = new JavaDictionary<string, object>();
                item.Add("FavoriteItemName", data[i,0]);
                item.Add("FavoriteItemAddress", data[i,1]);
                list.Add(item);
            }
            _favoritesService = SimpleIoc.Default.GetInstance<IFavoritesService>();
            var favorites = await _favoritesService.GetFavoritesAsync();
            foreach (Favorite f in favorites) {
                
            }
            var favoritesListView = FindViewById<ListView>(Resource.Id.FavoritesList);
           // favoritesListView.Adapter = new SimpleAdapter(this, list, Resource.Layout.FavoriteItem, new string[]{"FavoriteItemName", "FavoriteItemAddress"}, new int[]{Android.Resource.Id.Text1, Android.Resource.Id.Text2});
        }
    }
}

