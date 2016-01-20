using System.Collections.Generic;
using Android.Content;
using Android.Widget;
using EasyBike.Droid.Models;

namespace EasyBike.Droid.Helpers
{
    public class GooglePlacesAutocompleteAdapter : ArrayAdapter
    {
        public List<Prediction> Results;

        public GooglePlacesAutocompleteAdapter(Context context, int textViewResourceId) : base (context, textViewResourceId)
        {
        }

        public override int Count
        {
            get
            {
                return Results.Count;
            }
        }
        public override Java.Lang.Object GetItem(int position)
        {
            return Results[position].ToString();  
        }

    }
}