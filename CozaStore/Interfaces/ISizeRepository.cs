using CozaStore.Models;
using CozaStore.Helpers;


namespace CozaStore.Interfaces
{
    public interface ISizeRepository
    {
        void AddSize(Size size);
        void ToggleDelete(Size size);
        void UpdateSize(Size size);
        Task<PagedList<Size>> GetAllSizesAsync(int pageNumber);
        Task<Size?> GetSizeAsync(int id);

        IEnumerable<Size> GetAllSizes();
    }
}
