using Microsoft.EntityFrameworkCore;
using NemTracker.Model.P5Minute;
using NemTracker.Model.Stations;

namespace NemTracker.Persistence.Features
{
    // ReSharper disable once InconsistentNaming
    public class NEMDBContext : DbContext
    {
        public NEMDBContext(DbContextOptions options) : base(options)
        {}

        private static void BuildApplicationModel(ModelBuilder modelBuilder)
        {
        }
        
        //Station and General NEM Data
        public DbSet<Station> Stations { get; set; }
        public DbSet<Participant> Participants { get; set; }
        
        //P5 Data
        public DbSet<RegionSolution> RegionSolutions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            BuildApplicationModel(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=172.16.40.100;Database=nemtracker.test;" +
                           "Username=nemtracker.test;Password=@Password123@")
                .UseSnakeCaseNamingConvention();

    }
}