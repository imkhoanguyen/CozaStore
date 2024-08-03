using CozaStore.Entities;
using CozaStore.Helpers;

namespace CozaStore.Interfaces
{
    public interface ISubCategoryRepository
    {
        void AddSubCategory(SubCategory subCategory);
        void RemoveSubCategory(SubCategory subCategory);
        void UpdateSubCategory(SubCategory subCategory);
        Task<SubCategory?> GetSubCategoryAsync(int subCategoryId);

        Task<PagedList<SubCategory>> GetAllSubCategoriesAsync(int categoryId, int pageNumber = 1);
    }
}
