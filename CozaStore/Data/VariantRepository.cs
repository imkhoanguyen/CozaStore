using CozaStore.Entities;
using CozaStore.Helpers;
using CozaStore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CozaStore.Data
{
    public class VariantRepository : IVariantRepository
    {
        private readonly DataContext _context;
        public VariantRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<Variant?> GetMainVariant(int productId)
        {
            return await _context.Variants.FirstOrDefaultAsync(x => x.ProductId == productId
            && x.Status == (int)VariantStatus.Main);
        }
    }
}
