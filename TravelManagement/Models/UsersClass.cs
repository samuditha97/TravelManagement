using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TravelManagement.Models
{
    //user authentication class
	public class UsersClass
	{
		
            [BsonId]
            public ObjectId Id { get; set; }

            [BsonElement("Username")]
            public string Username { get; set; }

            [BsonElement("Password")]
            public string Password { get; set; }

            [BsonElement("Role")]
            public string Role { get; set; }

            [BsonElement("NIC")]
            public string NIC { get; set; }

    }
}

