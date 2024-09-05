using CozaStore.Data;
using CozaStore.DTOs;
using CozaStore.Models;
using CozaStore.ValidationAttr;
using System.ComponentModel.DataAnnotations.Schema;

namespace CozaStore.ViewModels
{
    public class ProductCreateVM
    {
        public Product? Product { get; set; }
        public List<int> SelectedCategory { get; set; } = [];
        public IEnumerable<Category> CategoryList { get; set; } = [];
        [AllowedExtensionsList(new string[] { ".jpg", ".jpeg" } )]
        public List<IFormFile> ImagesFile { get; set; } = [];
        public IEnumerable<StatusDto> StatusList { get; set; } = SD.GetProductStatusList();
 
    }
}
