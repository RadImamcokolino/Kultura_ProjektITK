using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using YourApp.Data;
using YourApp.Models;

namespace YourApp.Pages.Shared
{
    public class _EventRatingsPartialModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public _EventRatingsPartialModel(ApplicationDbContext db) { _db = db; }

        public int EventId { get; set; }
        public double Average { get; set; }
        public int Count { get; set; }
        public List<EventRating> Ratings { get; set; } = new();

        public async Task OnGetAsync(int eventId)
        {
            EventId = eventId;
            Ratings = await _db.EventRatings
                .Where(r => r.EventId == eventId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            Count = Ratings.Count;
            Average = Count == 0 ? 0 : Math.Round(Ratings.Average(r => r.Stars), 1);
        }
    }
}