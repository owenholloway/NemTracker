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
        
        public Station Station { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=172.16.40.100;Database=nemtracker.test;" +
                           "Username=nemtracker.test;Password=DsF82VQZ8ZqizkDhdxTHjE2mqfBeDdzL");
        
    }
}