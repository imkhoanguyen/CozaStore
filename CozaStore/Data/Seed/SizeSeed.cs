using CozaStore.Models;

namespace CozaStore.Data.Seed
{
    public class SizeSeed
    {
        public static void Seed(DataContext context)
        {
            if (context.Sizes.Any()) return;
            var sizes = new List<Size>
            {
                new Size { Name = "XS" },
                new Size { Name = "S" },
                new Size { Name = "M" },
                new Size { Name = "L" },
                new Size { Name = "XL" },
                new Size { Name = "XXL" },
                new Size { Name = "34" },
                new Size { Name = "35" },
                new Size { Name = "36" },
                new Size { Name = "37" },
                new Size { Name = "38" },
                new Size { Name = "39" },
                new Size { Name = "41" },
                new Size { Name = "42" },
                new Size { Name = "43" },
            };
            context.Sizes.AddRange(sizes);
            context.SaveChanges();
        }
    }
}
