using CozaStore.Models;

namespace CozaStore.Interfaces
{
    public interface IVariantRepository
    {
        void UpdateVariant(Variant variant);
        Task<Variant?> GetVariantAsync(int id);

        void DeleteVariant(Variant variant);
        void UpdateQuantity(int variantId, int quantity);
    }
}
