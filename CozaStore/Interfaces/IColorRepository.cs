using CozaStore.Models;
using CozaStore.Helpers;

namespace CozaStore.Interfaces
{
    public interface IColorRepository
    {
        void AddColor(Color color);
        void Delete(Color color);
        void UpdateColor(Color color);
        Task<Color?> GetColorAsync(int id);
        Task<PagedList<Color>> GetAllColorsAsync(ColorParams colorParams);

        Task<IEnumerable<Color>> GetAllColorsAsync();

    }
}
