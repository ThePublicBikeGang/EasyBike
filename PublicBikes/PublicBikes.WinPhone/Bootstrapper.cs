using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using PublicBikes.ViewModels;
using PublicBikes.WinPhone.Views;
using PublicBikes.WinPhone.Views.Cities;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace PublicBikes.WinPhone
{
    internal class Bootstrapper
    {
       public static void Start()
        {
            var nav = new WPNavigationService();
            nav.Configure(ViewModelLocator.ContractsPageKey, typeof(ContractsPage));
            nav.Configure(ViewModelLocator.SettingsPageKey, typeof(SettingsPage));

            SimpleIoc.Default.Register<INavigationService>(() => nav);
            SimpleIoc.Default.Register<IDialogService, DialogService>();
        }
    }

    public class WPNavigationService : INavigationService
    {
        public Dictionary<string, Type> Pages = new Dictionary<string, Type>();
        public void Configure(string key, Type value)
        {
            Pages.Add(key, value);
        }

        public string CurrentPageKey
        {
            get
            {
                return "";
            }
        }
     
        public void GoBack()
        {
            ((Frame)Window.Current.Content).GoBack();
        }

        public void NavigateTo(string pageKey)
        {
            ((Frame)Window.Current.Content).Navigate(Pages[pageKey]);
        }

        public void NavigateTo(string pageKey, object parameter)
        {
            ((Frame)Window.Current.Content).Navigate(Pages[pageKey], parameter);
        }
    }
}
