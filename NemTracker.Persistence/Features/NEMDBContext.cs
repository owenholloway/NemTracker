using Microsoft.EntityFrameworkCore;
using NemTracker.Model.Stations;

namespace NemTracker.Persistence.Features
{
    // ReSharper disable once InconsistentNaming
    public class NEMDBContext : DbContext
    {
        public NEMDBContext(DbContextOptions options) : base(options)
        {
            
        }
        
        public DbSet<Station> Stations { get; set; }

        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=172.16.40.100;Database=nemtracker.test;" +
                           "Username=nemtracker.test;Password=@Password123@")
                .UseSnakeCaseNamingConvention();

    }
}