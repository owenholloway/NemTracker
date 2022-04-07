using System.ComponentModel;

namespace NemTracker.Dtos.Aemo
{
    public enum CategoryEnum
    {
        [Description("Undefined")]
        Undefined = -1,
        [Description("Market")]
        Market = 1,
        [Description("Non-Market")]
        NonMarket = 2
    }
}