using CozaStore.Models;
using CozaStore.Interfaces;

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

        public void DeleteVariant(Variant variant)
        {
           _context.Variants.Remove(variant);
        }

        public void UpdateVariant(Variant variant)
        {
           _context.Variants.Update(variant);
        }

        public void UpdateQuantity(int variantId, int quantity)
        {
            var variantFromdb = _context.Variants.FirstOrDefault(x => x.Id == variantId);
            if (variantFromdb != null)
            {
                variantFromdb.Quantity -= quantity;
            }
        }
    }
}
