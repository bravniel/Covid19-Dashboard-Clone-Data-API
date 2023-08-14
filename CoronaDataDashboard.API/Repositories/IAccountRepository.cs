using CoronaDataDashboard.API.Models;
using Microsoft.AspNetCore.Identity;

namespace CoronaDataDashboard.API.Repositories
{
    public interface IAccountRepository
    {
        Task<string> LoginAsync(SignInModel signInModel);
        Task<IdentityResult> SignUpAsync(SignUpModel signUpModel);
    }
}