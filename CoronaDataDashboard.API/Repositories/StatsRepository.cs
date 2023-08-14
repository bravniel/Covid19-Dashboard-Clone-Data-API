using CoronaDataDashboard.API.Data;
using CoronaDataDashboard.API.Entities;
using CoronaDataDashboard.API.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Reflection;

namespace CoronaDataDashboard.API.Repositories
{
    public class StatsRepository : IStatsRepository
    {
        private readonly CoronaDataDashboardContext _coronaDataDashboardContext;
        private readonly Random _random;

        public StatsRepository(CoronaDataDashboardContext coronaDataDashboardContext)
        {
            _coronaDataDashboardContext = coronaDataDashboardContext;
            _random = new Random();
        }
        public async Task<Guid?> AddNewStat(NewStatModel newStatModel)
        {
            StatsModel statsModel = new()
            {
                Date = newStatModel.Date
            };

            _coronaDataDashboardContext.States.Add(statsModel);
            var res = await _coronaDataDashboardContext.SaveChangesAsync();
            if (res != 0)
            {
                return statsModel.Id;
            }
            return null;
        }
        public async Task<List<StatsModel>> GetAllStatesAsync()
        {
            var stats = await _coronaDataDashboardContext.States.Include(s => s.People).ToListAsync();
            if (stats != null && stats.Count != 0)
            {
                return stats;
            }

            var listOfPeople = await AddStatsModelsForYearAsync();
            return listOfPeople;
        }

        private async Task<List<StatsModel>> AddStatsModelsForYearAsync()
        {
            List<StatsModel> statsList = new();

            DateTime currentDate = DateTime.Now;
            DateTime oneYearBack = currentDate.AddYears(-1);

            DateTime dateToAdd = oneYearBack;
            while (dateToAdd <= currentDate)
            {
                StatsModel stat = new()
                {
                    Date = dateToAdd,
                };

                _coronaDataDashboardContext.States.Add(stat);
                int result = await _coronaDataDashboardContext.SaveChangesAsync();

                if (result == 0)
                {
                    return new List<StatsModel>(); // Failed to save the stat object - Return an empty list if no stats found
                }

                dateToAdd = dateToAdd.AddDays(1);
                statsList.Add(stat);
            }
            return statsList; // All days were added successfully

        }

