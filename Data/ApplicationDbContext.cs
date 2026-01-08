using Microsoft.EntityFrameworkCore;
using YourApp.Models;

namespace YourApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<EventRating> EventRatings => Set<EventRating>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EventRating>()
                .HasIndex(r => new { r.EventId, r.UserId })
                .IsUnique(); // One rating per user per event

            modelBuilder.Entity<EventRating>()
                .Property(r => r.Comment)
                .HasMaxLength(2000);
        }
    }
}