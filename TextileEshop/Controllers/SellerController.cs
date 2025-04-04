using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TextileEshop.Models;

namespace TextileEshop.Controllers
{
    [Authorize(Roles = "Seller")]
    public class SellerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<SellerController> _logger;

        public SellerController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<SellerController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var products = await _context.Products
                                .Where(p => p.SellerId == userId)
                                .ToListAsync();
            return View(products);
        }

        public IActionResult Add() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Product product)
        {
            _logger.LogInformation("Attempting to add a new product.");

            try
            {
                var seller = await _userManager.GetUserAsync(User);
                if (seller == null)
                {
                    _logger.LogError("Seller not found.");
                    ModelState.AddModelError("", "Unable to identify the seller.");
                    return View(product);
                }

                product.SellerId = seller.Id;
                product.CreatedDate = DateTime.Now;

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Model validation failed:");
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        _logger.LogWarning($"Validation Error: {error.ErrorMessage}");
                    }
                    return View(product);
                }

                _context.Add(product);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Product '{product.Name}' added successfully by seller ID: {product.SellerId}.");
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error while adding product.");
                ModelState.AddModelError("", "Database error occurred while adding the product.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while adding product.");
                ModelState.AddModelError("", "An unexpected error occurred.");
            }

            return View(product);
        }



        public async Task<IActionResult> Details(int id)
        {
            var userId = _userManager.GetUserId(User);
            var product = await _context.Products
                            .FirstOrDefaultAsync(p => p.Id == id && p.SellerId == userId);

            if (product == null)
                return NotFound();

            return View(product);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userManager.GetUserId(User);
            var product = await _context.Products
                            .FirstOrDefaultAsync(p => p.Id == id && p.SellerId == userId);

            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id)
                return NotFound();

            var userId = _userManager.GetUserId(User);
            var existingProduct = await _context.Products
                                      .FirstOrDefaultAsync(p => p.Id == id && p.SellerId == userId);

            if (existingProduct == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                existingProduct.Name = product.Name;
                existingProduct.Category = product.Category;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;
                existingProduct.ImageUrl = product.ImageUrl;
                existingProduct.StockQuantity = product.StockQuantity;
                existingProduct.Discount = product.Discount;

                _context.Update(existingProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User);
            var product = await _context.Products
                            .FirstOrDefaultAsync(p => p.Id == id && p.SellerId == userId);

            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = _userManager.GetUserId(User);
            var product = await _context.Products
                            .FirstOrDefaultAsync(p => p.Id == id && p.SellerId == userId);

            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Orders()
        {
            var userId = _userManager.GetUserId(User);
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.OrderItems.Any(oi => oi.Product.SellerId == userId))
                .ToListAsync();

            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, string status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.Status = status;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Orders");
        }

        public async Task<IActionResult> Reviews()
        {
            var reviews = await _context.Reviews.Include(r => r.Product).Include(r => r.User).ToListAsync();
            return View(reviews);
        }

    }
}
