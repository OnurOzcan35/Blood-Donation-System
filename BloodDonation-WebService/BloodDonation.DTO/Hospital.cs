using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BloodDonation.DTO
{
    public class Hospital
    {
        public int HospitalId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

    }
}
