using CozaStore.Entities;

namespace CozaStore.Interfaces
{
    public interface ICategoryRepository
    {
        void AddCategory(Category category);
        void UpdateCategory(Category category);
        Task<Category> GetCategory(int id);
        Task<IEnumerable<Category>> GetAllCategories();

    }
}
