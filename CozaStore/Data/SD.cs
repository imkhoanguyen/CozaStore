using CozaStore.Data.Enum;
using CozaStore.DTOs;

namespace CozaStore.Data
{
    public static class SD
    {

        public static IEnumerable<StatusDto> GetVariantStatusList()
        {
            return VariantStatus.GetValues(typeof(VariantStatus))
                       .Cast<VariantStatus>()
                       .Select(status => new StatusDto
                       {
                           Id = (int)status,
                           Name = status.ToString()
                       });
        }


        public static IEnumerable<StatusDto> GetProductStatusList()
        {
            return ProductStatus.GetValues(typeof(ProductStatus))
                       .Cast<ProductStatus>()
                       .Select(status => new StatusDto
                       {
                           Id = (int)status,
                           Name = status.ToString()
                       });
        }

        public static IEnumerable<StatusDto> GetOrderStatusList()
        {
            return OrderStatus.GetValues(typeof(OrderStatus))
                       .Cast<OrderStatus>()
                       .Select(status => new StatusDto
                       {
                           Id = (int)status,
                           Name = status.ToString()
                       });
        }

        public static IEnumerable<StatusDto> GetPaymentStatusList()
        {
            return PaymentStatus.GetValues(typeof(PaymentStatus))
                       .Cast<PaymentStatus>()
                       .Select(status => new StatusDto
                       {
                           Id = (int)status,
                           Name = status.ToString()
                       });
        }


        public static readonly IEnumerable<string> PriceRangeList = new List<string>
        {
            "$0-$50",
            "$50-$100",
            "$100-$150",
            "$150-$200",
            "$200+"
        };

    }
}
