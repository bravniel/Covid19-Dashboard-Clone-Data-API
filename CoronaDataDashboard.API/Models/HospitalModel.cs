using CoronaDataDashboard.API.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoronaDataDashboard.API.Models
{
    public class HospitalModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public Hospitals HospitalName { get; set; }
        [Required]
        public double OverallBedOccupancyPercentage { get; set; }
        [Required]
        public double PercentageOfIndoorWardBedOccupancy { get; set; }

    }
}
