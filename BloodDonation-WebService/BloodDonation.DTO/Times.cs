using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BloodDonation.DTO
{
    public class Times
    {
        public string UserId { get; set; }
        public int Day { get; set; }
        public int Time { get; set; }
        public bool IsAvaiable { get; set; }

    }
}
