using ESHOP.Data;
using ESHOP.Infrastructure;
using ESHOP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ESHOP.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        public Cart? Cart { get; set; }
        private readonly ESHOPDBContext _context;

        public CartController(ESHOPDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View("Cart", HttpContext.Session.GetJson<Cart>("cart"));
        }
        public IActionResult AddToCart(int productId)
        {
            Product? product = _context.Products.FirstOrDefault(product => product.ProductId == productId);
            if (product != null)
            {
                Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
                Cart.AddItem(product, 1);
                HttpContext.Session.SetJson("cart", Cart);
            }
            return View("Cart", Cart);

        }
        public IActionResult UpdateCart(int productId)
        {
            Product? product = _context.Products.FirstOrDefault(product => product.ProductId == productId);
            if (product != null)
            {
                Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
                Cart.AddItem(product, -1);
                HttpContext.Session.SetJson("cart", Cart);
            }
            return View("Cart", Cart);

        }
        public IActionResult RemoveFromCart(int productId)
        {
            Product? product = _context.Products.FirstOrDefault(product => product.ProductId == productId);
            if (product != null)
            {
                Cart = HttpContext.Session.GetJson<Cart>("cart");
                Cart.RemoveLine(product);
                HttpContext.Session.SetJson("cart", Cart);
            }
            return View("Cart", Cart);

        }
    }
}
