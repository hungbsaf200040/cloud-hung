using ESHOP.Data;
using Microsoft.AspNetCore.Mvc;

namespace ESHOP.Components
{
    public class JustArrived: ViewComponent
    {
        private readonly ESHOPDBContext _context;

        public JustArrived(ESHOPDBContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            return View(_context.Products.Where(p => p.IsArrived == true).ToList());
        }
    }
}
