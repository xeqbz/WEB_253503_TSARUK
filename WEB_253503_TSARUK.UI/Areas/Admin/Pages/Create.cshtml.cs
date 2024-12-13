using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_253503_TSARUK.UI.Services.CategoryService;
using WEB_253503_TSARUK.UI.Services.JewelryService;
using WEB_253503_TSARUK.Domain.Entities;

namespace WEB_253503_TSARUK.UI.Areas.Admin.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IJewelryService _jewelryService;
        private readonly ICategoryService _categoryService;
        private readonly IHttpClientFactory _httpClientFactory;

        public CreateModel(IJewelryService jewelryService, ICategoryService categoryService, IHttpClientFactory httpClientFactory)
        {
            _jewelryService = jewelryService;
            _categoryService = categoryService;
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public Jewelry Jewelry { get; set; } = new Jewelry();

        [BindProperty]
        public IFormFile ImageFile { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync()
        {
            var categoriesResponse = await _categoryService.GetCategoryListAsync();
            ViewData["CategoryId"] = new SelectList(categoriesResponse.Data, "Id", "Name");
            return Page();
        }

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid) 
            //    return Page();

            if (ImageFile != null)
            {
                var imageUrl = await UploadImageToApiAsync(ImageFile);
                if (string.IsNullOrEmpty(imageUrl))
                {
                    ModelState.AddModelError("", "Cannot upload image");
                    return Page();
                }

                Jewelry.Image = imageUrl;
            }
            Console.WriteLine("StartingCreating");

            var response = await _jewelryService.CreateProductAsync(Jewelry, null);
            if (response.Successfull)
            {
                return RedirectToPage("./Index");
            }

            ModelState.AddModelError("", "Cannot create product: " + response.ErrorMessage);
            Console.WriteLine("Error in creating");
            return Page();
        }

        private async Task<string> UploadImageToApiAsync(IFormFile imageFile)
        {
            var client = _httpClientFactory.CreateClient();
            var apiUrl = "https://localhost:7002/api/images";

            using var content = new MultipartFormDataContent();
            using var fileStreamContent = new StreamContent(imageFile.OpenReadStream());
            fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(imageFile.ContentType);
            content.Add(fileStreamContent, "file", imageFile.FileName);

            var response = await client.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var imageUrl = await response.Content.ReadAsStringAsync();
                return imageUrl.Trim('"');
            }

            return "";
        }
    }
}
