using Android.App;
using Android.OS;
using EasyBike.ViewModels;
using System.Threading.Tasks;
using EasyBike.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Android.Views;
using GalaSoft.MvvmLight.Helpers;
using Android.Widget;

namespace EasyBike.Droid.Views
{
    [Activity(Label = "ContractsActivity")]
    public partial class ContractsActivity 
    {
        public MainViewModel Vm
        {
            get
            {
                return App.Locator.Main;
            }
        }
        private List<Country> countries;

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            ContractsViewModel vm = null;
            await Task.Delay(30);
            //var t = Task.Run(async () =>
            //{
            //    vm = SimpleIoc.Default.GetInstanceWithoutCaching<ContractsViewModel>();
            //   // countries = await vm.GetCountries().ConfigureAwait(false);
            //};
            countries = await vm.GetCountries();

            foreach (var country in countries)
            {
              
            }
            
           // ContractsList.Adapter =  countries.GetAdapter(GetContractAdapter);
        }

        //private View GetContractAdapter(int position, Contract contract, View convertView, View view2)
        //{
        //    // Not reusing views here
        //    convertView = LayoutInflater.Inflate(Resource.Layout.FlowerTemplate, null);

        //    var title = convertView.FindViewById<TextView>(Resource.Id.NameTextView);
        //    title.Text = contract.Name;

        //    //var image = convertView.FindViewById<ImageView>(Resource.Id.FlowerImageView);
        //   // ImageDownloader.AssignImageAsync(image, flower.Model.Image, this);

        //    return convertView;
        //}
    }
}