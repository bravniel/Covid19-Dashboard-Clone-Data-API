using System.ComponentModel;

namespace CoronaDataDashboard.API.Entities
{
    public enum RiskLevels
    {
        [Description("מירבי")]
        Maximum,
        [Description("בינוני")]
        Moderate,
        [Description("נמוך")]
        Low,
    }
}
