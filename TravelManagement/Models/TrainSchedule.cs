using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TravelManagement.Models
{
	public class TrainSchedule
	{
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        [BsonIgnoreIfDefault]
        public string TrainId { get; set; }
        public string Name { get; set; }
        public string StartStation { get; set; }
        public string EndStations { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public bool IsActive { get; set; }
    }

}

