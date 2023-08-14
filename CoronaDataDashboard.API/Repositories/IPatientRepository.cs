using CoronaDataDashboard.API.Models;

namespace CoronaDataDashboard.API.Repositories
{
    public interface IPatientRepository
    {
        Task<Guid?> AddNewPerson(NewPersonModel newPersonModel);
        Task<List<PersonModel>> GetAllPeopleAsync();
    }
}