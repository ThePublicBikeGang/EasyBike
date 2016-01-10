using Android.App;
using Android.OS;
using EasyBike.Models;
using System.Collections.Generic;
using Android.Widget;
using EasyBike.Droid.Helpers;
using Android.Views;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using System;
using GalaSoft.MvvmLight.Ioc;

namespace EasyBike.Droid.Views
{
    [Activity(Label = "ContractsActivity")]
    public partial class ContractsActivity
    {
        private List<Country> countries;

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Contracts);

            // toolbar setup
            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            toolbar.SetOnMenuItemClickListener(new MenuItemClickListener(this));
            //toolbar.InflateMenu(Resource.Menu.contractPageMenu);
            //toolbar.ShowOverflowMenu();
            //await Task.Delay(30);
            //var t = Task.Run(async () =>
            //{
            //    vm = SimpleIoc.Default.GetInstanceWithoutCaching<ContractsViewModel>();
            //   // countries = await vm.GetCountries().ConfigureAwait(false);
            //};
            countries = await ViewModel.GetCountries();

            ContractsList.SetAdapter(new CountryListAdapter(this, countries));
            ContractsList.ChildClick += ContractsList_ChildClick;

            // ContractsList.Adapter =  countries.GetAdapter(GetContractAdapter);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.contractPageMenu, menu);
            return true;
        }

        private async void ContractsList_ChildClick(object sender, ExpandableListView.ChildClickEventArgs e)
        {
            var contract = countries[e.GroupPosition].Contracts[e.ChildPosition];

            var progressBar = e.ClickedView.FindViewById<ProgressBar>(Resource.Id.ProgressBar);
            // Show the progress bar
            if (!contract.Downloaded)
            {
                progressBar.Visibility = ViewStates.Visible;
            }
            ///ViewModel.ContractTappedCommand.Execute(contract);
            await ViewModel.AddOrRemoveContract(contract);
            //(ContractsList.Adapter as CountryListAdapter).GetChildView(e.GroupPosition)
            //_countries[groupPosition].Contracts[childPosition].

            progressBar.Visibility = ViewStates.Invisible;
            var checkBox = e.ClickedView.FindViewById<CheckBox>(Resource.Id.ContractCheckBox);
            checkBox.Checked = contract.Downloaded;
            //e.ClickedView.SetBackgroundColor(Color.ParseColor("#676767"));
            //var checkBox = e.ClickedView.FindViewById<CheckBox>(Resource.Id.ContractCheckBox);

            //checkBox.SetBinding(
            //     () => contract.Downloaded,
            //     () => checkBox.Checked);

        }


    }


    public class MenuItemClickListener : Java.Lang.Object, Toolbar.IOnMenuItemClickListener
    {
        ContractsActivity _context;

        public MenuItemClickListener(ContractsActivity context)
        {
            _context = context;
        }

        public bool OnMenuItemClick(IMenuItem item)
        {
            _context.ViewModel.ClearAllDownloadedContractsCommand.Execute(null);
            return true;
        }
    }
}