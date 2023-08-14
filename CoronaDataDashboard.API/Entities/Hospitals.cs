using System.ComponentModel;

namespace CoronaDataDashboard.API.Entities
{
    public enum Hospitals
    {
        [Description("ברזילי")]
        Barzilai,
        [Description("בני ציון")]
        BneiZion,
        [Description("הלל יפה")]
        HillelYaffe,
        [Description("וולפסון")]
        Wolfson,
        [Description("זיו")]
        Ziv,
        [Description("גליל")]
        Galilee,
        [Description("פוריה")]
        Poria,
        [Description("רמב\"ם")]
        Rambam,
        [Description("תל השומר")]
        TelHashomer,
        [Description("אסף הרופא")]
        AssafHarofeh,
        [Description("איכילוב")]
        Ichilov,
    }

    public static class EnumHelper
    {
        public static T GetEnumFromNumericValue<T>(int numericValue) where T : Enum
        {
            foreach (var enumValue in Enum.GetValues(typeof(T)))
            {
                if ((int)enumValue == numericValue)
                {
                    return (T)enumValue;
                }
            }

            throw new ArgumentException($"Enum value with numeric value '{numericValue}' not found.", nameof(numericValue));
        }
    }
}
