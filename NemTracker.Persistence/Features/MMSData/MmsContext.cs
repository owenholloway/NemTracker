using Microsoft.EntityFrameworkCore;
using NemTracker.Model.Model.MmsData.Dispatch;

namespace NemTracker.Persistence.Features.MMSData
{
    public class MmsDbContext : DbContext
    {
        public MmsDbContext(MmsDbContextContainer container) : base(container.ContextOptions)
        {
        }
        
        private static void BuildApplicationModel(ModelBuilder modelBuilder)
        {
        }
        
        public DbSet<DispatchLoad> dispatchload { get; set; }
        
        
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