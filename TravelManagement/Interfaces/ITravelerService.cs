using System;
using TravelManagement.Models;

namespace TravelManagement.Interfaces
{
    //traveler interface
	public interface ITravelerService
	{
        Task<Traveler> GetTravelerByNICAsync(string nic);
        Task CreateTravelerAsync(Traveler traveler);
        Task UpdateTravelerAsync(string nic, Traveler traveler);
        Task DeleteTravelerAsync(string nic);
        Task<IEnumerable<Traveler>> GetAllTravelersAsync();
    }
}