        public async Task<(int newPatientsYesterday, int totalUnhealthyPeople)> GetPatientStatisticsAsync()
        {
            ////DateTime yesterday = DateTime.Today.AddDays(-1);
            DateTime yesterday = DateTime.Parse("2023-06-01 20:09:15.8213581");


            int newPatientsYesterday = await _coronaDataDashboardContext.People
                .CountAsync(p => p.TestDate.Any(stat => stat.Date.Date == yesterday));

            int totalUnhealthyPeople = await _coronaDataDashboardContext.People
                .CountAsync(p => p.SickCondition != SickCondition.Healthy);

            return (newPatientsYesterday, totalUnhealthyPeople);
        }
        public async Task<StatisticsViewOn> CalculatePatientStatisticsAsync()
        {
            //DateTime yesterday = DateTime.Today.AddDays(-1);
            DateTime yesterday = DateTime.Parse("2023-05-10 00:00:00");
            DateTime today = DateTime.Parse("2023-05-11 00:00:00");
            
            // var date = await FindDateWithMaxNewUnhealthyPeopleAsync();
            //
            int numberOfNewVerifiedYesterday = await _coronaDataDashboardContext.People
                    .CountAsync(p => p.SickCondition != SickCondition.Healthy && p.TestDate.Min(stat => stat.Date.Date) == yesterday && p.TestDate.First().Date.Date == yesterday);
            int numberOfNewVerifiedToday = await _coronaDataDashboardContext.People
                    .CountAsync(p => p.SickCondition != SickCondition.Healthy && p.TestDate.Min(stat => stat.Date.Date) == today && p.TestDate.First().Date.Date == today);
            int totalNumberOfVerified = await _coronaDataDashboardContext.States.SelectMany(s => s.People).Where(p => p.SickCondition != SickCondition.Healthy).Select(p => p.Id).Distinct().CountAsync();
            //
            int totalNumberOfActivePatients = await _coronaDataDashboardContext.People.Where(p => p.TestDate.OrderByDescending(td => td.Date).FirstOrDefault() != null && p.SickCondition != SickCondition.Healthy).CountAsync();
            int numberOfActivePatientsHospitalized = await _coronaDataDashboardContext.People.Where(p => p.TestDate.OrderByDescending(td => td.Date).FirstOrDefault() != null && p.SickCondition != SickCondition.Healthy && p.IsHospitalized).CountAsync();
            //
            int numberOfHospitalizedPatientsInSevereAndCriticalCondition = await _coronaDataDashboardContext.People.Where(p => p.TestDate.OrderByDescending(td => td.Date).FirstOrDefault() != null && (p.SickCondition == SickCondition.Serious || p.SickCondition == SickCondition.Critical) && p.IsHospitalized).CountAsync();
            int numberOfHospitalizedPatientsInCriticalCondition = await _coronaDataDashboardContext.People.Where(p => p.TestDate.OrderByDescending(td => td.Date).FirstOrDefault() != null && p.SickCondition == SickCondition.Critical && p.IsHospitalized).CountAsync();
            int numberOfHospitalizedPatientsConnectedToACMO = await _coronaDataDashboardContext.People.Where(p => p.TestDate.OrderByDescending(td => td.Date).FirstOrDefault() != null && p.SickCondition == SickCondition.ECMOConnected && p.IsHospitalized).CountAsync();
            int numberOfHospitalizedPatientsVentilated = await _coronaDataDashboardContext.People.Where(p => p.TestDate.OrderByDescending(td => td.Date).FirstOrDefault() != null && p.SickCondition == SickCondition.Vantilated && p.IsHospitalized).CountAsync();
            int numberOfHospitalizedPatientsInModerateCondition = await _coronaDataDashboardContext.People.Where(p => p.TestDate.OrderByDescending(td => td.Date).FirstOrDefault() != null && p.SickCondition == SickCondition.Moderate && p.IsHospitalized).CountAsync();
            int numberOfHospitalizedPatientsInMildCondition = await _coronaDataDashboardContext.People.Where(p => p.TestDate.OrderByDescending(td => td.Date).FirstOrDefault() != null && p.SickCondition == SickCondition.Mild && p.IsHospitalized).CountAsync();
            //
            int numberOfPeopleGetVaccinatedInFirstDose = await _coronaDataDashboardContext.People.CountAsync(p => p.IsFirstDoseVaccinated);
            int numberOfPeopleGetVaccinatedInSecondDose = await _coronaDataDashboardContext.People.CountAsync(p => p.IsSecondDoseVaccinated);
            int numberOfPeopleGetVaccinatedInThirdDose = await _coronaDataDashboardContext.People.CountAsync(p => p.IsThirdDoseVaccinated);
            int numberOfPeopleGetVaccinatedInFourthDose = await _coronaDataDashboardContext.People.CountAsync(p => p.IsFourthDoseVaccinated);
            int numberOfPeopleGetVaccinatedInOmicron = await _coronaDataDashboardContext.People.CountAsync(p => p.IsOmicronVaccinated);
            //
            int cumulativeNumberOfDeceased = await _coronaDataDashboardContext.People.CountAsync(p => p.IsDeadPatients);
            //
            int numberOfNewTestsForVirusDetectionYesterday = await _coronaDataDashboardContext.People.CountAsync(p => (p.TestDate.Count == 1 && p.TestDate.First().Date.Date == yesterday) || (p.TestDate.Min(stat => stat.Date.Date) == yesterday && p.TestDate.First().Date.Date == yesterday));
            double percentageNumberOfPositiveTestsYesterday;
            if (numberOfNewTestsForVirusDetectionYesterday > 0) percentageNumberOfPositiveTestsYesterday = (numberOfNewVerifiedYesterday / numberOfNewTestsForVirusDetectionYesterday) * 100;
            else percentageNumberOfPositiveTestsYesterday = 0;
            int totalNumberOfTestsYesterday = await _coronaDataDashboardContext.States.Where(s => s.Date == yesterday).SelectMany(s => s.People).CountAsync();
            //
            //
            DateTime endDate = yesterday.AddDays(-1); // Get the specific date from the parameter
            DateTime startDate = endDate.AddDays(-6); // Calculate the start date (7 days ago)
            DateTime previousEndDate = startDate.AddDays(-1);
            DateTime previousStartDate = previousEndDate.AddDays(-6);

            // int numberOfNewVerifiedPeopleInLast7Days = await _coronaDataDashboardContext.People.CountAsync(p => (p.TestDate.Count == 1 && p.TestDate.First().Date.Date >= startDate && p.TestDate.First().Date.Date <= endDate) || (p.TestDate.Min(stat => stat.Date.Date) >= startDate && p.TestDate.Min(stat => stat.Date.Date) <= endDate && p.TestDate.First().Date.Date >= startDate && p.TestDate.First().Date.Date <= endDate));
            int numberOfNewVerifiedPeopleInLast7Days = await _coronaDataDashboardContext.People.CountAsync(p => p.TestDate.Min(stat => stat.Date.Date) >= startDate && p.TestDate.Min(stat => stat.Date.Date) <= endDate && p.TestDate.First().Date.Date >= startDate && p.TestDate.First().Date.Date <= endDate);
            int numberOfNewVerifiedPeopleInPrevious7DaysFromLast7Days = await _coronaDataDashboardContext.People.CountAsync(p => p.TestDate.Min(stat => stat.Date.Date) >= previousStartDate && p.TestDate.Min(stat => stat.Date.Date) <= previousEndDate && p.TestDate.First().Date.Date >= previousStartDate && p.TestDate.First().Date.Date <= previousEndDate);
            double percentageChangeOfNewVerifiedPeopleFromPrevious7Days;
            if (numberOfNewVerifiedPeopleInPrevious7DaysFromLast7Days > 0) percentageChangeOfNewVerifiedPeopleFromPrevious7Days = ((numberOfNewVerifiedPeopleInLast7Days - numberOfNewVerifiedPeopleInPrevious7DaysFromLast7Days) / numberOfNewVerifiedPeopleInLast7Days) * 100;
            else percentageChangeOfNewVerifiedPeopleFromPrevious7Days = 0;
            //
            int numberOfNewHospitalizedPatientsInSeriousAndCriticalConditionInLast7Days = await _coronaDataDashboardContext.People.Where(p => p.TestDate.Any(td => td.Date >= startDate && td.Date <= endDate) && (p.SickCondition == SickCondition.Serious || p.SickCondition == SickCondition.Critical) && p.IsHospitalized).CountAsync();
            int numberOfNewHospitalizedPatientsInSeriousAndCriticalConditionInPrevious7DaysFromLast7Days = await _coronaDataDashboardContext.People.Where(p => p.TestDate.Any(td => td.Date >= previousStartDate && td.Date <= previousEndDate) && (p.SickCondition == SickCondition.Serious || p.SickCondition == SickCondition.Critical) && p.IsHospitalized).CountAsync();
            double percentageChangeOfNewPatientsHospitalizedInSeriousAndCriticalConditionFromPrevious7Days;
            if (numberOfNewHospitalizedPatientsInSeriousAndCriticalConditionInLast7Days > 0) percentageChangeOfNewPatientsHospitalizedInSeriousAndCriticalConditionFromPrevious7Days = ((numberOfNewHospitalizedPatientsInSeriousAndCriticalConditionInLast7Days - numberOfNewHospitalizedPatientsInSeriousAndCriticalConditionInPrevious7DaysFromLast7Days) / numberOfNewHospitalizedPatientsInSeriousAndCriticalConditionInLast7Days) * 100;
            else percentageChangeOfNewPatientsHospitalizedInSeriousAndCriticalConditionFromPrevious7Days = 0;
            //
            int numberOfDeathsInLast7Days = await _coronaDataDashboardContext.People.Where(p => p.TestDate.Any(td => td.Date >= startDate && td.Date <= endDate) && p.IsDeadPatients).CountAsync();
            int numberOfDeathsInPrevious7DaysFromLast7Days = await _coronaDataDashboardContext.People.Where(p => p.TestDate.Any(td => td.Date >= previousStartDate && td.Date <= previousEndDate) && p.IsDeadPatients).CountAsync();
            double percentageChangeOfDeathsFromPrevious7Days;
            if (numberOfDeathsInLast7Days > 0) percentageChangeOfDeathsFromPrevious7Days = ((numberOfDeathsInLast7Days - numberOfDeathsInPrevious7DaysFromLast7Days) / numberOfDeathsInLast7Days) * 100;
            else percentageChangeOfDeathsFromPrevious7Days = 0;
            //
            int numberOfNewTastedPatientsInLast7Days = await _coronaDataDashboardContext.People.CountAsync(p => p.TestDate.Min(stat => stat.Date.Date) >= startDate && p.TestDate.Min(stat => stat.Date.Date) <= endDate && p.TestDate.First().Date.Date >= startDate && p.TestDate.First().Date.Date <= endDate);
            // int numberOfNewTastedPatientsInLast7Days = await _coronaDataDashboardContext.People.CountAsync(p => p.TestDate.Min(td => td.Date) >= startDate && p.TestDate.First().Date <= endDate);
            int numberOfNewTastedPatientsInPrevious7DaysFromLast7Days = await _coronaDataDashboardContext.People.CountAsync(p => p.TestDate.Min(stat => stat.Date.Date) >= previousStartDate && p.TestDate.Min(stat => stat.Date.Date) <= previousEndDate && p.TestDate.First().Date.Date >= previousStartDate && p.TestDate.First().Date.Date <= previousEndDate);
            double percentageChangeOfNewTastedPatientsFromPrevious7Days;
            if(numberOfNewTastedPatientsInLast7Days > 0) percentageChangeOfNewTastedPatientsFromPrevious7Days = ((numberOfNewTastedPatientsInLast7Days - numberOfNewTastedPatientsInPrevious7DaysFromLast7Days) / numberOfNewTastedPatientsInLast7Days) * 100;
            else percentageChangeOfNewTastedPatientsFromPrevious7Days = 0;
            int numberOfNewVerifiedPatientsInLast7Days = await _coronaDataDashboardContext.People.CountAsync(p => p.SickCondition != SickCondition.Healthy && p.TestDate.Min(stat => stat.Date.Date) >= previousStartDate && p.TestDate.Min(stat => stat.Date.Date) <= previousEndDate && p.TestDate.First().Date.Date >= previousStartDate && p.TestDate.First().Date.Date <= previousEndDate);
            double percentageChangeOfNewPositiveTastedPatientsFromPrevious7Days;
            if(numberOfNewTastedPatientsInLast7Days > 0) percentageChangeOfNewPositiveTastedPatientsFromPrevious7Days = (numberOfNewVerifiedPatientsInLast7Days / numberOfNewTastedPatientsInLast7Days) * 100;
            else percentageChangeOfNewPositiveTastedPatientsFromPrevious7Days = 0;
            //

            var statisticsViewOn = new StatisticsViewOn
            {
                Yesterday = new List<CardData>
    {
        new CardData
        {
            CardId = "verifiedYesterday",
            HeaderNumber = numberOfNewVerifiedYesterday,
            SubDataLines = new List<LineData>
            {
                new LineData
                {
                    LineId = "numberOfNewVerifiedToday",
                    LineNumber = numberOfNewVerifiedToday
                },
                new LineData
                {
                    LineId = "totalNumberOfVerified",
                    LineNumber = totalNumberOfVerified
                }
            }
        },
        new CardData
        {
            CardId = "activePatients",
            HeaderNumber = totalNumberOfActivePatients,
            SubDataLines = new List<LineData>
            {
                new LineData
                {
                    LineId = "numberOfActivePatientsHospitalized",
                    LineNumber = numberOfActivePatientsHospitalized
                }
            }
        },
        new CardData
        {
            CardId = "seriousPatients",
            HeaderNumber = numberOfHospitalizedPatientsInSevereAndCriticalCondition,
            SubDataLines = new List<LineData>
            {
                new LineData
                {
                    LineId = "numberOfHospitalizedPatientsInCriticalCondition",
                    LineNumber = numberOfHospitalizedPatientsInCriticalCondition
                },
                new LineData
                {
                    LineId = "numberOfHospitalizedPatientsConnectedToACMO",
                    LineNumber = numberOfHospitalizedPatientsConnectedToACMO
                },
                new LineData
                {
                    LineId = "numberOfHospitalizedPatientsVentilated",
                    LineNumber = numberOfHospitalizedPatientsVentilated
                },
                new LineData
                {
                    LineId = "numberOfHospitalizedPatientsInModerateCondition",
                    LineNumber = numberOfHospitalizedPatientsInModerateCondition
                },
                new LineData
                {
                    LineId = "numberOfHospitalizedPatientsInMildCondition",
                    LineNumber = numberOfHospitalizedPatientsInMildCondition
                }
            }
        },
        new CardData
        {
            CardId = "vaccinated",
            SubDataLines = new List<LineData>
            {
                new LineData
                {
                    LineId = "numberOfPeopleGetVaccinatedInFirstDose",
                    LineNumber = numberOfPeopleGetVaccinatedInFirstDose
                },
                new LineData
                {
                    LineId = "numberOfPeopleGetVaccinatedInSecondDose",
                    LineNumber = numberOfPeopleGetVaccinatedInSecondDose
                },
                new LineData
                {
                    LineId = "numberOfPeopleGetVaccinatedInThirdDose",
                    LineNumber = numberOfPeopleGetVaccinatedInThirdDose
                },
                new LineData
                {
                    LineId = "numberOfPeopleGetVaccinatedInFourthDose",
                    LineNumber = numberOfPeopleGetVaccinatedInFourthDose
                },
                new LineData
                {
                    LineId = "numberOfPeopleGetVaccinatedInOmicron",
                    LineNumber = numberOfPeopleGetVaccinatedInOmicron
                }
            }
        },
        new CardData
        {
            CardId = "deceasedAccumulate",
            HeaderNumber = cumulativeNumberOfDeceased
        },
        new CardData
        {
            CardId = "percentageOfPositiveTestsYesterday",
            HeaderNumber = percentageNumberOfPositiveTestsYesterday,
            SubDataLines = new List<LineData>
            {
                new LineData
                {
                    LineId = "numberOfNewTestsForVirusDetectionYesterday",
                    LineNumber = numberOfNewTestsForVirusDetectionYesterday
                },
                new LineData
                {
                    LineId = "totalNumberOfTestsYesterday",
                    LineNumber = totalNumberOfTestsYesterday
                }
            }
        }
    },
                SummaryOfTheLast7Days = new List<CardData>
    {
        new CardData
        {
            CardId = "amountOfNewVerifiedPeopleInLast7Days",
            HeaderNumber = numberOfNewVerifiedPeopleInLast7Days,
            SubDataLines = new List<LineData>
            {
                new LineData
                {
                    LineId = "percentageChangeOfNewVerifiedPeopleFromPrevious7Days",
                    LineNumber = percentageChangeOfNewVerifiedPeopleFromPrevious7Days
                }
            }
        },
        new CardData
        {
            CardId = "amountOfNewHospitalizedPatientsInSeriousAndCriticalConditionInLast7Days",
            HeaderNumber = numberOfNewHospitalizedPatientsInSeriousAndCriticalConditionInLast7Days,
            SubDataLines = new List<LineData>
            {
                new LineData
                {
                    LineId = "percentageChangeOfNewPatientsHospitalizedInSeriousAndCriticalConditionFromPrevious7Days",
                    LineNumber = percentageChangeOfNewPatientsHospitalizedInSeriousAndCriticalConditionFromPrevious7Days
                }
            }
        },
        new CardData
        {
            CardId = "amountOfDeathsInLast7Days",
            HeaderNumber = numberOfDeathsInLast7Days,
            SubDataLines = new List<LineData>
            {
                new LineData
                {
                    LineId = "percentageChangeOfDeathsFromPrevious7Days",
                    LineNumber = percentageChangeOfDeathsFromPrevious7Days
                }
            }
        },
        new CardData
        {
            CardId = "amountOfNewTastedPatientsInLast7Days",
            HeaderNumber = numberOfNewTastedPatientsInLast7Days,
            SubDataLines = new List<LineData>
            {
                new LineData
                {
                    LineId = "percentageChangeOfNewTastedPatientsFromPrevious7Days",
                    LineNumber = percentageChangeOfNewTastedPatientsFromPrevious7Days
                },
                new LineData
                {
                    LineId = "percentageChangeOfNewPositiveTastedPatientsFromPrevious7Days",
                    LineNumber = percentageChangeOfNewPositiveTastedPatientsFromPrevious7Days
                }
            }
        }
    }
            };

            return statisticsViewOn;
        }

