using CozaStore.Entities;
using CozaStore.Helpers;
using CozaStore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CozaStore.Data
{
    public class SubCategoryRepository : ISubCategoryRepository
    {
        private readonly DataContext _context;
        public SubCategoryRepository(DataContext context)
        {
            _context = context;
        }
        public void AddSubCategory(SubCategory subCategory)
        {
            _context.SubCategories.Add(subCategory);
        }

        public async Task<PagedList<SubCategory>> GetAllSubCategoriesAsync(int categoryId, int pageNumber = 1)
        {
            var query =  _context.SubCategories.AsQueryable().Where(x=>x.CategoryId == categoryId);
            if (pageNumber < 1) pageNumber = 1;
            return await PagedList<SubCategory>.CreateAsync(query, pageNumber, 5);
        }

        public IEnumerable<SubCategory> GetAllSubCategories()
        {
            return _context.SubCategories.ToList();
        }

        public async Task<SubCategory?> GetSubCategoryAsync(int subCategoryId)
        {
            return await _context.SubCategories.FindAsync(subCategoryId);
        }

        public void RemoveSubCategory(SubCategory subCategory)
        {
            _context.SubCategories.Remove(subCategory);
        }

        public void UpdateSubCategory(SubCategory subCategory)
        {
            _context.SubCategories.Update(subCategory);
        }
    }
}
