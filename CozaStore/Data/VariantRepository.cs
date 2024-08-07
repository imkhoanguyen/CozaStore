using CozaStore.Models;
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

        public async Task<Variant?> GetVariantAsync(int id)
        {
            return await _context.Variants.FindAsync(id);
        }

        public void ToggleStatusVariant(Variant variant)
        {
            var variantFromDb = _context.Variants.FirstOrDefault(x => x.Id == variant.Id);
            if (variantFromDb != null)
            {
                if(variantFromDb.Status == (int)VariantStatus.Deleted) variantFromDb.Status = (int)VariantStatus.Private;
                else if(variantFromDb.Status == (int)VariantStatus.Public
                  || variantFromDb.Status == (int)VariantStatus.Private) variantFromDb.Status = (int)VariantStatus.Deleted;

            }
        }

        public void UpdateVariant(Variant variant)
        {


            var variantFromDb = _context.Variants.FirstOrDefault(x=>x.Id == variant.Id);

            if (variantFromDb != null)
            {
                variantFromDb.Quantity = variant.Quantity;
                variantFromDb.SizeId = variant.SizeId;
                variantFromDb.ColorId = variant.ColorId;
                variantFromDb.Status = variant.Status;
                variantFromDb.PriceSell = variant.PriceSell;
                variantFromDb.PriceImport = variant.PriceImport;
            }
        }
    }
}