        private async Task<DateTime> FindDateWithMaxNewUnhealthyPeopleAsync()
        {
            DateTime maxDate = DateTime.MinValue;
            int maxCount = 0;

            var datesWithCount = await _coronaDataDashboardContext.People
                .GroupBy(p => p.TestDate.Min(stat => stat.Date.Date))
                .Select(g => new { Date = g.Key, Count = g.Count(p => p.SickCondition != SickCondition.Healthy) })
                .ToListAsync();

            foreach (var item in datesWithCount)
            {
                if (item.Count > maxCount)
                {
                    maxCount = item.Count;
                    maxDate = item.Date;
                }
            }

            return maxDate;
        }

        public async Task<DateTime> LatestStatesDateAsync()
        {
            DateTime latestDate = await _coronaDataDashboardContext.States.OrderByDescending(s => s.Date).Select(s => s.Date).FirstOrDefaultAsync();
            return latestDate;
        }
        public async Task<List<MorbidityFromAbroadModel>> GetAllMorbidityFromAbroadAsync()
        {
            var listOfAllMorbidityFromAbroad = await _coronaDataDashboardContext.MorbidityFromAbroad.Include(u => u.Countries).ToListAsync();
            if (listOfAllMorbidityFromAbroad != null && listOfAllMorbidityFromAbroad.Count != 0)
            {
                return listOfAllMorbidityFromAbroad;
            }

            List<MorbidityFromAbroadModel> newListOfAllMorbidityFromAbroad = await GenerateRandomCountriesData();
            return newListOfAllMorbidityFromAbroad;
            /*if (people == null)
            {
            }
            return people.ToList();*/
        }
        //public async Task<List<(string CountryName, string RiskLevel, int NumberOfEnteringPeopleToIsrael, int NumberOfVerifiedCitizens, int NumberOfVerifiedForeigners, double PercentageOfVerifiedArrivals)>> CalculateMorbiditySumAsync(int days = 90)
        public async Task<List<Dictionary<string, object>>> CalculateMorbiditySumAsync(int days = 90)
        {
            DateTime currentDate = DateTime.Today;
            DateTime startDate = currentDate.AddDays(-days);

            var morbidityData = await _coronaDataDashboardContext.MorbidityFromAbroad
                .Where(m => m.Date >= startDate && m.Date <= currentDate)
                .SelectMany(m => m.Countries)
                .GroupBy(c => c.CountryName)
                .Select(g => new 
                {
                    CountryName = g.Key,
                    AvgRiskLevel = g.Average(c => (int)c.RiskLevel),
                    NumberOfEnteringPeopleToIsrael = g.Sum(c => c.NumberOfEnteringPeopleToIsrael),
                    NumberOfVerifiedCitizens = g.Sum(c => c.NumberOfVerifiedCitizens),
                    NumberOfVerifiedForeigners = g.Sum(c => c.NumberOfVerifiedForeigners),
                    PercentageOfVerifiedArrivals = Math.Round(g.Average(c => c.PercentageOfVerifiedArrivals), 2), 
                })
                .ToListAsync();

            return morbidityData.Select(m => new Dictionary<string, object>
            {
                { "CountryName", GetEnumDescription(m.CountryName) },
                { "RiskLevel", Enum.GetName(typeof(RiskLevels), (RiskLevels)m.AvgRiskLevel) },
                { "NumberOfEnteringPeopleToIsrael", m.NumberOfEnteringPeopleToIsrael },
                { "NumberOfVerifiedCitizens", m.NumberOfVerifiedCitizens },
                { "NumberOfVerifiedForeigners", m.NumberOfVerifiedForeigners },
                { "PercentageOfVerifiedArrivals", m.PercentageOfVerifiedArrivals }
            }).ToList();
            //return morbidityData.Select(m => (CountryName: m.CountryName, RiskLevel: Enum.GetName(typeof(RiskLevels), (RiskLevels)m.AvgRiskLevel), NumberOfEnteringPeopleToIsrael: m.NumberOfEnteringPeopleToIsrael, NumberOfVerifiedCitizens: m.NumberOfVerifiedCitizens, NumberOfVerifiedForeigners: m.NumberOfVerifiedForeigners, PercentageOfVerifiedArrivals: m.PercentageOfVerifiedArrivals)).ToList();
        }

