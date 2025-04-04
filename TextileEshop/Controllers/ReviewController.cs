using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TextileEshop.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using TextileEshop.ViewModels;

namespace TextileEshop.Controllers
{
    public class ReviewController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ReviewController> _logger;

        public ReviewController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<ReviewController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddReview(ReviewInputViewModel input)
        {
            _logger.LogInformation("AddReview triggered for product ID: {ProductId}", input.ProductId);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid for ReviewInputViewModel:");
                foreach (var entry in ModelState)
                {
                    foreach (var error in entry.Value.Errors)
                    {
                        _logger.LogWarning(" - {Key}: {Error}", entry.Key, error.ErrorMessage);
                    }
                }

                return RedirectToAction("Details", "Home", new { id = input.ProductId });
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _logger.LogError("User not found or not logged in.");
                return Unauthorized();
            }

            var review = new Review
            {
                ProductId = input.ProductId,
                UserId = user.Id,
                Rating = input.Rating,
                Comment = input.Comment,
                Date = DateTime.Now
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Review saved successfully for product ID: {ProductId}", input.ProductId);
            return RedirectToAction("Details", "Home", new { id = input.ProductId });
        }

    }
}
