using CoronaDataDashboard.API.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoronaDataDashboard.API.Models
{
    public class SettlementModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string SettlementName { get; set; }
        [Required]
        public double ScoreAccordingToTrafficLightPlan { get; set; }
        [Required]
        public double NewVerifiedPatients { get; set; }
        [Required]
        public double PercentageOfPositiveTests { get; set; }
        [Required]
        public double VerifiedChangeRate { get; set; }
        [Required]
        public int ActivePatients { get; set; }
    }
}
