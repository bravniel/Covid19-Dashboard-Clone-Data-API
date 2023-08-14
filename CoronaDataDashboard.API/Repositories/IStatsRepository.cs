using CoronaDataDashboard.API.Models;

namespace CoronaDataDashboard.API.Repositories
{
    public interface IStatsRepository
    {
        Task<Guid?> AddNewStat(NewStatModel newStatModel);
        Task<List<StatsModel>> GetAllStatesAsync();
        Task<(int newPatientsYesterday, int totalUnhealthyPeople)> GetPatientStatisticsAsync();
        Task<StatisticsViewOn> CalculatePatientStatisticsAsync();
        Task<DateTime> LatestStatesDateAsync();
        Task<List<MorbidityFromAbroadModel>> GetAllMorbidityFromAbroadAsync();
        Task<List<HospitalModel>> AddHospitalsData();
        //Task<List<HospitalModel>> GetHospitalsDataForLatestDate();
        Task<List<Dictionary<string, object>>> GetHospitalsDataForLatestDate();
        Task<List<SettlementModel>> CalculateSettlementsStatisticsAsync();
        //Task<List<CountryModel>> CalculateMorbiditySumAsync(int days = 90);
        Task<List<Dictionary<string, object>>> CalculateMorbiditySumAsync(int days = 90);

    }
}
