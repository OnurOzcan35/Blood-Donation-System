using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BloodDonation.HospitalClient.Models
{
    public class UserModel
    {
        public string UserId { get; set; }
        public string BloodType { get; set; }
        public bool IsMessageAv { get; set; }

    }
}