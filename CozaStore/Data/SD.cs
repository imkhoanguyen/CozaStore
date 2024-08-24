using CozaStore.Data.Enum;
using CozaStore.DTOs;

namespace CozaStore.Data
{
    public static class SD
    {
        public static IEnumerable<StatusDto> GetVariantStatusList()
        {
            List<StatusDto> statusList = new List<StatusDto>
            {
                new StatusDto {Id = (int)VariantStatus.Private, Name = VariantStatus.Private.ToString()},
                new StatusDto {Id = (int)VariantStatus.Public, Name = VariantStatus.Public.ToString()},
            };
            return statusList;
        }

        public static IEnumerable<StatusDto> GetProductStatusList()
        {
            List<StatusDto> statusList = new List<StatusDto>
            {
                new StatusDto {Id = (int)ProductStatus.Deleted, Name = ProductStatus.Deleted.ToString()},
                new StatusDto {Id = (int)ProductStatus.Private, Name = ProductStatus.Private.ToString()},
                 new StatusDto {Id = (int)ProductStatus.Public, Name = ProductStatus.Public.ToString()},
            };
            return statusList;
        }

        public static readonly List<string> PriceRangeList = new List<string>
        {
            "$0-$50",
            "$50-$100",
            "$100-$150",
            "$150-$200",
            "$200+"
        };
    }
}
