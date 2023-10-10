using System;
using Microsoft.AspNetCore.Mvc;
using TravelManagement.Interfaces;
using TravelManagement.Models;
using TravelManagement.Repositories;

namespace TravelManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TravelerController : ControllerBase
    {
        private readonly ITravelerService _travelerRepository;

        public TravelerController(ITravelerService travelerRepository)
        {
            _travelerRepository = travelerRepository;
        }

        [HttpGet("{nic}")]
        public async Task<ActionResult<Traveler>> GetTraveler(string nic)
        {
            try
            {
                var traveler = await _travelerRepository.GetTravelerByNICAsync(nic);
                if (traveler == null)
                {
                    return NotFound("Traveler not found");
                }
                return Ok(traveler);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTraveler(Traveler traveler)
        {
            try
            {
 
                var existingTraveler = await _travelerRepository.GetTravelerByNICAsync(traveler.NIC);
                if (existingTraveler != null)
                {

                    var conflictResult = new ObjectResult("A traveler with the same NIC already exists.")
                    {
                        StatusCode = StatusCodes.Status409Conflict 
                    };
                    return conflictResult;
                }

                await _travelerRepository.CreateTravelerAsync(traveler);
                return CreatedAtAction(nameof(GetTraveler), new { nic = traveler.NIC }, traveler);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }


        [HttpPut("{nic}")]
        public async Task<IActionResult> UpdateTraveler(string nic, Traveler traveler)
        {
            if (nic != traveler.NIC)
            {
                return BadRequest("NIC parameter does not match the traveler's NIC.");
            }

            try
            {
                await _travelerRepository.UpdateTravelerAsync(nic, traveler);
                return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound("Traveler not found");
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpDelete("{nic}")]
        public async Task<IActionResult> DeleteTraveler(string nic)
        {
            try
            {
                var existingTraveler = await _travelerRepository.GetTravelerByNICAsync(nic);
                if (existingTraveler == null)
                {
                    return NotFound("Traveler not found");
                }

                await _travelerRepository.DeleteTravelerAsync(nic);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Traveler>>> GetAllTravelers()
        {
            try
            {
                var travelers = await _travelerRepository.GetAllTravelersAsync();
                return Ok(travelers);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
    }

}

