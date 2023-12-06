using ESHOP.Infrastructure;
using ESHOP.Models;
using Microsoft.AspNetCore.Mvc;

namespace ESHOP.Components
{
    public class CartWidget: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View(HttpContext.Session.GetJson<Cart>("cart"));
        }
    }
}
