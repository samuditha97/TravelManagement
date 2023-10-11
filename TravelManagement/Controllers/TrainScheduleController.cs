using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TravelManagement.DTO;
using TravelManagement.Interfaces;
using TravelManagement.Models;

namespace TravelManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainScheduleController : ControllerBase
    {
        private readonly ITrainScheduleService _service;

        public TrainScheduleController(ITrainScheduleService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrainScheduleDTO>>> GetAllTrainSchedules()
        {
            try
            {
                var trainSchedules = await _service.GetAllTrainSchedules();
                var trainSchedulesDTO = trainSchedules.Select(ts => new TrainScheduleDTO
                {
                    TrainId = ts.TrainId,
                    Name = ts.Name,
                    StartStation = ts.StartStation,
                    EndStations = ts.EndStations,
                    DepartureTime = ts.DepartureTime,
                    ArrivalTime = ts.ArrivalTime,
                    IsActive = ts.IsActive ? "Active" : "Inactive" // Convert bool to string
                }).ToList();

                return Ok(trainSchedulesDTO);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }


        [HttpGet("{trainId}")]
        public async Task<ActionResult<TrainScheduleDTO>> GetTrainSchedule(string trainId)
        {
            try
            {
                var trainSchedule = await _service.GetTrainScheduleById(trainId);
                if (trainSchedule == null)
                {
                    return NotFound("Train schedule not found");
                }

                var trainScheduleDTO = new TrainScheduleDTO
                {
                    TrainId = trainSchedule.TrainId,
                    Name = trainSchedule.Name,
                    StartStation = trainSchedule.StartStation,
                    EndStations = trainSchedule.EndStations,
                    DepartureTime = trainSchedule.DepartureTime,
                    ArrivalTime = trainSchedule.ArrivalTime,
                    IsActive = trainSchedule.IsActive ? "Active" : "Inactive"
                };

                return Ok(trainScheduleDTO);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateTrainSchedule([FromBody] TrainSchedule trainSchedule)
        {
            try
            {
                if (trainSchedule == null)
                {
                    return BadRequest("Invalid data.");
                }
                await _service.CreateTrainSchedule(trainSchedule);
                return CreatedAtAction(nameof(GetTrainSchedule), new { trainId = trainSchedule.TrainId }, trainSchedule);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpPut("{trainId}")]
        public async Task<IActionResult> UpdateTrainSchedule(string trainId, [FromBody] TrainSchedule trainSchedule)
        {
            try
            {
                if (trainSchedule == null || trainId != trainSchedule.TrainId)
                {
                    return BadRequest("Invalid data.");
                }
                var existingTrainSchedule = await _service.GetTrainScheduleById(trainId);
                if (existingTrainSchedule == null)
                {
                    return NotFound("Train schedule not found");
                }
                await _service.UpdateTrainSchedule(trainSchedule);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpDelete("{trainId}")]
        public async Task<IActionResult> DeleteTrainSchedule(string trainId)
        {
            try
            {
                var existingTrainSchedule = await _service.GetTrainScheduleById(trainId);
                if (existingTrainSchedule == null)
                {
                    return NotFound("Train schedule not found");
                }
                await _service.DeleteTrainSchedule(trainId);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
    }
}
