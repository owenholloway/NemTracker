using System;

namespace NemTracker.Dtos.P5Minute
{
    public enum AemoTypeEnum : short
    {
        LocalPrice = 1,
        CaseSolution = 2,
        InterconnectorSolution = 4,
        ConstraintSolution = 6,
        RegionSolution = 7
    }
}