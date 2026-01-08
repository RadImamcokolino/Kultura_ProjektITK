using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using YourApp.Data;
using YourApp.Models;

namespace YourApp.Pages.Events
{
    [Authorize] // require login
    public class RateModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public RateModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty(SupportsGet = true)]
        public int EventId { get; set; }

        public string? EventTitle { get; set; }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Range(1, 5, ErrorMessage = "Izberite oceno med 1 in 5 zvezdic.")]
            [Required]
            public int Stars { get; set; }

            [StringLength(2000, ErrorMessage = "Komentar je predolg.")]
            public string? Comment { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // TODO: Load event info (title) from your events table.
            EventTitle = $"Dogodek #{EventId}";

            // Optional: verify user attended the event before allowing rating
            // if (!await UserAttended(EventId, UserId)) return Forbid();

            // If already rated, preload for edit
            var userId = User.Identity?.Name ?? string.Empty;
            var existing = await _db.EventRatings.FirstOrDefaultAsync(r => r.EventId == EventId && r.UserId == userId);
            if (existing != null)
            {
                Input.Stars = existing.Stars;
                Input.Comment = existing.Comment;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var userId = User.Identity?.Name ?? string.Empty;

            var existing = await _db.EventRatings.FirstOrDefaultAsync(r => r.EventId == EventId && r.UserId == userId);
            if (existing == null)
            {
                var rating = new EventRating
                {
                    EventId = EventId,
                    UserId = userId,
                    Stars = Input.Stars,
                    Comment = Input.Comment
                };
                _db.EventRatings.Add(rating);
            }
            else
            {
                existing.Stars = Input.Stars;
                existing.Comment = Input.Comment;
                existing.CreatedAt = DateTime.UtcNow;
            }

            await _db.SaveChangesAsync();

            // Redirect back to event details
            return RedirectToPage("/Events/Details", new { id = EventId });
        }
    }
}