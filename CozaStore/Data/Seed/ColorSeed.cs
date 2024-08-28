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
                new Color { Name = "Black", Code = "Black" },
                new Color { Name = "White", Code = "White"},
                new Color { Name = "Gray", Code = "Gray"},
                new Color { Name = "Blue", Code = "Blue"},
                new Color { Name = "Green", Code = "Green"},
                new Color { Name = "Pink", Code = "Pink"},
                new Color { Name = "Orange", Code = "Orange"},
                new Color { Name = "Brown", Code = "Brown"},
                new Color { Name = "Yellow", Code = "Yellow"},
                new Color { Name = "Purple", Code = "Purple"},
                new Color { Name = "Red", Code ="Red" },
                new Color {Name = "Navy", Code ="Navy"},
                new Color {Name= "Ivory", Code = "Ivory"},

            };
            context.Colors.AddRange(colors);
            context.SaveChanges();
        }
    }
}
