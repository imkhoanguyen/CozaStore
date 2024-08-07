﻿namespace CozaStore.Helpers
{
    public static class SD
    {
        public static readonly IEnumerable<VariantStatusDto> VariantStatusList = Enum.GetValues(typeof(VariantStatus))
        .Cast<VariantStatus>()
        .Select(status => new VariantStatusDto
        {
            Id = (int)status,
            Name = Enum.GetName(typeof(VariantStatus), status) ?? string.Empty
        })
        .ToList();

        public static readonly IEnumerable<ProductStatusDto> ProductStatusList = Enum.GetValues(typeof(ProductStatus))
        .Cast<ProductStatus>()
        .Select(status => new ProductStatusDto
        {
            Id = (int)status,
            Name = Enum.GetName(typeof(ProductStatus), status) ?? string.Empty
        })
        .ToList();
    }
}
