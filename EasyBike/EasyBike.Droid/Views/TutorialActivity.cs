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
					Title = Resources.GetString(Resource.String.tutorialTitle),
					Text = Resources.GetString(Resource.String.tutorialDownload1),
				ImageResource = Resource.Drawable.download1,
			});
			_items.Add(new TutorialItem()
			{
					Text = Resources.GetString(Resource.String.tutorialDownload2),
					Details = Resources.GetString(Resource.String.tutorialDownload2Details),
				ImageResource = Resource.Drawable.download2,
			});
			_items.Add(new TutorialItem()
			{
					Title = Resources.GetString(Resource.String.tutorialSwitch1),
					Text = Resources.GetString(Resource.String.tutorialSwitch1Text),
					Details = Resources.GetString(Resource.String.tutorialSwitch1Details),
					ImageResource = Resource.Drawable.walkmode,
			});
			_items.Add(new TutorialItem()
			{
					Text = Resources.GetString(Resource.String.tutorialSwitch2),
					ImageResource = Resource.Drawable.bikemode,
			});
            _items.Add(new TutorialItem()
            {
					Title = Resources.GetString(Resource.String.tutorialCompass1),
					Text = Resources.GetString(Resource.String.tutorialCompass1Text),
                ImageResource = Resource.Drawable.compassmode,
            });
            _items.Add(new TutorialItem()
            {
					Text = Resources.GetString(Resource.String.tutorialCompass2),
                ImageResource = Resource.Drawable.compassmode2,
            });
            _items.Add(new TutorialItem()
            {
					Title = Resources.GetString(Resource.String.tutorialSearch1),
					Text = Resources.GetString(Resource.String.tutorialSearch1Text),
                ImageResource = Resource.Drawable.search,
            });
            _items.Add(new TutorialItem()
            {
					Title = Resources.GetString(Resource.String.tutorialGetAddr1),
					Text = Resources.GetString(Resource.String.tutorialGetAddr1Text),
                ImageResource = Resource.Drawable.getaddress,
            });

            _items.Add(new TutorialItem()
            {
					Title = Resources.GetString(Resource.String.tutorialAddFavorite1),
					Text = Resources.GetString(Resource.String.tutorialAddFavorite1Text),
					Details = Resources.GetString(Resource.String.tutorialAddFavorite1Details),
                ImageResource = Resource.Drawable.addFavorite,
            });
            _items.Add(new TutorialItem()
            {
					Title = Resources.GetString(Resource.String.tutorialManageFavorite1),
					Text = Resources.GetString(Resource.String.tutorialManageFavorite1Text),
                ImageResource = Resource.Drawable.favorites,
            });
            _items.Add(new TutorialItem()
            {
					Title = Resources.GetString(Resource.String.tutorialDirection1),
					Text = Resources.GetString(Resource.String.tutorialDirection1Text),
					Details = Resources.GetString(Resource.String.tutorialDirection1Details),
                ImageResource = Resource.Drawable.getDirections,
            });
            
            _items.Add(new TutorialItem()
            {
					Title = Resources.GetString(Resource.String.tutorialShare1),
					Text = Resources.GetString(Resource.String.tutorialShare1Text),
                ImageResource = Resource.Drawable.shareLocation,
            });
            _items.Add(new TutorialItem()
            {
					Text = Resources.GetString(Resource.String.tutorialShare2),
                ImageResource = Resource.Drawable.sharemessage,
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