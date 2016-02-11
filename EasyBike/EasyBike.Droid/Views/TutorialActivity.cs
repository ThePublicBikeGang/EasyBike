using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using EasyBike.Droid.Helpers;
using System.Collections.Generic;

namespace EasyBike.Droid.Views
{
    [Activity(Label = "TutorialActivity")]
    public class TutorialActivity : Activity
    {

        private RecyclerView _tutorialListView;
        private RecyclerView.Adapter mAdapter;
        private RecyclerView.LayoutManager _layoutManager;
        private List<TutorialItem> _items;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Tutorial);

            _items = new List<TutorialItem>();
            _items.Add(new TutorialItem()
            {
                Title = "Download a city",
                Text = "Tap on \"download cities\" on the menu panel",
                ImageResource = Resource.Drawable.download1,
            });
            _items.Add(new TutorialItem()
            {
                Title = "",
                Text = "It will show the available cities list. Then tap on a city to download or remove it.",
                Details = "It will download the stations list from the selected service." +
             " If there is some new stations provided by the public bike service, you can refresh that list by removing and downloading again the city." +
             " This list will get bigger on future updates",
                ImageResource = Resource.Drawable.download2,
            });
            _items.Add(new TutorialItem()
            {
                Title = "Switch between bike and station mode",
                Text = "Tap on the bike icon or (P) icon on the bottom right corner.",
                ImageResource = Resource.Drawable.switchmode,
            });
            _items.Add(new TutorialItem()
            {
                Title = "Use the compass",
                Text = "Tap once on the position button in the command bar, it will focus on your location." +
                " Tap again on the button to activate the compass.",

                Details = "If you tapped once on the location button, it will change the location button for the compass button." +
                "Dragging the map when you are in location or compass mode will deactivate the location or compass mode." +
                "To reset the north of the map just click on the top right corner button that appear if the north of the map have changed.",
                ImageResource = Resource.Drawable.compassmode,
            });
            _items.Add(new TutorialItem()
            {
                Title = "Get directions to go somewhere",
                Text = "Tap on a station or a searched location, it will show up a path walk to get there." +
                            " Hold on a station or on a searched location to show up a command panel. Press \"Drive to\"." +
                            " It will start your favorite driving app like HERE drive.",
                Details = "You can then get your" +
                            " headphones on, lock your phone, put it in your pocket and listen to the directions" +
                            " from the HERE drive app :) (take care, it's usally a car itinerary so you might need to tweak it a bit)",
                ImageResource = Resource.Drawable.cyclemode,
            });
            _items.Add(new TutorialItem()
            {
                Title = "Add some favorites",
                Text = "Tao on a station or on a searched location to show up an action bar, then press the star icon.",
                Details = "It will store the location of the point. You can name it as you like and manage" +
                            " favorites from the dedicated page.",
                ImageResource = Resource.Drawable.addFavorite,
            });
            _items.Add(new TutorialItem()
            {
                Title = "Get the address from a location",
                Text = "Hold on the map. It will shows up the found address and the walk path to get there if it exist.",
                Details = "",
                ImageResource = Resource.Drawable.cyclemode,
            });
            _items.Add(new TutorialItem()
            {
                Title = "Search for an address",
                Text = "Tap the search icon button in the command bar and type an address in the search text box.",
                Details = "",
                ImageResource = Resource.Drawable.cyclemode,
            });

            _items.Add(new TutorialItem()
            {
                Title = "Share a location",
                Text = "By holding on a bike station or tapping on a searched location, you can share that position by mail or text.",
                ImageResource = Resource.Drawable.shareLocation,
            });
            _items.Add(new TutorialItem()
            {
                Text = "The message will automatically contains the address if it has been resolved." +
                " It will also contains direct link to the EasyBike app, IPhone/IPad native map and Google map.",
                ImageResource = Resource.Drawable.cyclemode,
            });

            _items.Add(new TutorialItem()
            {
                Title = "Tips : Store your home/car location",
                Text = "Like a \"Dude, where is my car\" app, if you want to easely get back to your home / car / whereever location, just hold a finger on the" +
                " location and add it to your favorites.",
                Details = "It's then really easy to get back somewhere.",
                ImageResource = Resource.Drawable.cyclemode,
            });

            _tutorialListView = FindViewById<RecyclerView>(Resource.Id.TutorialList);
            // use this setting to improve performance if you know that changes
            // in content do not change the layout size of the RecyclerView
            _tutorialListView.HasFixedSize = true;
            // use a linear layout manager
            _layoutManager = new LinearLayoutManager(this);
            _tutorialListView.SetLayoutManager(_layoutManager);
            // specify an adapter (see also next example)
            mAdapter = new TutorialListAdapter(this, _items);
            _tutorialListView.SetAdapter(mAdapter);
        }
    }

    public class TutorialItem
    {
        public int ImageResource { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Details { get; set; }
    }
}