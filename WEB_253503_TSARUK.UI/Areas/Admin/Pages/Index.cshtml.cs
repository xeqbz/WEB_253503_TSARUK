using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WEB_253503_TSARUK.Domain.Entities;
using WEB_253503_TSARUK.UI.Services.JewelryService;

namespace WEB_253503_TSARUK.UI.Areas.Admin.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IJewelryService _jewelryService;

        public IndexModel(IJewelryService jewelryService)
        {
            _jewelryService = jewelryService;
        }

        public IList<Jewelry> Jewelry { get;set; } = new List<Jewelry>();

        public async Task OnGetAsync()
        {
            var response = await _jewelryService.GetAllProductListAsync(null, 1);
            if (response.Successfull && response.Data != null)
                Jewelry = response.Data.Items;
            else
                Jewelry = new List<Jewelry>();
        }
    }
}
