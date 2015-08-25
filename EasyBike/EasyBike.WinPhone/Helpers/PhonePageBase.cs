using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace EasyBike.WinPhone.Helpers
{
    public class PhonePageBase : Page
    {
        public IDialogService Dialog
        {
            get
            {
                return ServiceLocator.Current.GetInstance<IDialogService>();
            }
        }

        public NavigationService GlobalNavigation
        {
            get
            {
                return (NavigationService)ServiceLocator.Current.GetInstance<INavigationService>();
            }
        }

        public void UpdateCurrentBinding()
        {
            var currentTextBox = FocusManager.GetFocusedElement() as TextBox;

            if (currentTextBox != null)
            {
                var currentBinding = currentTextBox.GetBindingExpression(TextBox.TextProperty);
                if (currentBinding != null)
                {
                    currentBinding.UpdateSource();
                }
            }
        }
    }
}