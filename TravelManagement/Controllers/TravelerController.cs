using System;
using Microsoft.AspNetCore.Mvc;
using TravelManagement.DTO;
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

        // GET api/traveler/{nic}
        [HttpGet("{nic}")]
        public async Task<ActionResult<TravelerDetailDTO>> GetTraveler(string nic)
        {
            try
            {
                // Get a traveler by NIC
                var traveler = await _travelerRepository.GetTravelerByNICAsync(nic);
                if (traveler == null)
                {
                    return NotFound("Traveler not found");
                }

                var travelerDTO = new TravelerDetailDTO
                {
                    NIC = traveler.NIC,
                    FirstName = traveler.FirstName,
                    LastName = traveler.LastName,
                    Email = traveler.Email,
                    MobileNo = traveler.MobileNo,
                    IsActive = traveler.IsActive ? "Active" : "Inactive"
                };

                return Ok(travelerDTO);
            }
            catch (Exception ex)
            {
                // Log and handle exceptions
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        // POST api/traveler
        [HttpPost]
        public async Task<IActionResult> CreateTraveler(Traveler traveler)
        {
            try
            {
                // Check if a traveler with the same NIC already exists
                var existingTraveler = await _travelerRepository.GetTravelerByNICAsync(traveler.NIC);
                if (existingTraveler != null)
                {
                    var conflictResult = new ObjectResult("A traveler with the same NIC already exists.")
                    {
                        StatusCode = StatusCodes.Status409Conflict
                    };
                    return conflictResult;
                }

                // Create a new traveler
                await _travelerRepository.CreateTravelerAsync(traveler);
                return CreatedAtAction(nameof(GetTraveler), new { nic = traveler.NIC }, traveler);
            }
            catch (Exception ex)
            {
                // Log and handle exceptions
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        // PUT api/traveler/{nic}
        [HttpPut("{nic}")]
        public async Task<IActionResult> UpdateTraveler(string nic, Traveler traveler)
        {
            if (nic != traveler.NIC)
            {
                return BadRequest("NIC parameter does not match the traveler's NIC.");
            }

            try
            {
                // Update a traveler
                await _travelerRepository.UpdateTravelerAsync(nic, traveler);
                return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound("Traveler not found");
            }
            catch (Exception ex)
            {
                // Log and handle exceptions
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        // DELETE api/traveler/{nic}
        [HttpDelete("{nic}")]
        public async Task<IActionResult> DeleteTraveler(string nic)
        {
            try
            {
                // Check if the traveler exists and then delete
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
                // Log and handle exceptions
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        // GET api/traveler
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Traveler>>> GetAllTravelers()
        {
            try
            {
                // Get all travelers and map to DTO
                var travelers = await _travelerRepository.GetAllTravelersAsync();
                var travelerDTOs = travelers.Select(traveler => new TravelerDetailDTO
                {
                    NIC = traveler.NIC,
                    FirstName = traveler.FirstName,
                    LastName = traveler.LastName,
                    Email = traveler.Email,
                    MobileNo = traveler.MobileNo,
                    IsActive = traveler.IsActive ? "Active" : "Inactive"
                });

                return Ok(travelerDTOs);
            }
            catch (Exception ex)
            {
                // Log and handle exceptions
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
    }
}
