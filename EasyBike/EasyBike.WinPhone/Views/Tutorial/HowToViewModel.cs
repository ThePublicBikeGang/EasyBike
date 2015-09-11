using System.Collections.ObjectModel;

namespace EasyBike.WinPhone.Views.Tutorial
{
    public class HowToViewModel
    {
        public ObservableCollection<HowToModel> Items = new ObservableCollection<HowToModel>();

        public HowToViewModel()
        {
            Items.Add(new HowToModel()
            {
                Title = "Download a city",
                Text ="Tap on \"download cities\" on the menu panel",
                ImageUrl = "downloadcities.png",
            });
            Items.Add(new HowToModel()
            {
                Title = "",
                Text = "It will show the available cities list. Then tap on a city to download or remove it.",
                Details = "It will download the stations list from the selected service." +
                " If there is some new stations provided by the public bike service, you can refresh that list by removing and downloading again the city."+
                " This list will get bigger on future updates",
                ImageUrl = "downloadcities2.png",
            });
            Items.Add(new HowToModel()
            {
                Title = "Switch between bike and station mode",
                Text = "Tap on the bike icon or (P) icon on the bottom right corner.",
                ImageUrl = "cyclemode.png",
            });
            Items.Add(new HowToModel()
            {
                Title = "Use the compass",
                Text = "Tap once on the position button in the command bar, it will focus on your location."+
                " Tap again on the button to activate the compass.",

                Details = "If you tapped once on the location button, it will change the location button for the compass button."+
                "Dragging the map when you are in location or compass mode will deactivate the location or compass mode."+
                "To reset the north of the map just click on the top right corner button that appear if the north of the map have changed.",
                ImageUrl = "compass.png",
            });
            Items.Add(new HowToModel()
            {
                Title = "Get directions to go somewhere",
                Text = "Tap on a station or a searched location, it will show up a path walk to get there." +
                            " Hold on a station or on a searched location to show up a command panel. Press \"Drive to\"."+
                            " It will start your favorite driving app like HERE drive.",
                Details = "You can then get your" +
                            " headphones on, lock your phone, put it in your pocket and listen to the directions"+
                            " from the HERE drive app :) (take care, it's usally a car itinerary so you might need to tweak it a bit)",
                ImageUrl = "shareLocation1.jpg",
            });
            Items.Add(new HowToModel()
            {
                Title = "Add some favorites",
                Text = "Hold on a station or on a searched location to show up a command panel, then press \"Add to favorites\".",
                Details = "It will store the location of the point. You can name it as you like and manage" +
                            " favorits from the dedicated page.",
                ImageUrl = "shareLocation1.jpg",
            });
            Items.Add(new HowToModel()
            {
                Title = "Get the address from a location",
                Text = "Hold on the map. It will shows up the found address and the walk path to get there if it exist.",
                Details = "",
                ImageUrl = "mapHolding.png",
            });
            Items.Add(new HowToModel()
            {
                Title = "Search for an address",
                Text = "Tap the search icon button in the command bar and type an address in the search text box.",
                Details = "",
                ImageUrl = "addressSearch.png",
            });

            Items.Add(new HowToModel()
            {
                Title = "Share a location",
                Text = "By tapping on a bike station or on a searched place, you can share that location by mail or text.",
                ImageUrl = "shareLocation1.jpg",
            });
            Items.Add(new HowToModel()
            {
                ImageUrl = "shareLocation2.jpg",
            });
            Items.Add(new HowToModel()
            {
                Text = "The message will automatically contains the address if it has been resolved."+
                " It will also contains direct link to the EasyBike app, IPhone/IPad native map and Google map.",
                ImageUrl = "shareLocation3.jpg",
            });

            Items.Add(new HowToModel()
            {
                Title = "Tips : Store your home/car location",
                Text = "Like a \"Dude, where is my car\" app, if you want to easely get back to your home / car / whereever location, just hold a finger on the"+
                " location and add it to your favorites.",
                Details = "It's then really easy to get back somewhere.",
                ImageUrl = "whereIsMyCar.jpg",
            });
        }
    }
}
