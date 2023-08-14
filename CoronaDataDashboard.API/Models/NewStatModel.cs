using System.ComponentModel.DataAnnotations;

namespace CoronaDataDashboard.API.Models
{
    public class NewStatModel
    {
        [Required]
        public DateTime Date { get; set; }
    }
}
