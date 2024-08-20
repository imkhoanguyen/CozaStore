using CozaStore.Models;

namespace CozaStore.Data.Seed
{
    public class ColorSeed
    {
        public static void Seed(DataContext context)
        {
            if (context.Colors.Any()) return;
            var colors = new List<Color>
            {
                new Color { Name = "Black" },
                new Color { Name = "White" },
                new Color { Name = "Gray" },
                new Color { Name = "Blue" },
                new Color { Name = "Green" },
                new Color { Name = "Pink" },
                new Color { Name = "Orange" },
                new Color { Name = "Brown" },
                new Color { Name = "Yellow" },
                new Color { Name = "Purple" },
                new Color { Name = "Red" },
                new Color {Name = "Navy"},
                new Color {Name= "Ivory"},

            };
            context.Colors.AddRange(colors);
            context.SaveChanges();
        }
    }
}
