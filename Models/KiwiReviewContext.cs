using Kiwi_review.Models.DatabaseModel;
using Microsoft.EntityFrameworkCore;

namespace Kiwi_review.Models
{
    public class KiwiReviewContext : DbContext
    {
        public KiwiReviewContext(DbContextOptions<KiwiReviewContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Highlight> Highlights { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Movie> Movies { get; set; }
    }
}