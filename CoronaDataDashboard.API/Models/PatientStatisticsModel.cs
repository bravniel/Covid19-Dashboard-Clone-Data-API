namespace CoronaDataDashboard.API.Models
{
    public class PatientStatisticsModel
    {
        public DateTime Date { get; set; }
        public int NewUnhealthyPeopleYesterday { get; set; }
            public int TotalUnhealthyPeople { get; set; }
            public int TotalUnhealthyPatients { get; set; }
            public int TotalUnhealthyHospitalized { get; set; }
            public int TotalSeriousHospitalized { get; set; }
            public int TotalCriticalHospitalized { get; set; }
            public int TotalModerateHospitalized { get; set; }
            public int TotalMildHospitalized { get; set; }
            public int TotalFirstDoseVaccinated { get; set; }
            public int TotalSecondDoseVaccinated { get; set; }
            public int TotalThirdDoseVaccinated { get; set; }
            public int TotalFourthDoseVaccinated { get; set; }
            public int TotalOmicronVaccinated { get; set; }
            public int TotalDeadPeople { get; set; }
            public double PercentageUnhealthyYesterday { get; set; }
    }
}
