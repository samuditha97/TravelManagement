using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TravelManagement.Models
{
	public class Traveler
	{
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string NIC { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public bool IsActive { get; set; } = false;
    }
}

