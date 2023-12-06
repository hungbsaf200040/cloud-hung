using ESHOP.Data;
using Microsoft.AspNetCore.Mvc;

namespace ESHOP.Components
{
    public class Imagebar: ViewComponent
    {
        private readonly ESHOPDBContext _context;

        public Imagebar(ESHOPDBContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            return View("Index",_context.Categories.ToList());
        }
    }
}
