using CozaStore.Models;

namespace CozaStore.Data.Seed
{
    public class CategorySeed
    {
        public static void Seed(DataContext context)
        {
            if (context.Categories.Any()) return;
            var categories = new List<Category>
            {
                new Category { Name = "Women" },
                new Category { Name = "Men" },
                new Category { Name = "Shirts" },
                new Category { Name = "Tops" },
                new Category { Name = "Jean" },
                new Category { Name = "Dresses" },
                new Category { Name = "Skirts" },
                new Category { Name = "Hats" },
                new Category { Name = "Sweatshirts & Hoodies" },
            };
            context.Categories.AddRange(categories);
            context.SaveChanges();
        }
    }
}
