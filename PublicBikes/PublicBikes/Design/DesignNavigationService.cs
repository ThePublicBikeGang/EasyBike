namespace PublicBikes.Design
{
    /// <summary>
    /// This class is only here to avoid errors at design time.
    /// </summary>
    public class DesignNavigationService : GalaSoft.MvvmLight.Views.INavigationService
    {
        public void GoBack()
        {
            // Do nothing
        }

        public void NavigateTo(string pageKey)
        {
            // Do nothing
        }

        public void NavigateTo(string pageKey, object parameter)
        {
            // Do nothing
        }

        public string CurrentPageKey
        {
            get
            {
                return null;
            }
        }
    }
}
