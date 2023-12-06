using ESHOP.Data;
using Microsoft.AspNetCore.Mvc;

namespace ESHOP.Components
{
    public class Navbar: ViewComponent
    {
        private readonly ESHOPDBContext _context;

        public Navbar(ESHOPDBContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            return View(_context.Categories.ToList());
        }

    }
}
