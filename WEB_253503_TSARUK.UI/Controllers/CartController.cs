using Microsoft.AspNetCore.Mvc;
using WEB_253503_TSARUK.UI.Services.JewelryService;
using WEB_253503_TSARUK.Domain.Models;

namespace WEB_253503_TSARUK.UI.Controllers
{
    public class CartController : Controller
    {
        private readonly IJewelryService _jewelryService;
        private readonly Cart _cart;

        public CartController(IJewelryService jewelryService, Cart cart)
        {
            _jewelryService = jewelryService;
            _cart = cart;
        }

        public async Task<IActionResult> Add(int id, string returnUrl)
        {
            var data = await _jewelryService.GetProductByIdAsync(id);

            if (data != null && data.Successfull && data.Data != null)
                _cart.AddToCart(data.Data);

            return Redirect(returnUrl);
        }

        public IActionResult Remove(int id, string returnUrl)
        {
            _cart.RemoveItems(id);
            return Redirect(returnUrl);
        }

        public IActionResult Clear(string returnUrl)
        {
            _cart.ClearAll();
            return Redirect(returnUrl);
        }

        public IActionResult ViewCart()
        {
            return View(_cart);
        }
    }
}
