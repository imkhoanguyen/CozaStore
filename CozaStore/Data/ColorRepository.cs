using CozaStore.Models;
using CozaStore.Helpers;
using CozaStore.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CozaStore.Data
{
    public class ColorRepository : IColorRepository
    {
        private readonly DataContext _context;
        public ColorRepository(DataContext context)
        {
            _context = context;
        }
        public void AddColor(Color color)
        {
            _context.Colors.Add(color);
        }

        public async Task<IEnumerable<Color>> GetAllColorsAsync()
        {
            return await _context.Colors.ToListAsync();
        }

        public Task<PagedList<Color>> GetAllColorsAsync(string searchString, int page = 1)
        {
            var query = _context.Colors.OrderByDescending(x => x.IsDelete == false).ThenByDescending(x => x.Id).AsQueryable();
            if (searchString != null)
            {
                query = query.Where(x => x.Name.ToLower().Contains(searchString.ToLower())
                || x.Id.ToString() == searchString);
            }
            if (page < 1) page = 1;
            return PagedList<Color>.CreateAsync(query, page, 10);
        }

        public async Task<Color?> GetColorAsync(int id)
        {
            return await _context.Colors.FindAsync(id);
        }

        public void ToggleDelete(Color color)
        {
            var colorFromDb = _context.Colors.FirstOrDefault(x => x.Id == color.Id);
            if (colorFromDb != null)
            {
                if(colorFromDb.IsDelete) colorFromDb.IsDelete = false;
                else colorFromDb.IsDelete = true;
            }
        }

        public void UpdateColor(Color color)
        {
            var colorFromDb = _context.Colors.FirstOrDefault(x => x.Id == color.Id);
            if(colorFromDb != null)
            {
                colorFromDb.Name = color.Name;
            }
        }
    }
}
