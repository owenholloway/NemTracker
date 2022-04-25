using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NemTracker.Model.Model.Reports;
using NemTracker.Model.Model.Stations;

namespace NemTracker.Persistence.Features
{
    // ReSharper disable once InconsistentNaming
    public class NEMDBContext : DbContext
    {

        public NEMDBContext(DbContextOptions options) : base(options)
        {
        }

        private static void BuildApplicationModel(ModelBuilder modelBuilder)
        {
        }
        
        //Station and General NEM Data
        public DbSet<Station> Stations { get; set; }
        public DbSet<Participant> Participants { get; set; }
        
        //P5 Data
        public DbSet<RegionSolution> RegionSolutions { get; set; }
        public DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            BuildApplicationModel(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSnakeCaseNamingConvention();
        }
    }
}