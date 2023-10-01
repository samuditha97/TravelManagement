using System;
using MongoDB.Driver;
using TravelManagement.Interfaces;
using TravelManagement.Models;

namespace TravelManagement.Repositories
{
	public class TravelerRepository: ITravelerService
    {

        private readonly IMongoCollection<Traveler> _travelerCollection;

        public TravelerRepository(IMongoDatabase db)
        {
            _travelerCollection = db.GetCollection<Traveler>("Travelers");
        }

        public async Task<Traveler> GetTravelerByNICAsync(string nic)
        {
            return await _travelerCollection.Find(t => t.NIC == nic).FirstOrDefaultAsync();
        }

        public async Task CreateTravelerAsync(Traveler traveler)
        {
            await _travelerCollection.InsertOneAsync(traveler);
        }

        public async Task UpdateTravelerAsync(string nic, Traveler updatedTraveler)
        {

            var existingTraveler = await _travelerCollection.Find(t => t.NIC == nic).FirstOrDefaultAsync();

            if (existingTraveler == null)
            {
                throw new NotFoundException("Traveler not found"); 
            }


            existingTraveler.FirstName = updatedTraveler.FirstName;
            existingTraveler.LastName = updatedTraveler.LastName;
            existingTraveler.Email = updatedTraveler.Email;
            existingTraveler.IsActive = updatedTraveler.IsActive;

    
            var filter = Builders<Traveler>.Filter.Eq(t => t.NIC, nic);
            var update = Builders<Traveler>.Update
                .Set(t => t.FirstName, existingTraveler.FirstName)
                .Set(t => t.LastName, existingTraveler.LastName)
                .Set(t => t.Email, existingTraveler.Email)
                .Set(t => t.IsActive, existingTraveler.IsActive);

            await _travelerCollection.UpdateOneAsync(filter, update);
        }



        public async Task DeleteTravelerAsync(string nic)
        {
            var filter = Builders<Traveler>.Filter.Eq(t => t.NIC, nic);
            await _travelerCollection.DeleteOneAsync(filter);
        }

        public async Task<IEnumerable<Traveler>> GetAllTravelersAsync()
        {
            var travelers = await _travelerCollection.Find(_ => true).ToListAsync();
            return travelers;
        }
    }
}

