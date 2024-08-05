using CozaStore.Entities;

namespace CozaStore.Interfaces
{
    public interface IVariantRepository
    {
        Task<Variant?> GetMainVariant(int productId);
    }
}
