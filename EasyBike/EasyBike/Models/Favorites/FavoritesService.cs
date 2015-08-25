using EasyBike.Models.Storage;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyBike.Models.Favorites
{
    public class FavoritesService : IFavoritesService
    {
        private readonly IStorageService _storageService;

        public FavoritesService(IStorageService storageService)
        {
            _storageService = storageService;
        }

        public async Task AddFavoriteAsync(Favorite favorite)
        {
            await _storageService.StoreAsync(favorite.ToString(), favorite).ConfigureAwait(false);
        }

        public async Task RemoveFavoriteAsync(Favorite favorite)
        {
            await _storageService.RemoveAsync(favorite.ToString()).ConfigureAwait(false);
        }

        public async Task<List<Favorite>> GetFavoritesAsync()
        {
            return (await _storageService.GetAsync<Favorite>()).ToList();
        }
    }
}
