using Microsoft.EntityFrameworkCore;
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
        
        public DbSet<Station> Stations { get; set; }
        public DbSet<Participant> Participants { get; set; }
        
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