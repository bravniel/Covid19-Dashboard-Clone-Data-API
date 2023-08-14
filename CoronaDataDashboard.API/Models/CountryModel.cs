using CoronaDataDashboard.API.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoronaDataDashboard.API.Models
{
    public class CountryModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Countries CountryName { get; set; }
        public ICollection<MorbidityFromAbroadModel> MorbidityFromAbroad { get; set; }
        public RiskLevels RiskLevel { get; set; }
        public int NumberOfEnteringPeopleToIsrael { get; set; }
        public int NumberOfVerifiedCitizens { get; set; }
        public int NumberOfVerifiedForeigners { get; set; }
        public double PercentageOfVerifiedArrivals { get; set; }
    }
}
