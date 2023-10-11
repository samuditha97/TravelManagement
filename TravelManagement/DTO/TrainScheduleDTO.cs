using System;
namespace TravelManagement.DTO
{
	public class TrainScheduleDTO
	{
        public string TrainId { get; set; }
        public string Name { get; set; }
        public string StartStation { get; set; }
        public string EndStations { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string IsActive { get; set; }
    }
}

