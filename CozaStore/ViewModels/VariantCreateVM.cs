using CozaStore.Data;
using CozaStore.DTOs;
using CozaStore.Models;

namespace CozaStore.ViewModels
{
    public class VariantCreateVM
    {
        public Variant? Variant { get; set; }
        public IEnumerable<Color> ColorList { get; set; } = [];
        public IEnumerable<StatusDto> StatusList { get; set; } = SD.GetVariantStatusList();
        public IEnumerable<Size> SizeList { get; set; } = [];
    }
}
