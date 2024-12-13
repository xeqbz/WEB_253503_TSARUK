using Microsoft.AspNetCore.Mvc;
using WEB_253503_TSARUK.Domain.Models;

namespace WEB_253503_TSARUK.UI.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        private readonly Cart _cart;

        public CartViewComponent(Cart cart)
        {
            _cart = cart;
        }

        public IViewComponentResult Invoke()
        {
            return View(_cart);
        }
    }
}
