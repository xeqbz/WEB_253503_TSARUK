using NSubstitute;
using WEB_253503_TSARUK.Domain.Entities;
using WEB_253503_TSARUK.Domain.Models;
using WEB_253503_TSARUK.UI.Services.CategoryService;
using WEB_253503_TSARUK.UI.Services.JewelryService;
using WEB_253503_TSARUK.UI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Runtime.CompilerServices;

namespace WEB_253503_TSARUK.Tests.Controllers
{
    public class JewelryControllerTests
    {
        private readonly IJewelryService _jewelryService = Substitute.For<IJewelryService>();
        private readonly ICategoryService _categoryService = Substitute.For<ICategoryService>();

        private JewelryController CreateController()
        {
            return new JewelryController(_jewelryService,_categoryService);
        }

        [Fact]
        public async Task Index_ReturnsNotFound_WhenCategoriesNotLoaded()
        {
            var controller = CreateController();
            _categoryService.GetCategoryListAsync().Returns(Task.FromResult(ResponseData<List<Category>>.Error("Не удалось загрузить категории")));

            var result = await controller.Index(null);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Не удалось загрузить категории", notFoundResult.Value);
        }

        [Fact]
        public async Task Index_ReturnsNotFound_WhenJewelryNotLoaded()
        {
            var controller = CreateController();
            var category = "TestCategory";
            _categoryService.GetCategoryListAsync().Returns(Task.FromResult(ResponseData<List<Category>>.Success(new List<Category> { new Category { Name = "TestCategory", NormalizedName = "testcategory" } })));
            _jewelryService.GetProductListAsync(category, 1).Returns(Task.FromResult(ResponseData<ListModel<Jewelry>>.Error("Не удалось загрузить украшения")));

            var result = await controller.Index(category);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Не удалось загрузить украшения", notFoundResult.Value);
        }

        [Fact]
        public async Task Index_PopulatesViewDataWithCategories_WhenCategoriesAreSuccessfullyLoaded()
        {
            var controller = CreateController();
            var httpContext = new DefaultHttpContext();
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
            httpContext.Request.Headers["X-Requested-With"] = "";

            var expectedCategories = new List<Category>
            {
                new Category { Name = "Серьги", NormalizedName = "серьги"},
                new Category { Name = "Кольца", NormalizedName = "кольца"},
                new Category { Name = "Браслеты", NormalizedName = "браслеты"},
            };

            _categoryService.GetCategoryListAsync().Returns(Task.FromResult(ResponseData<List<Category>>.Success(expectedCategories)));
            _jewelryService.GetProductListAsync(null, 1).Returns(Task.FromResult(ResponseData<ListModel<Jewelry>>.Success(new ListModel<Jewelry>())));

            var result = await controller.Index(null);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.ViewData["Categories"]);
            var categoriesInViewData = viewResult.ViewData["Categories"] as List<Category>;
            Assert.Equal(expectedCategories, categoriesInViewData);
        }

        [Fact]
        public async Task Index_SetsCurrentCategoryToAll_WhenCategoryIsNull()
        {
            var controller = CreateController();
            var httpContext = new DefaultHttpContext();
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            _categoryService.GetCategoryListAsync().Returns(Task.FromResult(ResponseData<List<Category>>.Success(new List<Category>())));
            _jewelryService.GetProductListAsync(null, 1).Returns(Task.FromResult(ResponseData<ListModel<Jewelry>>.Success(new ListModel<Jewelry>())));

            var result = await controller.Index(null);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("All", viewResult.ViewData["CurrentCategory"]);
        }

        [Fact]
        public async Task Index_Sets_CurrentCategoryCorrectly_WhenCategoryIsSpecified()
        {
            var controller = CreateController();
            var httpContext = new DefaultHttpContext();
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext= httpContext
            };

            string category = "rings";
            var categories = new List<Category>
            {
                new Category { Name = "Кольца", NormalizedName = "rings"},
                new Category { Name = "Серьги", NormalizedName = "earrings"},
                new Category { Name = "Браслеты", NormalizedName = "bracelets"}
            };

            _categoryService.GetCategoryListAsync().Returns(Task.FromResult(ResponseData<List<Category>>.Success(categories)));
            _jewelryService.GetProductListAsync(category, 1).Returns(Task.FromResult(ResponseData<ListModel<Jewelry>>.Success(new ListModel<Jewelry>())));

            var result = await controller.Index(category);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Кольца", viewResult.ViewData["CurrentCategory"]);
        }

        [Fact]
        public async Task Index_ReturnsViewWithJewelryListModel_WhenDataSuccessfullyLoaded()
        {
            var controller = CreateController();
            var httpContext = new DefaultHttpContext();
            controller.ControllerContext = new ControllerContext() { HttpContext = httpContext };

            string category = "rings";
            var categories = new List<Category>
            {
                new Category { Name = "Кольца", NormalizedName = "rings"},
                new Category { Name = "Серьги", NormalizedName = "earrings"},
                new Category { Name = "Браслеты", NormalizedName = "bracelets"}
            };

            var expectedJewelries = new ListModel<Jewelry>
            {
                Items = new List<Jewelry> { new Jewelry(), new Jewelry(), new Jewelry() },
                CurrentPage = 1,
                TotalPages = 2
            };

            _categoryService.GetCategoryListAsync().Returns(Task.FromResult(ResponseData<List<Category>>.Success(categories)));
            _jewelryService.GetProductListAsync(category, 1).Returns(Task.FromResult(ResponseData<ListModel<Jewelry>>.Success(expectedJewelries)));

            var result = await controller.Index(category);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ListModel<Jewelry>>(viewResult.Model);
            Assert.Equal(expectedJewelries, model);
        }
    }
}
