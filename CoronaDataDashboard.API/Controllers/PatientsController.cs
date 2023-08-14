using CoronaDataDashboard.API.Models;
using CoronaDataDashboard.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoronaDataDashboard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientRepository _patientRepository;
        public PatientsController(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        [HttpPost("")]
        [Authorize]
        public async Task<IActionResult> AddNewPerson([FromBody] NewPersonModel newPersonModel)
        {
            var res = await _patientRepository.AddNewPerson(newPersonModel);
            if (res == null)
            {
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

        //[Authorize]
        [HttpGet("")]
        public async Task<IActionResult> GetAllPatients()
        {
            var res = await _patientRepository.GetAllPeopleAsync();
            if (res.Count == 0)
            {
                //return NoContent(); // Return 204 No Content if no stats found
                return BadRequest();
            }
            return Ok(res);
        }

        //[HttpPost("add-hero-to-trainer/{heroId:int}")]
        //public async Task<IActionResult> AddHeroToTrainer([FromRoute] int heroId)
        //{
        //    var userName = User.Identity.Name;
        //    if (userName == null)
        //    {
        //        return BadRequest();
        //    }
        //    var res = await _heroesRepository.AddHeroToTrainer(heroId, userName);
        //    if (!res)
        //    {
        //        return BadRequest();
        //    }
        //    return Ok();
        //}

        //[HttpGet("all-mine-heroes")]
        //[Authorize]
        //public async Task<IActionResult> GetAllMyHeroes()
        //{
        //    var userName = User.Identity.Name;
        //    if (userName == null)
        //    {
        //        return BadRequest();
        //    }
        //    var res = await _heroesRepository.GetAllHeroesByUserName(userName);
        //    if (res == null || res.Count == 0)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(res);
        //}

        //[HttpPost("train/{heroId:int}")]
        //public async Task<IActionResult> TrainMyHero([FromRoute] int heroId)
        //{
        //    var userName = User?.Identity?.Name;
        //    if (userName == null)
        //    {
        //        return BadRequest();
        //    }
        //    var res = await _heroesRepository.TrainHero(heroId, userName);
        //    if (res == null)
        //    {
        //        return BadRequest();
        //    }
        //    return Ok(res);
        //}
    }
}
