using Xunit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_253503_TSARUK.API.Data;
using WEB_253503_TSARUK.Domain.Entities;
using WEB_253503_TSARUK.API.Services.JewelryService;

namespace WEB_253503_TSARUK.Tests.Services
{
    public class JewelryServiceTests
    {
        private AppDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);
            return context;
        }

        [Fact]
        public async Task GetProductListAsync_ReturnsFirstPageWithThreeItemsAndCalculateTotalPagesCorrectly()
        {
            var context = CreateInMemoryDbContext();
            var categories = new List<Category>
            {
                new Category { Name = "Кольца", NormalizedName = "rings" },
                new Category { Name = "Серьги", NormalizedName = "earrings" },
                new Category { Name = "Браслеты", NormalizedName = "bracelets" }
            };

            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            context.Jewelries.AddRange(
                new Jewelry { Name = "Jewelry 1", Description = "Description 1", Price = 1000, Category = categories[0], Image = null },
                new Jewelry { Name = "Jewelry 2", Description = "Description 2", Price = 2000, Category = categories[1], Image = null },
                new Jewelry { Name = "Jewelry 3", Description = "Description 3", Price = 3000, Category = categories[1], Image = null },
                new Jewelry { Name = "Jewelry 4", Description = "Description 4", Price = 4000, Category = categories[2], Image = null },
                new Jewelry { Name = "Jewelry 5", Description = "Description 5", Price = 5000, Category = categories[0], Image = null },
                new Jewelry { Name = "Jewelry 6", Description = "Description 6", Price = 6000, Category = categories[1], Image = null }
            );

            await context.SaveChangesAsync();

            var service = new JewelryService(context);
            var result = await service.GetProductListAsync(null);

            Assert.True(result.Successfull);
            Assert.NotNull(result.Data);
            Assert.Equal(3, result.Data.Items.Count);

            int totalPages = (int)Math.Ceiling(6 / (double)3);
            Assert.Equal(totalPages, result.Data.TotalPages);
        }

        [Fact]
        public async Task GetProductListAsync_ReturnsCorrectPage_WhenSpecificPageRequested()
        {
            var context = CreateInMemoryDbContext();
            var categories = new List<Category>
            {
                new Category { Name = "Кольца", NormalizedName = "rings" }
            };

            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            context.Jewelries.AddRange(
                new Jewelry { Name = "Jewelry 1", Description = "Description 1", Price = 1000, Category = categories[0], Image = null },
                new Jewelry { Name = "Jewelry 2", Description = "Description 2", Price = 2000, Category = categories[0], Image = null },
                new Jewelry { Name = "Jewelry 3", Description = "Description 3", Price = 3000, Category = categories[0], Image = null },
                new Jewelry { Name = "Jewelry 4", Description = "Description 4", Price = 4000, Category = categories[0], Image = null },
                new Jewelry { Name = "Jewelry 5", Description = "Description 5", Price = 5000, Category = categories[0], Image = null },
                new Jewelry { Name = "Jewelry 6", Description = "Description 6", Price = 6000, Category = categories[0], Image = null }
            );

            await context.SaveChangesAsync();

            var service = new JewelryService(context);

            int requestPageNo = 2;
            int pageSize = 3;

            var result = await service.GetProductListAsync(null, requestPageNo, pageSize);

            Assert.True(result.Successfull);
            Assert.NotNull(result.Data);
            Assert.Equal(3, result.Data.Items.Count);
            Assert.Equal(requestPageNo, result.Data.CurrentPage);

            Assert.Contains(result.Data.Items, m => m.Name == "Jewelry 4");
            Assert.Contains(result.Data.Items, m => m.Name == "Jewelry 5");
            Assert.Contains(result.Data.Items, m => m.Name == "Jewelry 6");
        }

        [Fact]
        public async Task GetProductListAsync_FiltersJewelriesByCategoryCorrectly()
        {
            var context = CreateInMemoryDbContext();

            var categories = new List<Category>
            {
                new Category { Name = "Кольца", NormalizedName = "rings" },
                new Category { Name = "Серьги", NormalizedName = "earrings" }
            };

            await context.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            context.Jewelries.AddRange(
                new Jewelry { Name = "Jewelry 1", Description = "Description 1", Price = 1000, Category = categories[0], Image = null },
                new Jewelry { Name = "Jewelry 2", Description = "Description 2", Price = 2000, Category = categories[0], Image = null },
                new Jewelry { Name = "Jewelry 3", Description = "Description 3", Price = 3000, Category = categories[1], Image = null },
                new Jewelry { Name = "Jewelry 4", Description = "Description 4", Price = 4000, Category = categories[1], Image = null },
                new Jewelry { Name = "Jewelry 5", Description = "Description 5", Price = 5000, Category = categories[1], Image = null }
            );

            await context.SaveChangesAsync();
            
            var service = new JewelryService(context);
            string categoryNormalizedName = "earrings".ToLower();
            var result = await service.GetProductListAsync(categoryNormalizedName);

            Assert.True(result.Successfull);
            Assert.NotNull(result.Data);
            Assert.Equal(3, result.Data.Items.Count);

            Assert.Equal(1, result.Data.CurrentPage);
            Assert.Equal(1, result.Data.TotalPages);

            foreach (var jewelry in result.Data.Items)
                Assert.Equal("Серьги", jewelry.Category.Name);
        }

        [Fact]
        public async Task GetProductListAsync_DoesNotAllowPageSizeGreaterThanMaxPageSize()
        {
            var context = CreateInMemoryDbContext();

            var categories = new List<Category>
            {
                new Category { Name = "Серьги", NormalizedName = "earrings" }
            };

            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            for (int i = 1; i <= 25; i++)
            {
                context.Jewelries.Add(new Jewelry { Name = $"Jewelry {i}", Description = $"Description {i}", Price = 1000 + i, Category = categories[0] });
            }

            await context.SaveChangesAsync();

            var service = new JewelryService(context);

            int requestedPageSize = 50;
            int maxPageSize = 20;

            var result = await service.GetProductListAsync(null, 1, requestedPageSize);

            Assert.True(result.Successfull);
            Assert.NotNull(result.Data);
            Assert.Equal(maxPageSize, result.Data.Items.Count);
            Assert.Equal(1, result.Data.CurrentPage);
        }

        [Fact]
        public async Task GetProductListAsync_ReturnsError_WhenPageNumber_ExceedsTotalPages()
        {
            var context = CreateInMemoryDbContext();

            var categories = new List<Category>
            {
                 new Category { Name = "Кольца", NormalizedName = "rings" }
            };

            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            for (int i = 1; i <= 5; i++)
            {
                context.Jewelries.Add(new Jewelry { Name = $"Jewelry {i}", Description = $"Description {i}", Price = 1000 + i, Category = categories[0] });
            }

            await context.SaveChangesAsync();

            var service = new JewelryService(context);

            int requestedPageNo = 3;
            int pageSize = 3;

            var result = await service.GetProductListAsync(null, requestedPageNo, pageSize);

            Assert.False(result.Successfull);
            Assert.Equal("No such page", result.ErrorMessage);
        }
    }
}
