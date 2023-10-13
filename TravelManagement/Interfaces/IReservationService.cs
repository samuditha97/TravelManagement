using System;
using TravelManagement.Models;

namespace TravelManagement.Interfaces
{
    //reservation interface
	public interface IReservationService
	{
        Task CreateReservation(Reservation reservation);
        Task UpdateReservation(string referenceId, Reservation updatedReservation);
        Task CancelReservation(string referenceId);
        Task<Reservation> GetReservation(string referenceId);
        Task<IEnumerable<Reservation>> GetAllReservations();
    }
}

