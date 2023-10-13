using System;
namespace TravelManagement.DTO
{
    //traveler detail dto
	public class TravelerDetailDTO
	{
        public string NIC { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string IsActive { get; set; }
    }
}

