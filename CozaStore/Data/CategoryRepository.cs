using CozaStore.Entities;
using CozaStore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CozaStore.Data
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;
        public CategoryRepository(DataContext context)
        {
            _context = context;
        }
        public void AddCategory(Category category)
        {
            _context.Categories.Add(category);
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        public Task<Category> GetCategory(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateCategory(Category category)
        {
            throw new NotImplementedException();
        }
    }
}
