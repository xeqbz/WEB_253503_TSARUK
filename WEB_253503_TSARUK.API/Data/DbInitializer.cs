using Microsoft.EntityFrameworkCore;
using WEB_253503_TSARUK.Domain.Entities;

namespace WEB_253503_TSARUK.API.Data
{
    public static class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await context.Database.MigrateAsync();

            if (context.Categories.Any() || context.Jewelries.Any())
            {
                return;
            }

            var baseUrl = app.Configuration.GetValue<string>("AppSettings:BaseUrl");

            var categories = new Category[]
            {
                new Category { Name = "Серьги", NormalizedName = "earrings"},
                new Category { Name = "Кольца", NormalizedName = "rings"},
                new Category { Name = "Браслеты", NormalizedName = "bracelets"}
            };

            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            var jewelries = new Jewelry[]
            {
                new Jewelry
                {
                    Name = "Heart Ring",
                    Description = "Hand-finished in 14k rose gold unique metal blend, this piece features a pink heart-shaped central, elevated stone.",
                    Price = 10500,
                    Image = $"{baseUrl}/images/heart-ring.jpg",
                    Category = categories.FirstOrDefault(c=>c.NormalizedName == "rings")
                },
                new Jewelry
                {
                    Name = "Sparlking Band Ring",
                    Description = "The pavé row sparkles from the outer front half of the band, which features a squared profile. Peek inside the band to see the engraved iconic Pandora logo.",
                    Price = 4500,
                    Image = $"{baseUrl}/images/sparkling-ring.jpg",
                    Category = categories.FirstOrDefault(c=>c.NormalizedName == "rings"),
                },
                new Jewelry
                {
                    Name = "Sparkling Hoop Earrings",
                    Description = "An essential for every collection, these sterling silver hoop earrings are set with a row of clear cubic zirconia. Versatile and classic, these hoops make a winning gift or addition to your own look. ",
                    Price = 5600,
                    Image = $"{baseUrl}/images/sparkling-ear.jpg",
                    Category = categories.FirstOrDefault(c=>c.NormalizedName == "earrings")
                },
                new Jewelry
                {
                    Name = "Elevated Heart Stud Earrings",
                    Description = "These timeless and dreamy sterling silver earrings feature an elevated cubic zirconia setting for extra brilliance and sparkle. The sparkling ear studs are a classic style that will be treasured for years to come thanks to its sophisticated, yet simple design.",
                    Price = 6000,
                    Image = $"{baseUrl}/images/heart-ear.jpg",
                    Category = categories.FirstOrDefault(c=>c.NormalizedName == "earrings")
                },
                new Jewelry
                {
                    Name = "Sparkling Tennis Bracelet",
                    Description = "This sterling silver Sparkling Tennis Bracelet features squared collets and is decorated with a bold line of clear cubic zirconia to make a statement fit for any occasion.",
                    Price = 11000,
                    Image = $"{baseUrl}/images/tennis-braclet.jpg",
                    Category = categories.FirstOrDefault(c=>c.NormalizedName == "bracelets")
                },
                new Jewelry
                {
                    Name = "Celestial Stars Bracelet",
                    Description = "Our star bracelet features a collection of irregularly-shaped sterling silver star shapes embellished with round cubic zirconia stones bound together by pavé bars to evoke a life beyond your wildest dreams, with each star symbolizing a past milestone or a future achievement.",
                    Price = 8800,
                    Image = $"{baseUrl}/images/star-braclet.jpg",
                    Category = categories.FirstOrDefault(c=>c.NormalizedName == "bracelets")
                }
            };

            await context.Jewelries.AddRangeAsync(jewelries);
            await context.SaveChangesAsync();
        }
    }
}
