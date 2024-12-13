using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WEB_253503_TSARUK.UI.Services.JewelryService;
using WEB_253503_TSARUK.Domain.Entities;

namespace WEB_253503_TSARUK.UI.Areas.Admin.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly IJewelryService _jewelryService;

        public DeleteModel(IJewelryService jewelryService)
        {
            _jewelryService = jewelryService;
        }

        [BindProperty]
        public Jewelry Jewelry { get; set; } = new Jewelry();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _jewelryService.GetProductByIdAsync(id.Value);
            if (!response.Successfull || response.Data == null)
            {
                return NotFound();
            }
            
            Jewelry = response.Data;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _jewelryService.DeleteProductAsync(id.Value); 

            return RedirectToPage("./Index");
        }
    }
}
