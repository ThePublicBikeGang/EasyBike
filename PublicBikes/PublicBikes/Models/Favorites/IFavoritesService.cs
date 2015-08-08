using System.Collections.Generic;
using System.Threading.Tasks;

namespace PublicBikes.Models.Favorites
{
    public interface IFavoritesService
    {
        Task AddFavoriteAsync(Favorite favorite);
        Task RemoveFavoriteAsync(Favorite favorite);
        Task<List<Favorite>> GetFavoritesAsync();
    }
}
