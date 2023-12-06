using ESHOP.Data;
using Microsoft.AspNetCore.Mvc;

namespace ESHOP.Components
{
    public class Trandy: ViewComponent
    {
      
            private readonly ESHOPDBContext _context;

            public Trandy(ESHOPDBContext context)
            {
                _context = context;
            }
            public IViewComponentResult Invoke()
            {
                return View(_context.Products.Where(p => p.IsTrandy == true).ToList());
            }
    }
}
