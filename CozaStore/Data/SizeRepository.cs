using CozaStore.Models;
using CozaStore.Helpers;
using CozaStore.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

        public void Delete(Size size)
        {
            var sizeFromDb = _context.Sizes.FirstOrDefault(x => x.Id == size.Id);
            if (sizeFromDb != null)
            {
                sizeFromDb.IsDelete = true;
            }
        }

        public async Task<PagedList<Size>> GetAllSizesAsync(SizeParams sizeParams)
        {
            var query = _context.Sizes.Where(x => !x.IsDelete).AsQueryable();

            if (!sizeParams.SearchString.IsNullOrEmpty())
            {
                query = query.Where(x => x.Id == int.Parse(sizeParams.SearchString) ||
                x.Name.ToLower().Contains(sizeParams.SearchString.ToLower()));
            }

            return await PagedList<Size>.CreateAsync(query, sizeParams.PageNumber, sizeParams.PageSize);
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
            return await _context.Sizes.Where(x=>!x.IsDelete).ToListAsync();
        }
    }
}
