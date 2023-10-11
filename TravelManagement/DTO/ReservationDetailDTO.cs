using System;
namespace TravelManagement.DTO
{
	public class ReservationDetailDTO
	{
        public string ReferenceId { get; set; }
        public string TrainId { get; set; }
        public string Train { get; set; }
        public string TrainClass { get; set; }
        public int? TicketCount { get; set; }
        public string CheckIn { get; set; }
        public string CheckOut { get; set; }
        public DateTime? ReservationDate { get; set; }
        public string IsCanceled { get; set; }
    }
}