        private async Task<List<MorbidityFromAbroadModel>> GenerateRandomCountriesData()
        {
            DateTime currentDate = DateTime.Today;
            DateTime oneYearAgo = currentDate.AddYears(-1);

            var listOfCountriesMorbidity = new List<MorbidityFromAbroadModel>();

            while (currentDate >= oneYearAgo)
            {
                var morbidityFromAbroad = new MorbidityFromAbroadModel
                {
                    Date = currentDate,
                    Countries = new List<CountryModel>()
                };

                foreach (Countries country in Enum.GetValues(typeof(Countries)))
                {
                    var countryModel = new CountryModel
                    {
                        CountryName = country,
                        RiskLevel = (RiskLevels)_random.Next(1, 4),
                        NumberOfEnteringPeopleToIsrael = _random.Next(1000, 10000),
                        NumberOfVerifiedCitizens = _random.Next(500, 3000),
                        NumberOfVerifiedForeigners = _random.Next(100, 1000),
                        PercentageOfVerifiedArrivals = _random.NextDouble() * 100
                    };

                    morbidityFromAbroad.Countries.Add(countryModel);
                }

                _coronaDataDashboardContext.MorbidityFromAbroad.Add(morbidityFromAbroad);
                await _coronaDataDashboardContext.SaveChangesAsync();

                listOfCountriesMorbidity.Add(morbidityFromAbroad);

                currentDate = currentDate.AddDays(-1);
            }

            return listOfCountriesMorbidity;
        }


