using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TextileEshop.Models;

namespace TextileEshop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var trendingProducts = await _context.Products
                .OrderByDescending(p => p.CreatedDate)
                .Take(6)
                .ToListAsync();

            var newArrivals = await _context.Products
                .OrderByDescending(p => p.CreatedDate)
                .Take(6)
                .ToListAsync();

            ViewBag.TrendingProducts = trendingProducts;
            ViewBag.NewArrivals = newArrivals;

            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var reviews = await _context.Reviews
                .Where(r => r.ProductId == id)
                .Include(r => r.User)
                .OrderByDescending(r => r.Date)
                .ToListAsync();

            ViewBag.Reviews = reviews;

            return View(product);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
