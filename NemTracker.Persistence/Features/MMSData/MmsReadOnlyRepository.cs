using Microsoft.EntityFrameworkCore;
using NemTracker.Persistence.Interfaces;
using Oxygen.Features;

namespace NemTracker.Persistence.Features.MMSData
{
    public class MmsReadOnlyRepository : ReadOnlyRepository, IMmsReadOnlyRepository
    {
        public MmsReadOnlyRepository(MmsDbContext context) : base(context)
        {
            
        }
    }
}