        public void AddNewMorbidityData(DateTime date)
        {
            var morbidityFromAbroad = new MorbidityFromAbroadModel
            {
                Date = date,
                Countries = new List<CountryModel>()
            };

            foreach (Countries country in Enum.GetValues(typeof(Countries)))
            {
                var countryModel = new CountryModel
                {
                    CountryName = country,
                    RiskLevel = (RiskLevels)_random.Next(1, 4),
                    NumberOfEnteringPeopleToIsrael = _random.Next(1000, 10000),
                    NumberOfVerifiedCitizens = _random.Next(500, 3000),
                    NumberOfVerifiedForeigners = _random.Next(100, 1000),
                    PercentageOfVerifiedArrivals = _random.NextDouble() * 100
                };

                morbidityFromAbroad.Countries.Add(countryModel);
            }

            _coronaDataDashboardContext.MorbidityFromAbroad.Add(morbidityFromAbroad);
            _coronaDataDashboardContext.SaveChanges();
        }

        public async Task<List<HospitalModel>> AddHospitalsData()
        {
            DateTime currentDate = DateTime.Today;
            // Check if there is any data for the current date in the database
            bool dataExistsForCurrentDate = await _coronaDataDashboardContext.Hospitals
                .AnyAsync(h => h.Date == currentDate);
            if (dataExistsForCurrentDate) // If data exists for the current date, return the data for that date
            {
                List<HospitalModel> hospitalsData = await _coronaDataDashboardContext.Hospitals
                    .Where(h => h.Date == currentDate)
                    .ToListAsync();
                return hospitalsData;
            }
            else // If no data exists for the current date, generate new data for all hospitals
            {
                var listOfHospitalsData = new List<HospitalModel>();
                foreach (Hospitals hospital in Enum.GetValues(typeof(Hospitals)))
                {
                    var hospitalModal = new HospitalModel
                    {
                        Date = currentDate,
                        HospitalName = hospital,
                        OverallBedOccupancyPercentage = _random.NextDouble() * 100,
                        PercentageOfIndoorWardBedOccupancy = _random.NextDouble() * 100,
                    };
                    _coronaDataDashboardContext.Hospitals.Add(hospitalModal);
                    listOfHospitalsData.Add(hospitalModal);
                }
                await _coronaDataDashboardContext.SaveChangesAsync();
                return listOfHospitalsData;
            }
        }

