using CozaStore.Models;
using CozaStore.Helpers;


namespace CozaStore.Interfaces
{
    public interface ISizeRepository
    {
        void AddSize(Size size);
        void Delete(Size size);
        void UpdateSize(Size size);
        Task<PagedList<Size>> GetAllSizesAsync(SizeParams sizeParams);
        Task<Size?> GetSizeAsync(int id);

        Task<IEnumerable<Size>> GetAllSizesAsync();
    }
}
