using CoronaDataDashboard.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoronaDataDashboard.API.Data
{
    public class CoronaDataDashboardContext : IdentityDbContext<AppUser>
    {
        public CoronaDataDashboardContext(DbContextOptions<CoronaDataDashboardContext> options) : base(options)
        {
        }
        public DbSet<StatsModel> States { get; set; }
        public DbSet<PersonModel> People { get; set; }
        public DbSet<HospitalModel> Hospitals { get; set; }
        public DbSet<SettlementModel> Settlements { get; set; }
        public DbSet<CountryModel> Countries { get; set; }
        public DbSet<MorbidityFromAbroadModel> MorbidityFromAbroad { get; set; }

    }
}