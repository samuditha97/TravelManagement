using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TravelManagement.Interfaces;
using TravelManagement.Models;
using TravelManagement.Repositories;

namespace TravelManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservation([FromBody] Reservation reservation)
        {
            try
            {
                await _reservationService.CreateReservation(reservation);
                return CreatedAtAction(nameof(GetReservation), new { referenceId = reservation.ReferenceId }, reservation);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{referenceId}")]
        public async Task<IActionResult> UpdateReservation(string referenceId, [FromBody] Reservation updatedReservation)
        {
            try
            {
                await _reservationService.UpdateReservation(referenceId, updatedReservation);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{referenceId}")]
        public async Task<IActionResult> CancelReservation(string referenceId)
        {
            try
            {
                await _reservationService.CancelReservation(referenceId);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{referenceId}")]
        public async Task<IActionResult> GetReservation(string referenceId)
        {
            var reservation = await _reservationService.GetReservation(referenceId);

            if (reservation != null)
            {
                return Ok(reservation);
            }

            return NotFound("Reservation not found.");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReservations()
        {
            var reservations = await _reservationService.GetAllReservations();
            return Ok(reservations);
        }
    }
}
