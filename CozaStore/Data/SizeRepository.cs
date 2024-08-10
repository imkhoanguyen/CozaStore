using CozaStore.Models;
using CozaStore.Helpers;
using CozaStore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CozaStore.Data
{
    public class SizeRepository : ISizeRepository
    {
        private readonly DataContext _context;
        public SizeRepository(DataContext context)
        {
            _context = context;
        }
        public void AddSize(Size size)
        {
            _context.Sizes.Add(size);
        }

        public void ToggleDelete(Size size)
        {
            var sizeFromDb = _context.Sizes.FirstOrDefault(x => x.Id == size.Id);
            if (sizeFromDb != null)
            {
                if(sizeFromDb.IsDelete) size.IsDelete = false;
                else size.IsDelete = true;
            }
        }

        public async Task<PagedList<Size>> GetAllSizesAsync(int pageNumber = 1)
        {
            var query =  _context.Sizes.OrderByDescending(x => x.IsDelete == false).ThenByDescending(x => x.Id).AsQueryable();
            if (pageNumber < 1) pageNumber = 1;
            return await PagedList<Size>.CreateAsync(query, pageNumber, 10);
        }

        public async Task<Size?> GetSizeAsync(int id)
        {
            return await _context.Sizes.FindAsync(id);
        }

        public void UpdateSize(Size size)
        {
            var sizeFromDb = _context.Sizes.FirstOrDefault(x=>x.Id == size.Id);
            if (sizeFromDb != null)
            {
                sizeFromDb.Name = size.Name;
            }
        }

        public async Task<IEnumerable<Size>> GetAllSizesAsync()
        {
            return await _context.Sizes.ToListAsync();
        }
    }
}
