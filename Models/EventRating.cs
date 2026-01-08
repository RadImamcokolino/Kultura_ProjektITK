using System.ComponentModel.DataAnnotations;

namespace YourApp.Models
{
    public class EventRating
    {
        public int Id { get; set; }

        [Required]
        public int EventId { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Range(1, 5)]
        [Required]
        public int Stars { get; set; }

        [StringLength(2000)]
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}