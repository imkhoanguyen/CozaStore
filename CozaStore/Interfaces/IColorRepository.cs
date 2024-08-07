using CozaStore.Models;
using CozaStore.Helpers;

namespace CozaStore.Interfaces
{
    public interface IColorRepository
    {
        void AddColor(Color color);
        void ToggleDelete(Color color);
        void UpdateColor(Color color);
        Task<Color?> GetColorAsync(int id);
        Task<PagedList<Color>> GetAllColorsAsync(string searchString,int page);

        IEnumerable<Color> GetAllColors();

    }
}
