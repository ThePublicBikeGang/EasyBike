using EasyBike.Models;
using EasyBike.ViewModels;
using EasyBike.WinPhone.Common;
using System.Collections.ObjectModel;
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Phone.UI.Input;
using System.Collections.Generic;
using GalaSoft.MvvmLight.Ioc;

namespace EasyBike.WinPhone.Views.Cities
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ContractsPage : Page
    {
        private NavigationHelper navigationHelper;
        private CollectionViewSource contractCollectionViewSource;
        private TaskCompletionSource<bool> loadedTsc = new TaskCompletionSource<bool>();
        private List<Country> countries;
        public ContractsPage()
        {
            InitializeComponent();

            navigationHelper = new NavigationHelper(this);
            navigationHelper.LoadState += this.NavigationHelper_LoadState;
            navigationHelper.SaveState += this.NavigationHelper_SaveState;
            Loaded += ContractsPage_Loaded;

            Initialize();
        }

        private void ContractsPage_Loaded(object sender, RoutedEventArgs e)
        {
            loadedTsc.SetResult(true);
        }

        private async void Initialize()
        {
            var vm = SimpleIoc.Default.GetInstanceWithoutCaching<ContractsViewModel>();
            var t = Task.Run(async () =>
            {
                countries = await vm.GetCountries().ConfigureAwait(false);
            });
            await Task.WhenAll(t, loadedTsc.Task);
            DataContext = vm;
            contractCollectionViewSource = new CollectionViewSource
            {
                IsSourceGrouped = true,
                Source = countries,
                ItemsPath = new PropertyPath("Contracts")
            };

            ContractListView.ItemsSource = contractCollectionViewSource.View;
            ContractsListViewZoomOut.ItemsSource = contractCollectionViewSource.View.CollectionGroups;

            foreach (var country in countries)
            {
                if (country.ImageByteArray != null)
                {
                    var bitmap = await ByteArrayToImageAsync(country.ImageByteArray as byte[]);
                    bitmap.DecodePixelWidth = 60;
                    country.ImageSource = bitmap;
                }
                await Task.Delay(1);
            }
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        private ObservableCollection<ContractGroup> _contracts;
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
            if (!GalaSoft.MvvmLight.ViewModelBase.IsInDesignModeStatic)
            {
                HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            }

        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            var viewModel = (DataContext as ContractsViewModel);
            if (viewModel != null)
            {
                viewModel.StopLoadingContractsCommand.Execute(this);
            }
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
            //_contracts.CollectionChanged -= Contracts_CollectionChanged;
            //  Try to prevent a "value does not fall into the valid within the expected exception" but this is not working
            //_contracts.Clear();
            //DataContext = null;
            //contractCollectionViewSource.Source = null;
            //ContractListView.ItemsSource = null;
            //ContractsListViewZoomOut.ItemsSource = null;
        }

        public async Task<BitmapImage> ByteArrayToImageAsync(byte[] pixeByte)
        {
            using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
            {
                BitmapImage image = new BitmapImage();
                await stream.WriteAsync(pixeByte.AsBuffer());
                stream.Seek(0);
                await image.SetSourceAsync(stream);
                return image;
            }
        }

        //private async void Contracts_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        //{
        //    //try
        //    //{
        //    //    var c = e.NewItems[0] as ContractGroup;
        //    //    if (c != null)
        //    //    {
        //    //        if (c.ImageByteArray != null)
        //    //        {
        //    //            var bitmap = await ByteArrayToImageAsync(c.ImageByteArray as byte[]);
        //    //            bitmap.DecodePixelWidth = 60;
        //    //            c.ImageSource = bitmap;

        //    //            //using (var ms = new MemoryStream((c.ImageSource as byte[])))
        //    //            //{
        //    //            //    bitmap.SetSourceAsync(WindowsRuntimeStreamExtensions.AsRandomAccessStream(ms));
        //    //            //}
        //    //            //c.ImageSource = bitmap;
        //    //        }
        //    //    }
        //    //}
        //    //catch { }
        //}

        protected async override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}
