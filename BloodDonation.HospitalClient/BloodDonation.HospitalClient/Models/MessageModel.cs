using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BloodDonation.HospitalClient.Models
{
    public class MessageModel
    {
        public int MessageId { get; set; }
        public int HospitalId { get; set; }
        public string UserId { get; set; }
        public bool IsPerson { get; set; }
        public string Content { get; set; }
    }
}