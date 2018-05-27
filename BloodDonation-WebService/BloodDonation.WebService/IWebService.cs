using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using BloodDonation.DTO;

namespace BloodDonation.WebService
{
    // NOT: "IService1" arabirim adını kodda ve yapılandırma dosyasında birlikte değiştirmek için "Yeniden Düzenle" menüsündeki "Yeniden Adlandır" komutunu kullanabilirsiniz.
    [ServiceContract]
    public interface WebService
    {
        [OperationContract]
        bool UserRegister(string UserId,string BloodType,int Counter,bool IsLocationAv,bool IsAlltimes,bool IsAvaiable,bool IsMessageAv);

        [OperationContract]
        User SelectUser(string UserId);

        [OperationContract]
        bool UserUpdateTime(string UserId, int Day, int Time, bool IsAvaiable);

        [OperationContract]
        Times[] SelectUserTime(string UserId);

        [OperationContract]
        bool UserDelete(string DonorId);

        [OperationContract]
        bool UserLocationUpdate(string UserId, string Name, bool IsLocationAv,decimal Latitude, decimal Longitude);

        [OperationContract]
        List<Hospital> SelectUserLocation(string UserId);

        [OperationContract]
        List<Hospital> SelectHospital();

        [OperationContract]
        bool UserLocationDelete(string UserId, string Name);

        [OperationContract]
        bool UserUpdateGeneral(string DonorId, string BloodType, int DonationCounter, bool IsLocationAv, bool IsAlltimes, bool IsAvaiable, bool IsMessageAv);

        [OperationContract]
        bool ControlHospitalUser(int HospitalId, string UserName, string UserPassword);

        [OperationContract]
        Notification SelectNotification(int NotificationId);

        [OperationContract]
        bool UpdateStateNotification(int NotificationId);

        [OperationContract]
        void SelectUserNotificationAccept(int HospitalId, int UserId, string BloodType);

        [OperationContract]
        bool DeleteAllUserLocations(string UserId);

        [OperationContract]
        bool UpdateAllUserTimes(string UserId, bool IsAvaiable);

        [OperationContract]
        bool AddToken(string UserId, string Token);

        [OperationContract]
        DataTable NotificationAccept(int HospitalId);

        [OperationContract]
        bool NotificationFinished(int HospitalId, string BloodType);

        [OperationContract]
        bool SendMessage(string UserId, int HospitalId, bool IsPerson, string Content);

        [OperationContract]
        Notification GetActiveNotification(string UserId);

        [OperationContract]
        Notification GetNotFinishedNotification(string UserId);

        [OperationContract]
        List<Message> GetAllMessages(string UserId, int HospitalId);

        [OperationContract]
        bool UpdateReadNotification(int NotificationId);
    }

}