        //public async Task<List<HospitalModel>> GetHospitalsDataForLatestDate()
        //{
        //    DateTime latestDate = await _coronaDataDashboardContext.Hospitals.MaxAsync(h => h.Date);

        //    List<HospitalModel> hospitalsData = await _coronaDataDashboardContext.Hospitals
        //        .Where(h => h.Date == latestDate)
        //        .ToListAsync();

        //    return hospitalsData;
        //}
        public async Task<List<Dictionary<string, object>>> GetHospitalsDataForLatestDate()
        {
            DateTime latestDate = await _coronaDataDashboardContext.Hospitals.MaxAsync(h => h.Date);

            var hospitalsData = await _coronaDataDashboardContext.Hospitals
                .Where(h => h.Date == latestDate)
                .Select(h => new
                {
                    h.Date,
                    HospitalName =h.HospitalName,
                    h.OverallBedOccupancyPercentage,
                    h.PercentageOfIndoorWardBedOccupancy
                })
                .ToListAsync();

            var response = new List<Dictionary<string, object>>();

            foreach (var hospital in hospitalsData)
            {
                var dataDictionary = new Dictionary<string, object>
        {
            { "Date", hospital.Date },
            { "HospitalName", GetEnumDescription(hospital.HospitalName) },
            { "OverallBedOccupancyPercentage", Math.Round(hospital.OverallBedOccupancyPercentage, 2) },
            { "PercentageOfIndoorWardBedOccupancy", Math.Round(hospital.PercentageOfIndoorWardBedOccupancy, 2) }
        };
                response.Add(dataDictionary);
            }

            return response;
        }
        public async Task<List<SettlementModel>> CalculateSettlementsStatisticsAsync()
        {
            //DateTime yesterday = DateTime.Today.AddDays(-1);
            //DateTime yesterday = DateTime.Parse("2023-05-10 00:00:00");
            //DateTime today = DateTime.Parse("2023-05-11 00:00:00");

            // var date = await FindDateWithMaxNewUnhealthyPeopleAsync();
            //

            var listOfSettlementsData = new List<SettlementModel>();
            DateTime latestDate = await LatestStatesDateAsync();
            DateTime dayBeforeTheLatestDate = latestDate.AddDays(-1);
            foreach (IsraelSettlements settlement in Enum.GetValues(typeof(IsraelSettlements)))
            {
                int numberOfNewVerifiedYesterday = await _coronaDataDashboardContext.People
                                    .CountAsync(p => p.SickCondition != SickCondition.Healthy && p.City == settlement && p.TestDate.Min(stat => stat.Date.Date) == latestDate && p.TestDate.First().Date.Date == latestDate);
                int numberOfNewTestsForVirusDetectionYesterday = await _coronaDataDashboardContext.People.CountAsync(p => p.City == settlement && ((p.TestDate.Count == 1 && p.TestDate.First().Date.Date == latestDate) || (p.TestDate.Min(stat => stat.Date.Date) == latestDate && p.TestDate.First().Date.Date == latestDate)));
                int numberOfResidentsInTheCity = await _coronaDataDashboardContext.People.CountAsync(p => p.City == settlement);
                int numberOfNewVerifiedOneDayBefore = await _coronaDataDashboardContext.People
                                    .CountAsync(p => p.SickCondition != SickCondition.Healthy && p.City == settlement && p.TestDate.Min(stat => stat.Date.Date) == dayBeforeTheLatestDate && p.TestDate.First().Date.Date == dayBeforeTheLatestDate);
                var settlementData = new SettlementModel
                {
                    SettlementName = GetEnumDescription(settlement),
                    ScoreAccordingToTrafficLightPlan = numberOfResidentsInTheCity > 0 ? (numberOfNewVerifiedYesterday * 10.0) / numberOfResidentsInTheCity : 1,
                    NewVerifiedPatients = numberOfNewVerifiedYesterday,
                    PercentageOfPositiveTests = numberOfNewTestsForVirusDetectionYesterday > 0 ? (numberOfNewVerifiedYesterday *100.0) / numberOfNewTestsForVirusDetectionYesterday : 0,
                    VerifiedChangeRate = numberOfNewVerifiedYesterday> 0 ? ((numberOfNewVerifiedYesterday - numberOfNewVerifiedOneDayBefore) * 100.0) / numberOfNewVerifiedYesterday : 0,
                    ActivePatients = await _coronaDataDashboardContext.People.Where(p => p.City == settlement && p.TestDate.OrderByDescending(td => td.Date).FirstOrDefault() != null && p.SickCondition != SickCondition.Healthy).CountAsync(),
                };
                listOfSettlementsData.Add(settlementData);
            }
            return listOfSettlementsData;
        }
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            if (field == null)
                return string.Empty;

