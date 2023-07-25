using Microsoft.AspNetCore.Mvc;
using Northwind.Mvc.Models;
using System.Diagnostics;
using Packt.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Northwind.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly NorthwindContext db;

        public HomeController(ILogger<HomeController> logger, NorthwindContext injectContext)
        {
            _logger = logger;
            db = injectContext;
        }
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> IndexAsync()
        {
            HomeIndexViewModel model = new(
                VisitorCount: Random.Shared.Next(1, 1002),
                Categories: await db.Categories.ToListAsync(),
                Products: await db.Products.ToListAsync());
            return View(model);
        }
        [Route("private")]
        [Authorize(Roles = "Administrators")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<IActionResult> ProductDetail(int? id, string alertstyle="success")
        {
            ViewData["alertstyle"] = alertstyle;
            if (!id.HasValue)
            {
                return BadRequest("you must pass a product ID in the route, for example /Home/ProductDetails/21");
            }
            Product? model = await db.Products.SingleOrDefaultAsync(p => p.ProductId == id);
            if(model is null)
            {
                return NotFound($"ProductId {id} not found");
            }
            return View(model);
        }

        public IActionResult ModelBinding()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ModelBinding(Thing thing)
        {
            HomeModelBindingViewModel model = new(Thing: thing, HasErrors: !ModelState.IsValid, ValidationErrors: ModelState.Values.SelectMany(state => state.Errors).Select(error => error.ErrorMessage));
            return View(model);
        }

        public IActionResult ProductsThatCostMoreThan(decimal? price)
        {
            if(!price.HasValue)
            {
                return BadRequest("You must pass a product price in the query string, for example, /Home/ProductsThatCostMoreThan?price=50");
            }

            IEnumerable<Product> model = db.Products.Include(p => p.Category).Include(p => p.Supplier).Where(p=>p.UnitPrice>price);

            if (!model.Any())
            {
                return NotFound($"Not found products cost more than {price:C}");
            }
            ViewData["MaxPrice"] = price.Value.ToString("C");
            return View(model);
        }

        public async Task<IActionResult> Category(int? id)
        {
            if(!id.HasValue)
            {
                return BadRequest("You must pass a product price in the query string, for example, /Home/Category/2");
            }
            IEnumerable<Product> model = db.Products.Where(p => p.CategoryId == id);
            if (!model.Any())
            {
                return NotFound($"CategoryId {id} not found");
            }
            ViewData["Title"] = model.First().Category;
            return View(model);
        }
    }
}