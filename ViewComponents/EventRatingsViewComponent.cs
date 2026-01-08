using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourApp.Data;
using YourApp.Models;

namespace YourApp.ViewComponents
{
    public class EventRatingsViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _db;

        public EventRatingsViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }

        public class EventRatingsViewModel
        {
            public int EventId { get; set; }
            public double Average { get; set; }
            public int Count { get; set; }
            public List<EventRating> Ratings { get; set; } = new();
        }

        public async Task<IViewComponentResult> InvokeAsync(int eventId)
        {
            var ratings = await _db.EventRatings
                .Where(r => r.EventId == eventId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            var vm = new EventRatingsViewModel
            {
                EventId = eventId,
                Ratings = ratings,
                Count = ratings.Count,
                Average = ratings.Count == 0 ? 0 : Math.Round(ratings.Average(r => r.Stars), 1)
            };

            return View(vm);
        }
    }
}