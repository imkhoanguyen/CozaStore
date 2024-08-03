using CozaStore.Entities;
using CozaStore.Helpers;
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

        public void DeleteCategory(Category category)
        {
            _context.Categories.Remove(category);
        }

        public async Task<PagedList<Category>> GetAllCategoriesAsync(string searchString, int pageNumber = 1)
        {
            var query = _context.Categories.AsQueryable();

            if (searchString != null)
            {
                query = query.Include(x => x.SubCategories).Where(x => x.Name.ToLower().Contains(searchString.ToLower())
                || x.Id.ToString() == searchString);

                  
            }

            if (pageNumber < 1) pageNumber = 1;

            return await PagedList<Category>.CreateAsync(query.AsNoTracking(), pageNumber, 10);
        }

        public async Task<Category?> GetCategoryAsync(int id)
        {
            return await _context.Categories.Include(x => x.SubCategories).FirstOrDefaultAsync(x => x.Id == id);
        }

        public void UpdateCategory(Category category)
        {
            _context.Categories.Update(category);
        }
    }
}
