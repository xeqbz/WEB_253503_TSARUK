using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WEB_253503_TSARUK.UI.Services.JewelryService;
using WEB_253503_TSARUK.Domain.Entities;
using WEB_253503_TSARUK.UI.Services.CategoryService;

namespace WEB_253503_TSARUK.UI.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly IJewelryService _jewelryService;
        private readonly ICategoryService _categoryService;
        private readonly IHttpClientFactory _httpClientFactory;

        public EditModel(IJewelryService jewelryService, ICategoryService categoryService, IHttpClientFactory httpClientFactory)
        {
            _jewelryService = jewelryService;
            _categoryService = categoryService;
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public Jewelry Jewelry { get; set; } = new Jewelry();

        [BindProperty]
        public IFormFile? Upload {  get; set; } 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _jewelryService.GetProductByIdAsync(id.Value);
            if (!response.Successfull || response.Data == null)
                return NotFound();

            Jewelry = response.Data;

            var categoriesResponse = await _categoryService.GetCategoryListAsync();
            ViewData["CategoryId"] = new SelectList(categoriesResponse.Data, "Id", "Name");
            return Page();
        }


        //public async Task<IActionResult> OnPostAsync()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Page();
        //    }

        //    _context.Attach(Jewelry).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!JewelryExists(Jewelry.Id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return RedirectToPage("./Index");
        //}

        //private bool JewelryExists(int id)
        //{
        //    return _context.Jewelries.Any(e => e.Id == id);
        //}
    }
}
