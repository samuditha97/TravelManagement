using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using TravelManagement.Interfaces;
using TravelManagement.Models;

namespace TravelManagement.Repositories
{
    public class TrainScheduleRepository : ITrainScheduleService
    {
        private readonly IMongoCollection<TrainSchedule> _collection;

        public TrainScheduleRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<TrainSchedule>("TrainSchedule");
        }

        public async Task CreateTrainSchedule(TrainSchedule trainSchedule)
        {
            await _collection.InsertOneAsync(trainSchedule);
        }

        public async Task DeleteTrainSchedule(string trainId)
        {
            var filter = Builders<TrainSchedule>.Filter.Eq(ts => ts.TrainId, trainId);
            await _collection.DeleteOneAsync(filter);
        }

        public async Task<IEnumerable<TrainSchedule>> GetAllTrainSchedules()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<TrainSchedule> GetTrainScheduleById(string trainId)
        {
            var filter = Builders<TrainSchedule>.Filter.Eq(ts => ts.TrainId, trainId);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task UpdateTrainSchedule(TrainSchedule trainSchedule)
        {
            var filter = Builders<TrainSchedule>.Filter.Eq(ts => ts.TrainId, trainSchedule.TrainId);
            await _collection.ReplaceOneAsync(filter, trainSchedule);
        }
    }
}
