using CoronaDataDashboard.API.Data;
using CoronaDataDashboard.API.Entities;
using CoronaDataDashboard.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CoronaDataDashboard.API.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        /// <summary>
        /// /NUMBEROFREQUEST SINGLTON.
        /// איזה סוגים יש ואיך אני עושה כל אחד מהם 
        /// </summary>
        private readonly CoronaDataDashboardContext _coronaDataDashboardContext;
        public PatientRepository(CoronaDataDashboardContext coronaDataDashboardContext)
        {
            _coronaDataDashboardContext = coronaDataDashboardContext;
        }
        public async Task<Guid?> AddNewPerson(NewPersonModel newPersonModel)
        {
            PersonModel personModel = new()
            {
                //TestDate = await GetStatByDateAsync(newPersonModel.TestDate),
                FullName  = newPersonModel.FullName,
                Gender = newPersonModel.Gender,
                City = newPersonModel.City,
                SickCondition = newPersonModel.SickCondition,
                IsFirstDoseVaccinated = newPersonModel.IsFirstDoseVaccinated,
                IsSecondDoseVaccinated = newPersonModel.IsSecondDoseVaccinated,
                IsThirdDoseVaccinated=newPersonModel.IsThirdDoseVaccinated,
                IsFourthDoseVaccinated =newPersonModel.IsFourthDoseVaccinated,
                IsOmicronVaccinated = newPersonModel.IsOmicronVaccinated,
                IsDeadPatients = newPersonModel.IsDeadPatients,
                IsHospitalized = newPersonModel.IsHospitalized,
            };
            var stat = await GetStatByDateAsync(newPersonModel.TestDate);
            if (stat != null)
            {
                if (personModel.TestDate.Count == 0 )
                {
                    personModel.TestDate = new List<StatsModel>();
                }
                personModel.TestDate.Add(stat);
                _coronaDataDashboardContext.People.Add(personModel);
                var res = await _coronaDataDashboardContext.SaveChangesAsync();
                if (res != 0)
                {
                    await AddPersonToStatAsync(personModel.Id, stat);
                    return personModel.Id;
                }
            }

            //_coronaDataDashboardContext.People.Add(personModel);
            //var res = await _coronaDataDashboardContext.SaveChangesAsync();
            //if (res != 0)
            //{
            //    await AddPersonToStatAsync(personModel.Id, personModel.TestDate.Id);
            //    return personModel.Id;
            //}

            return null;
        }
        private async Task<List<PersonModel>> AddNewListOfPeopleAsync(List<NewPersonModel> newListOfPeopleModel)
        {
            List<PersonModel> listOfPeople = new List<PersonModel>(); // Initialize as an empty list
            Random random = new Random();
            foreach (var newPersonModel in newListOfPeopleModel)
            {
                PersonModel personModel = new()
                {
                    //TestDate = await GetStatByDateAsync(newPersonModel.TestDate),
                    FullName = newPersonModel.FullName,
                    Gender = newPersonModel.Gender,
                    City = newPersonModel.City,
                    SickCondition = newPersonModel.SickCondition,
                    IsFirstDoseVaccinated = newPersonModel.IsFirstDoseVaccinated,
                    IsSecondDoseVaccinated = newPersonModel.IsSecondDoseVaccinated,
                    IsThirdDoseVaccinated = newPersonModel.IsThirdDoseVaccinated,
                    IsFourthDoseVaccinated = newPersonModel.IsFourthDoseVaccinated,
                    IsOmicronVaccinated = newPersonModel.IsOmicronVaccinated,
                    IsDeadPatients = newPersonModel.IsDeadPatients,
                    IsHospitalized = newPersonModel.IsHospitalized,
                };
                var stat = await GetStatByDateAsync(newPersonModel.TestDate);
                if (stat != null)
                {
                    if (personModel.TestDate == null)
                    {
                        personModel.TestDate = new List<StatsModel>();
                    }
                    personModel.TestDate.Add(stat); _coronaDataDashboardContext.People.Add(personModel);
                    var res = await _coronaDataDashboardContext.SaveChangesAsync();
                    if (res != 0)
                    {
                        await AddPersonToStatAsync(personModel.Id, stat);
                        if (random.Next(2) == 0)
                        {
                            await AddPersonToAllDaysWithRandomlyData(personModel);
                        }
                        listOfPeople?.Add(personModel);
                    }
                }
            }
            return listOfPeople;
        }
        private async Task<StatsModel> GetStatByDateAsync(DateTime testDate)
        {
            /////פותח תרד במקביל אליך צריך לעשות לו אוויט
            ///אם אתה לא עושה אוויט הוא מתקדם הלאה 
            ///
           await Task.Run(() => Console.WriteLine("efejfje"));
           
            var stat = await _coronaDataDashboardContext.States.Include(s => s.People).FirstOrDefaultAsync(s => s.Date.Date == testDate.Date);
            return stat;
        }
        private async Task<bool> AddPersonToStatAsync(Guid personId, StatsModel stats)
        {
            //var stat = await _coronaDataDashboardContext.States.Include(s => s.People).FirstOrDefaultAsync(s => s.Id == statId);
            //if (stat == null)
            //{
            //    return false;
            //}
            var person = await _coronaDataDashboardContext.People.Include(p => p.TestDate).FirstOrDefaultAsync(p => p.Id == personId);
            if (person == null)
            {
                return false;
            }
            if (stats.People.Contains(person))
            {
                return false;
            }
            stats.People.Add(person);
            var res = await _coronaDataDashboardContext.SaveChangesAsync();
            return res != 0;
        }
        public async Task<List<PersonModel>> GetAllPeopleAsync()
        {
            var people = await _coronaDataDashboardContext.People.Include(u => u.TestDate).ToListAsync();
            if(people != null && people.Count != 0)
            {
                return people;
            }

            List<NewPersonModel> newRandomPeopleList = GenerateRandomPeopleData();
            var listOfPeople = await AddNewListOfPeopleAsync(newRandomPeopleList);
            return listOfPeople;
            /*if (people == null)
            {
            }
            return people.ToList();*/
        }
        private static List<NewPersonModel> GenerateRandomPeopleData()
        {
            List<NewPersonModel> newPeople = new List<NewPersonModel>(); // Initialize as an empty list
            IsraelSettlements[]? cities = Enum.GetValues(typeof(IsraelSettlements)) as IsraelSettlements[];
            Gender[]? genders = Enum.GetValues(typeof(Gender)) as Gender[];
            SickCondition[]? states = Enum.GetValues(typeof(SickCondition)) as SickCondition[];
            Random random = new Random();

            //DateTime currentDate = DateTime.Now;
            DateTime currentDate = DateTime.Parse("2023-06-11 20:09:15.8213581");

            DateTime oneYearBack = currentDate.AddYears(-1);

            DateTime dateToAdd = oneYearBack;
            while (dateToAdd <= currentDate)
            {
                int numOfPeople = random.Next(10, 100);
                for (int i = 1; i < numOfPeople; i++)
                {
                    NewPersonModel newPerson = new NewPersonModel()
                    {
                        FullName = "Dummy",
                        Gender = genders[random.Next(genders.Length)],
                        City = cities[random.Next(cities.Length)]
                    };

                    //DateTime date = new DateTime(2022, random.Next(1, 28), random.Next(1, 12));
                    //DateTime date = new DateTime(2023,01,random.Next(1, 28));
                    //DateTime date = new DateTime(2023, 01, 11);

                    newPerson.TestDate = dateToAdd;

                    bool isAlive = random.Next(2) == 0;
                    newPerson.IsDeadPatients = !isAlive;

                    newPerson.IsFirstDoseVaccinated = random.Next(2) == 0;
                    newPerson.IsSecondDoseVaccinated = random.Next(2) == 0;
                    newPerson.IsThirdDoseVaccinated = random.Next(2) == 0;
                    newPerson.IsFourthDoseVaccinated = random.Next(2) == 0;
                    newPerson.IsOmicronVaccinated = random.Next(2) == 0;

                    newPerson.SickCondition = states[random.Next(states.Length)];

                    if (newPerson.SickCondition != SickCondition.Healthy)
                    {
                        newPerson.IsHospitalized = random.Next(2) == 0;
                    }
                    else
                    {
                        newPerson.IsHospitalized = false;
                    }

                    newPeople.Add(newPerson);
                }
                dateToAdd = dateToAdd.AddMonths(1);

            }
            return newPeople;
        }

        //private async Task UpdatePersonDataRandomly(PersonModel personModel)
        //{
        //    foreach (var newPerson in newPeople)
        //    {
        //        var person = await _coronaDataDashboardContext.People.FirstOrDefaultAsync(p => p.FullName == newPerson.FullName);

        //        if (person == null)
        //        {
        //            // Create a new person if they don't exist
        //            var newPersonModel = new PersonModel
        //            {
        //                FullName = newPerson.FullName,
        //                Gender = newPerson.Gender,
        //                City = newPerson.City,
        //                SickCondition = newPerson.SickCondition,
        //                IsFirstDoseVaccinated = newPerson.IsFirstDoseVaccinated,
        //                IsSecondDoseVaccinated = newPerson.IsSecondDoseVaccinated,
        //                IsThirdDoseVaccinated = newPerson.IsThirdDoseVaccinated,
        //                IsFourthDoseVaccinated = newPerson.IsFourthDoseVaccinated,
        //                IsOmicronVaccinated = newPerson.IsOmicronVaccinated,
        //                IsDeadPatients = newPerson.IsDeadPatients
        //            };

        //            _coronaDataDashboardContext.People.Add(newPersonModel);
        //        }
        //        else
        //        {
        //            // Update the existing person's status for a specific day
        //            var stats = await _coronaDataDashboardContext.Stats.FirstOrDefaultAsync(s => s.Date == newPerson.TestDate);
        //            if (stats == null)
        //            {
        //                // Create a new stats record if it doesn't exist for the specified date
        //                stats = new StatsModel
        //                {
        //                    Date = newPerson.TestDate,
        //                    People = new List<PersonModel>()
        //                };

        //                _coronaDataDashboardContext.Stats.Add(stats);
        //            }

        //            // Update the person's status for the specific day
        //            person.SickCondition = newPerson.SickCondition;
        //            person.IsHospitalized = (newPerson.SickCondition != SickCondition.Healthy) ? newPerson.IsHospitalized : false;
        //            stats.People.Add(person);
        //        }
        //    }

        //    await _coronaDataDashboardContext.SaveChangesAsync();
        //}

        private async Task AddPersonToAllDaysWithRandomlyData(PersonModel person)
        {
            //var stats = await _coronaDataDashboardContext.States.Include(s => s.People).ToListAsync();
            DateTime latestTestDate = person.TestDate.Max().Date; // Get the latest test date of the person

            var stats = await _coronaDataDashboardContext.States
                .Where(s => s.Date >= latestTestDate)
                .Include(s => s.People)
                .ToListAsync();

            SickCondition[]? sickConditions = Enum.GetValues(typeof(SickCondition)) as SickCondition[];

            foreach (var stat in stats)
            {
                //if (person.IsDeadPatients)
                if (person.IsDeadPatients || person.SickCondition == SickCondition.Healthy)
                {
                    // If IsDeadPatients is true, stop adding the person to the subsequent days
                    break;
                }

                // Check if the person doesn't already exist for the current date
                if (!stat.People.Any(p => p.Id == person.Id))
                {
                    // Generate random values for SickCondition, IsHospitalized, and IsDeadPatients
                    Random random = new Random();
                    person.SickCondition = sickConditions[random.Next(sickConditions.Length)];
                    person.IsHospitalized = (person.SickCondition != SickCondition.Healthy) ? random.Next(2) == 0 : false;
                    person.IsDeadPatients = (random.Next(2) == 0) ? false : person.IsDeadPatients;

                    stat.People.Add(person);

                    // Update the person's information in the People table
                    var existingPerson = await _coronaDataDashboardContext.People.Include(p => p.TestDate).FirstOrDefaultAsync(p => p.Id == person.Id);
                    if (existingPerson != null)
                    {
                        existingPerson.SickCondition = person.SickCondition;
                        existingPerson.IsHospitalized = person.IsHospitalized;
                        existingPerson.IsDeadPatients = person.IsDeadPatients;
                        existingPerson.TestDate.Add(stat);
                    }
                }
            }
            await _coronaDataDashboardContext.SaveChangesAsync();
        }

    }
}


