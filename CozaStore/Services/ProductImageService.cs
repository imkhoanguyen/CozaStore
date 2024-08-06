using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CozaStore.Helpers;
using CozaStore.Interfaces;
using Microsoft.Extensions.Options;

namespace CozaStore.Services
{
    public class ProductImageService : IProductImageService
    {
        private readonly Cloudinary _cloudinary;
        public ProductImageService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);
            _cloudinary = new Cloudinary(acc);
        }
        public async Task<ImageUploadResult> AddImageAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Width(600).Height(900).Crop("fit"), // options: "fill", "limit", "scale"
                    Folder = "cozastorage-net8"
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }
            return uploadResult;
        }

        public async Task<DeletionResult> DeleteImageAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            return await _cloudinary.DestroyAsync(deleteParams);
        }
    }
}
