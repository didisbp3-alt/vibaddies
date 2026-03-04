using Microsoft.AspNetCore.Mvc;
using MVC_Buddies.Dtos;
using MVC_Buddies.Services;

namespace MVC_Buddies.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        // =============================
        // List reviews for a PetSitter
        // =============================
        public async Task<IActionResult> Index(int? petSitterId)
        {
            var reviews = petSitterId.HasValue
                ? await _reviewService.GetReviewsByPetSitterAsync(petSitterId.Value)
                : new List<ReviewDto>();

            var model = new ReviewListViewModel
            {
                Reviews = reviews,
                PetSitterId = petSitterId
            };

            return View(model);
        }

        // =============================
        // Create review
        // =============================
        public IActionResult Create(int bookingId)
        {
            var model = new CreateReviewDto { BookingId = bookingId };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateReviewDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _reviewService.CreateReviewAsync(model);
            return RedirectToAction("Index", new { petSitterId = null });
        }

        // =============================
        // Delete review
    
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _reviewService.DeleteReviewAsync(id);
            return RedirectToAction("Index");
        }
    }
}
