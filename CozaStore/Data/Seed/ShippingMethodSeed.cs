using CozaStore.Models;

namespace CozaStore.Data.Seed
{
    public class ShippingMethodSeed
    {
        public static void Seed(DataContext context)
        {
            if (context.ShippingMethods.Any()) return;
            var shippingMethods = new List<ShippingMethod>
            {
                new ShippingMethod { Name = "Standard Delivery", Description = "(4-5 working days)", Cost = 0 },
                new ShippingMethod { Name = "Express Delivery", Description = "(1-2 working days)", Cost = 4 },
            };
            context.ShippingMethods.AddRange(shippingMethods);
            context.SaveChanges();
        }
    }
}
