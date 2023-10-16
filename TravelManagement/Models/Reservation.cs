using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TravelManagement.Models
{
    //reservation details class
	public class Reservation
	{
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        [BsonIgnoreIfDefault]
        public string ReferenceId { get; set; }
        public string NIC { get; set; }
        public string TrainId { get; set; }
        public string Train { get; set;  }
        public string TrainClass { get; set; }
        public int? TicketCount { get; set;  }
        public string CheckIn { get; set;  }
        public string CheckOut { get; set;  }
        public DateTime? ReservationDate { get; set; }
        public bool? IsCanceled { get; set; }

        public Reservation()
        {
            IsCanceled = false;
        }
    }
}

