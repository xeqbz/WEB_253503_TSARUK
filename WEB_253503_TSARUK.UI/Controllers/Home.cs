using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_253503_TSARUK.UI.Models;

namespace WEB_253503_TSARUK.UI.Controllers
{
    public class Home : Controller
    {
        public IActionResult Index()
        {
            ViewData["lb"] = "Лабораторная работа №2";

            var items = new List<ListDemo>
            {
                new ListDemo { Id = 1, Name = "Item 1" },
                new ListDemo { Id = 2, Name = "Item 2" },
                new ListDemo { Id = 3, Name = "Item 3" }
            };
            var selectList = new SelectList(items, "Id", "Name");

			return View(selectList);
        }
    }
}
