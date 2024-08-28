using CozaStore.Models;
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
            var categoryFromDb = _context.Categories.FirstOrDefault(c => c.Id == category.Id);
            if(categoryFromDb != null)
            {
                categoryFromDb.IsDelete = true;
            }
        }

        public async Task<PagedList<Category>> GetAllCategoriesAsync(CategoryParams categoryParams)
        {
            var query = _context.Categories.Where(x=>!x.IsDelete).OrderByDescending(x=>x.Id).AsQueryable();

            if (categoryParams.SearchString != null)
            {
                query = query.Where(x => x.Name.ToLower().Contains(categoryParams.SearchString.ToLower())
                        || x.Id.ToString() == categoryParams.SearchString);
            }
            return await PagedList<Category>.CreateAsync(query, categoryParams.PageNumber, categoryParams.PageSize);
        }


        public async Task<Category?> GetCategoryAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public void UpdateCategory(Category category)
        {
            _context.Categories.Update(category);
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
           return await _context.Categories.Where(x=>!x.IsDelete).ToListAsync();
        }
    }
}
