using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodDonation.DTO
{
    public class User
    {
        public string DonorId { get; set; }
        public string BloodType { get; set; }
        public int Counter { get; set; }
        public bool IsLocationAv { get; set; }
        public bool IsAlltimes { get; set; }
        public bool IsAvaiable { get; set; }
        public bool IsMessageAv { get; set; }
    }
}
