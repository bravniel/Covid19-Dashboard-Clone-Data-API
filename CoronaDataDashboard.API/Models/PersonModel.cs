using CoronaDataDashboard.API.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoronaDataDashboard.API.Models
{
    public class PersonModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        //public StatsModel TestDate { get; set; }
        public ICollection<StatsModel> TestDate { get; set; }

        public string? FullName { get; set; }
        [Required]
        public Gender Gender { get; set; }
        [Required]
        public IsraelSettlements City { get; set; }
        [Required]
        public SickCondition SickCondition { get; set; }
        [Required]
        public bool IsFirstDoseVaccinated { get; set; }
        [Required]
        public bool IsSecondDoseVaccinated { get; set; }
        [Required]
        public bool IsThirdDoseVaccinated { get; set; }
        [Required]
        public bool IsFourthDoseVaccinated { get; set; }
        [Required]
        public bool IsOmicronVaccinated { get; set; }
        [Required]
        public bool IsHospitalized { get; set; }
        [Required]
        public bool IsDeadPatients { get; set; }
    }
}