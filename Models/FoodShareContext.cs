using Microsoft.EntityFrameworkCore;

namespace FoodShare.Models
{
    public class FoodShareContext : DbContext
    {
        public FoodShareContext(
            DbContextOptions<FoodShareContext> options)
            : base(options)
        {
        }

        public DbSet<Donation> Donations { get; set; }
    }
}