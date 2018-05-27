using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BloodDonation.DTO
{
    public class Location
    {
        public string UserId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
