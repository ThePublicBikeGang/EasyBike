using GalaSoft.MvvmLight.Ioc;
using EasyBike.Config;
using EasyBike.WinPhone.Common;
using System;
using Windows.ApplicationModel.Email;
using Windows.ApplicationModel.Store;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace EasyBike.WinPhone.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AboutPage : Page
    {
        private NavigationHelper navigationHelper;
        private readonly IConfigService _configService;

        public AboutPage()
        {
            this.InitializeComponent();

            _configService = SimpleIoc.Default.GetInstance<IConfigService>();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        /// <summary>
        /// Obtient le <see cref="NavigationHelper"/> associé à ce <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Remplit la page à l'aide du contenu passé lors de la navigation. Tout état enregistré est également
        /// fourni lorsqu'une page est recréée à partir d'une session antérieure.
        /// </summary>
        /// <param name="sender">
        /// La source de l'événement ; en général <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Données d'événement qui fournissent le paramètre de navigation transmis à
        /// <see cref="Frame.Navigate(Type, Object)"/> lors de la requête initiale de cette page et
        /// un dictionnaire d'état conservé par cette page durant une session
        /// antérieure.  L'état n'aura pas la valeur Null lors de la première visite de la page.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Conserve l'état associé à cette page en cas de suspension de l'application ou de
        /// suppression de la page du cache de navigation.  Les valeurs doivent être conformes aux
        /// exigences en matière de sérialisation de <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">La source de l'événement ; en général <see cref="NavigationHelper"/></param>
        /// <param name="e">Données d'événement qui fournissent un dictionnaire vide à remplir à l'aide de l'
        /// état sérialisable.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region Inscription de NavigationHelper

        /// <summary>
        /// Les méthodes fournies dans cette section sont utilisées simplement pour permettre
        /// NavigationHelper pour répondre aux méthodes de navigation de la page.
        /// <para>
        /// La logique spécifique à la page doit être placée dans les gestionnaires d'événements pour  
        /// <see cref="NavigationHelper.LoadState"/>
        /// et <see cref="NavigationHelper.SaveState"/>.
        /// Le paramètre de navigation est disponible dans la méthode LoadState 
        /// en plus de l'état de page conservé durant une session antérieure.
        /// </para>
        /// </summary>
        /// <param name="e">Fournit des données pour les méthodes de navigation et
        /// les gestionnaires d'événements qui ne peuvent pas annuler la requête de navigation.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
            DataContext = e.Parameter;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }


        #endregion
        private async void mailButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            EmailRecipient sendTo = new EmailRecipient()
            {
                Address = (await _configService.GetConfigAsync()).SupportEmail
            };

            // string version = XDocument.Load("ms-appx:///WMAppManifest.xml").Root.Element("App").Attribute("Version").Value;

            EmailMessage mail = new EmailMessage();
            mail.Subject = (await _configService.GetConfigAsync()).ApplicationName; //+ version;
            mail.To.Add(sendTo);
            await EmailManager.ShowComposeNewEmailAsync(mail);
        }

        private async void rateButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // await Windows.System.Launcher.LaunchUriAsync(new Uri(@"zune:reviewapp?appid=app" + CurrentApp.AppId));
            await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=" + CurrentApp.AppId));
        }
    }
}

