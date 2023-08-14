namespace CoronaDataDashboard.API.Models
{
    public class CardData
    {
        public string CardId { get; set; }
        public double?  HeaderNumber { get; set; }
        public List<LineData> SubDataLines { get; set; }
    }
}
