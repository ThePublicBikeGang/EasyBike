using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using PublicBikes.Models.Favorites;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PublicBikes.ViewModels
{
    public class FavoritesViewModel : ViewModelBase
    {
        private readonly IFavoritesService _favoritesService;
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;

        private List<Favorite> _selectedFavorites = new List<Favorite>();
        private ObservableCollection<Favorite> _favorites = new ObservableCollection<Favorite>();
        public ObservableCollection<Favorite> Favorites
        {
            get { return _favorites; }
        }

        public FavoritesViewModel(IFavoritesService favoritesService, IDialogService dialogService, INavigationService navigationService)
        {
            _favoritesService = favoritesService;
            _dialogService = dialogService;
            _navigationService = navigationService;
            Init();
        }

        public async void Init()
        {
            foreach(var favorite in await _favoritesService.GetFavoritesAsync())
            {
                Favorites.Add(favorite);
            }
            if(Favorites.Count == 0)
            {
                VisualState = "LonelyHere";
            }
        }
        public async void AddFavorite(Favorite favorite)
        {
            await _favoritesService.AddFavoriteAsync(favorite);
        }

        private string _visualState;
        public string VisualState
        {
            get { return _visualState; }
            set
            {
                Set(() => VisualState, ref _visualState, value);
            }
        }


        private RelayCommand<Favorite> _selectionChangedCommand;
        public RelayCommand<Favorite> SelectionChangedCommand
        {
            get
            {
                return _selectionChangedCommand
                       ?? (_selectionChangedCommand = new RelayCommand<Favorite>(
                           (favorite) =>
                           {
                               if (favorite != null)
                               {
                                   if (_selectedFavorites.Contains(favorite))
                                   {
                                       _selectedFavorites.Remove(favorite);
                                   }
                                   else
                                   {
                                       _selectedFavorites.Add(favorite);
                                   }

                                   if(_selectedFavorites.Count == 0)
                                   {
                                       VisualState = "DeleteCollapsed";
                                   }
                                   else
                                   {
                                       VisualState = "DeleteVisible";
                                   }
                                   
                               }
                           }
                           ));
            }
        }
        private RelayCommand<Favorite> _navigateToFavoriteCommand;
        public RelayCommand<Favorite> NavigateToFavoriteCommand
        {
            get
            {
                return _navigateToFavoriteCommand
                       ?? (_navigateToFavoriteCommand = new RelayCommand<Favorite>(
                           (favorite) =>
                           {
                               if (favorite != null)
                               {
                                   _navigationService.GoBack();
                                   ServiceLocator.Current.GetInstance<MainViewModel>().GoToFavorite(favorite);
                               }
                           }
                           ));
            }
        }

        private RelayCommand _deleteFavoritesCommand;
        public RelayCommand  DeleteFavoritesCommand
        {
            get
            {
                return _deleteFavoritesCommand
                       ?? (_deleteFavoritesCommand = new RelayCommand(
                           async () =>
                           {
                               foreach(var favorite in _selectedFavorites.ToList())
                               {
                                   await _favoritesService.RemoveFavoriteAsync(favorite);
                                   Favorites.Remove(favorite);
                               }
                           }
                           ));
            }
        }

    }
}

