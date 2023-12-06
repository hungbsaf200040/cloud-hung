using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ESHOP.Data;
using ESHOP.Models;

using System.Drawing.Printing;
using static ESHOP.Controllers.ProductsController;
using ESHOP.Models.ViewModels;

namespace ESHOP.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ESHOPDBContext _context;
        public int PageSize = 9;
        public ProductsController(ESHOPDBContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        public class PriceRange
        {
            public int Min { get; set; }
            public int Max { get; set; }
        }
        [HttpPost]
        public IActionResult GetFilteredProducts([FromBody] FilterData filter)
        {
            var filterdProducts = _context.Products.ToList();
            if(filter.PriceRanges !=null&& filter.PriceRanges.Count>0 && !filter.PriceRanges.Contains("all"))
            {
                List<PriceRange> priceRanges = new List<PriceRange>(); 
                foreach(var range in filter.PriceRanges)
                {
                    var value = range.Split("-").ToArray();
                    PriceRange priceRange = new PriceRange();
                    priceRange.Min = Int16.Parse(value[0]);
                    priceRange.Max = Int16.Parse(value[1]);
                    priceRanges.Add(priceRange);
                }
                filterdProducts = filterdProducts.Where(p => priceRanges.Any(r => p.ProductPrice >= r.Min && p.ProductPrice <= r.Max)).ToList();

            }
            //if (filter.Colors != null && filter.Colors.Count > 0 && !filter.Colors.Contains("all"))
            //{
            //    filterdProducts = filterdProducts.Where(p => filter.Colors.Contains(p.Color.ColorName)).ToList();
            //}
            //if (filter.Sizes != null && filter.Sizes.Count > 0 && !filter.Sizes.Contains("all"))
            //{
            //    filterdProducts = filterdProducts.Where(p => filter.Sizes.Contains(p.Size.SizeName)).ToList();
            //}

            return PartialView("_ReturnProducts",filterdProducts);
        }


        // GET: Products

        public async Task<IActionResult> Index(int ProductPage = 1)
        {
            
            return View(new ProductListViewModel
            {
           Products = _context.Products
           .Skip((ProductPage - 1) * PageSize)
          .Take(PageSize),
                PagingInfo = new PagingInfo
           {
               ItemsPerPage = PageSize,
               CurrenPage = ProductPage,
               TotalItems = _context.Products.Count()
           }
       }
       );
        }

        [HttpPost]
        public async Task<IActionResult> Search(string keywords, int ProductPage = 1)
        {


            return View("Index",
                new ProductListViewModel
                {
                   Products = _context.Products
                    .Where(p => p.ProductName.Contains(keywords))
                    .Skip((ProductPage - 1) * PageSize)
                    .Take(PageSize),
                    PagingInfo = new PagingInfo
                    {
                        ItemsPerPage = PageSize,
                        CurrenPage = ProductPage,
                        TotalItems = _context.Products.Count()
                    }
                }
                );
        }
        public async Task<IActionResult> ProductsByCart(int categoryId)
        {
            var eSHOPDBContext = _context.Products.Where(p => p.CategoryId == categoryId).Include(p => p.Category);
            return View("IndexProducts", await eSHOPDBContext.ToListAsync());
        }



        public async Task<IActionResult> Admin()
        {
            var eSHOPDBContext = _context.Products.Include(p => p.Category);
            return View(await eSHOPDBContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
               
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
           
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]



        public async Task<IActionResult> Create([Bind("ProductId,ProductName,ProductDescription,CategoryId,ProductPrice,ProductDiscount,ProductPhoto,IsTrandy,IsArrived")] Product product)
        {
            if (ModelState.IsValid)

            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
           
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
           
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ProductDescription,CategoryId,ProductPrice,ProductDiscount,ProductPhoto,IsTrandy,IsArrived")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
           
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ESHOPDBContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
