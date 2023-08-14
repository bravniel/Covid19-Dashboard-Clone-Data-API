using CoronaDataDashboard.API.Models;
using CoronaDataDashboard.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoronaDataDashboard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private readonly IStatsRepository _statRepository;
        public StatsController(IStatsRepository statRepository)
        {
            _statRepository = statRepository;
        }

        [HttpPost("")]
        public async Task<IActionResult> AddNewStat([FromBody] NewStatModel newStatModel)
        {
            var res = await _statRepository.AddNewStat(newStatModel);
            if (res == null)
            {
                return BadRequest();
            }
            return Ok(res);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllPatients()
        {
            var res = await _statRepository.GetAllStatesAsync();
            if (res.Count == 0)
            {
                //return NoContent(); // Return 204 No Content if no stats found
                return BadRequest();
            }
            return Ok(res);
        }
        
        [HttpGet("focus-on")]
        public async Task<IActionResult> GetCalculatePatientStatisticsAsync()
        {
            var res = await _statRepository.CalculatePatientStatisticsAsync();
            if (res == null)
            {
                //return NoContent(); // Return 204 No Content if no stats found
                return BadRequest();
            }
            return Ok(res);
        }
        [HttpGet("latest-states-date")]
        public async Task<IActionResult> GetLatestStatesDateAsync()
        {
            var res = await _statRepository.LatestStatesDateAsync();
            if (res == null)
            {
                //return NoContent(); // Return 204 No Content if no stats found
                return BadRequest();
            }
            return Ok(res);
        }

        //[HttpGet("")]
        //[Authorize]
        //public async Task<IActionResult> GetRandomPatientData([FromQuery] DateTime statDate)
        //{
        //    var res = await _patientRepository.GetAllPeopleAsync();
        //    return Ok(res);
        //}

        //[HttpGet("")]
        ////[Authorize]
        //public async Task<IActionResult> GetAllPatients()
        //{
        //    var res = await _patientRepository.GetAllPeopleAsync();
        //    return Ok(res);
        //}

        [HttpGet("all-morbidity-from-abroad")]
        public async Task<IActionResult> GetAllMorbidityFromAbroad()
        {
            var res = await _statRepository.GetAllMorbidityFromAbroadAsync();
            if (res.Count == 0)
            {
                //return NoContent(); // Return 204 No Content if no stats found
                return BadRequest();
            }
            return Ok(res);
        }
        [HttpGet("calculate-morbidity-sum-from-abroad")]
        public async Task<IActionResult> GetCalculateMorbiditySum([FromQuery] int days = 90)
        {
            var res = await _statRepository.CalculateMorbiditySumAsync(days);
            if (res.Count == 0)
            {
                //return NoContent(); // Return 204 No Content if no stats found
                return BadRequest();
            }
            return Ok(res);
        }
        
        [HttpPost("hospitals-data")]
        public async Task<IActionResult> AddNewHospitalsData()
        {
            var res = await _statRepository.AddHospitalsData();
            if (res == null)
            {
                return BadRequest();
            }
            return Ok(res);
        }
        [HttpGet("latest-hospitals-date")]
        public async Task<IActionResult> GetHospitalsDataForLatestDateAsync()
        {
            var res = await _statRepository.GetHospitalsDataForLatestDate();
            if (res == null)
            {
                //return NoContent(); // Return 204 No Content if no stats found
                return BadRequest();
            }
            return Ok(res);
        }
        [HttpGet("lights-in-settlements")]
        public async Task<IActionResult> GetLightsInSettlementsDataForLatestDateAsync()
        {
            var res = await _statRepository.CalculateSettlementsStatisticsAsync();
            if (res == null)
            {
                //return NoContent(); // Return 204 No Content if no stats found
                return BadRequest();
            }
            return Ok(res);
        }
    }
}
