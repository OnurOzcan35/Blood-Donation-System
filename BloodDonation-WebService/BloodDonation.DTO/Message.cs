using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BloodDonation.DTO
{
    public class Message
    {
        public int MessageId { get; set; }
        public string UserId { get; set; }
        public int HospitalId { get; set; }
        public bool IsPerson { get; set; }
        public string Content { get; set; }
        public DateTime MessageDate { get; set; }

    }
}
