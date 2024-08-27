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

        public async Task<PagedList<Color>> GetAllColorsAsync(ColorParams colorParams)
        {
            var query = _context.Colors.Where(x=>!x.IsDelete).AsQueryable();
            if (colorParams.SearchString != null)
            {
                query = query.Where(x => x.Name.ToLower().Contains(colorParams.SearchString.ToLower())
                || x.Id.ToString() == colorParams.SearchString);
            }
           
            return await PagedList<Color>.CreateAsync(query, colorParams.PageNumber, colorParams.PageSize);
        }

        public async Task<Color?> GetColorAsync(int id)
        {
            return await _context.Colors.FindAsync(id);
        }

        public void Delete(Color color)
        {
            var colorFromDb = _context.Colors.FirstOrDefault(x => x.Id == color.Id);
            if (colorFromDb != null)
            {
                colorFromDb.IsDelete = true;
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
