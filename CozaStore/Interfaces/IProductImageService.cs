using CloudinaryDotNet.Actions;

namespace CozaStore.Interfaces
{
    public interface IProductImageService
    {
        Task<ImageUploadResult> AddImageAsync(IFormFile file);
        Task<DeletionResult> DeleteImageAsync(string publicId);

    }
}