            DescriptionAttribute attribute = field.GetCustomAttribute<DescriptionAttribute>();

            return attribute != null ? attribute.Description : value.ToString();
        }
    }

    //PatientStatisticsModel statistics = new()
    //{
    //    Date = date,
    //    NewUnhealthyPeopleYesterday = newUnhealthyPeopleYesterday,
    //    TotalUnhealthyPeople = totalUnhealthyPeople,
    //    TotalUnhealthyPatients = totalUnhealthyPatients,
    //    TotalUnhealthyHospitalized = totalUnhealthyHospitalized,
    //    TotalSeriousHospitalized = totalSeriousHospitalized,
    //    TotalCriticalHospitalized = totalCriticalHospitalized,
    //    TotalModerateHospitalized = totalModerateHospitalized,
    //    TotalMildHospitalized = totalMildHospitalized,
    //    TotalFirstDoseVaccinated = totalFirstDoseVaccinated,
    //    TotalSecondDoseVaccinated = totalSecondDoseVaccinated,
    //    TotalThirdDoseVaccinated = totalThirdDoseVaccinated,
    //    TotalFourthDoseVaccinated = totalFourthDoseVaccinated,
    //    TotalOmicronVaccinated = totalOmicronVaccinated,
    //    TotalDeadPeople = totalDeadPeople,
    //    PercentageUnhealthyYesterday = percentageUnhealthyYesterday
    //};

}
