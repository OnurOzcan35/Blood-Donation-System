using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BloodDonation.DTO
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public string UserId { get; set; }
        public int HospitalId { get; set; }
        public string HospitalName { get; set; }
        public string BloodType { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int HospitalUserId { get; set; }
        public DateTime NotiDate { get; set; }
        public bool State { get; set; }

    }
}
