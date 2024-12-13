using Microsoft.AspNetCore.Mvc;
using WEB_253503_TSARUK.Domain.Models;
using WEB_253503_TSARUK.Domain.Entities;
using WEB_253503_TSARUK.UI.Services.CategoryService;
using WEB_253503_TSARUK.UI.Services.JewelryService;
using WEB_253503_TSARUK.UI.Extensions;

namespace WEB_253503_TSARUK.UI.Controllers
{
    [Route("Catalog")]
    public class JewelryController : Controller
    {
        IJewelryService _jewelryService;
        ICategoryService _categoryService;

        public JewelryController(IJewelryService jewelryService, ICategoryService catService)
        {
            _jewelryService = jewelryService;
            _categoryService = catService;
        }

        [Route("")]
        [Route("{category?}")]
        public async Task<IActionResult> Index(string? category, int pageNo = 1)
        {
            var categoryResponse = await _categoryService.GetCategoryListAsync();
            if (!categoryResponse.Successfull)
                return NotFound(categoryResponse.ErrorMessage);
            ViewBag.Categories = categoryResponse.Data;
            
            var productResponse = await _jewelryService.GetProductListAsync(category, pageNo);
            if (!productResponse.Successfull)
                return NotFound(productResponse.ErrorMessage);

            var currentCategory = category;
            if (string.IsNullOrEmpty(currentCategory))
                currentCategory = "All";
            else
            {
                var selectedCategory = categoryResponse.Data.FirstOrDefault(c => c.NormalizedName == currentCategory);
                currentCategory = selectedCategory?.Name ?? "All";
            }

            ViewBag.CurrentCategory = currentCategory;
            ViewBag.CurrentPage = pageNo;
            ViewBag.TotalPages = productResponse.Data.TotalPages;

            if (Request.IsAjaxRequest())
                return PartialView("~/Views/Shared/Components/Jewelry/_ProductListPartial.cshtml", productResponse.Data);

            return View(productResponse.Data);
        }
    }


}
