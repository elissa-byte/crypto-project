using ElissaSaliba_RalphBouAntoun_HybridCryptoSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ElissaSaliba_RalphBouAntoun_HybridCryptoSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Messages> Messages { get; set; }
    }
}
