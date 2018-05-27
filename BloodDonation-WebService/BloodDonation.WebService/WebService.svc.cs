using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Net;
using System.Web.Services;
using System.Xml;
using BloodDonation.DTO;
using Newtonsoft.Json;

namespace BloodDonation.WebService
{

    public class IWebService : WebService
    {



        [WebMethod(Description = "BloodDonation User Register")]
        public bool UserRegister(string UserId, string BloodType, int Counter, bool IsLocationAv, bool IsAlltimes, bool IsAvaiable, bool IsMessageAv)
        {

            BloodDonation.Requirements.Utilities BloodUtil = new BloodDonation.Requirements.Utilities();
            try
            {

                BloodUtil.WriteEventLogRecord("BloodDonation", "BloodDonation", "Registeration-Start", System.Diagnostics.EventLogEntryType.Information);



                #region Config Read

                string Server;
                string User;
                string Password;
                string DatabaseName;

                try
                {
                    DataSet ds = new DataSet();
                    string strConfigFileName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.ToString()) + "\\Config.xml";
                    ds.ReadXml(strConfigFileName);

                    Server = ds.Tables[0].Rows[0]["Server"].ToString();
                    User = ds.Tables[0].Rows[0]["User"].ToString();
                    Password = ds.Tables[0].Rows[0]["Password"].ToString();
                    DatabaseName = ds.Tables[0].Rows[0]["DatabaseName"].ToString();

                }
                catch (Exception exc)
                {
                    throw new Exception("Web service Config could not read", exc);
                }

                #endregion

                #region Database Connection

                BloodDonation.Requirements.DataLayer MyDal = new BloodDonation.Requirements.DataLayer(Server, User, Password, DatabaseName);


                #endregion

                #region Process

                DataTable dtParam = new DataTable();
                dtParam.Columns.Add("ParameterName", typeof(string));
                dtParam.Columns.Add("ParameterValue", typeof(object));
                dtParam.Columns.Add("ParameterType", typeof(object));
                DataRow dr = dtParam.NewRow();
                dr["ParameterName"] = "DonorId";
                dr["ParameterValue"] = UserId;
                dr["ParameterType"] = OleDbType.VarChar;
                dtParam.Rows.Add(dr);
                dr = dtParam.NewRow();
                dr["ParameterName"] = "BloodType";
                dr["ParameterValue"] = BloodType;
                dr["ParameterType"] = OleDbType.VarChar;
                dtParam.Rows.Add(dr);
                dr = dtParam.NewRow();
                dr["ParameterName"] = "DonationCounter";
                dr["ParameterValue"] = Counter;
                dr["ParameterType"] = OleDbType.Integer;
                dtParam.Rows.Add(dr);
                dr = dtParam.NewRow();
                dr["ParameterName"] = "IsLocationAv";
                dr["ParameterValue"] = IsLocationAv;
                dr["ParameterType"] = OleDbType.Boolean;
                dtParam.Rows.Add(dr);
                dr = dtParam.NewRow();
                dr["ParameterName"] = "IsAlltimes";
                dr["ParameterValue"] = IsAlltimes;
                dr["ParameterType"] = OleDbType.Boolean;
                dtParam.Rows.Add(dr);
                dr = dtParam.NewRow();
                dr["ParameterName"] = "IsAvaiable";
                dr["ParameterValue"] = IsAvaiable;
                dr["ParameterType"] = OleDbType.Boolean;
                dtParam.Rows.Add(dr);
                dr = dtParam.NewRow();
                dr["ParameterName"] = "IsMessageAv";
                dr["ParameterValue"] = IsMessageAv;
                dr["ParameterType"] = OleDbType.Boolean;
                dtParam.Rows.Add(dr);

                MyDal.ExecQueryScalar("Blood_Donation_User_Register ?,?,?,?,?,?,?", dtParam);



                #endregion

                return true;
            }
            catch (Exception exc)
            {

                string hata = exc.Message;
                if (exc.InnerException != null)
                {
                    hata += exc.InnerException.Message;
                }

                BloodUtil.WriteEventLogRecord("BloodDonation", "BloodDonation", "Registeration-Error : " + hata, System.Diagnostics.EventLogEntryType.Error);

                return false;
            }
        }

        [WebMethod(Description = "BloodDonation User Settings")]
        public User SelectUser(string UserId)
        {
            BloodDonation.Requirements.Utilities BloodUtil = new BloodDonation.Requirements.Utilities();
            try
            {

                BloodUtil.WriteEventLogRecord("BloodDonation", "BloodDonation", "User-Select-Start", System.Diagnostics.EventLogEntryType.Information);



                #region Config Read

                string Server;
                string User;
                string Password;
                string DatabaseName;

                try
                {
                    DataSet ds = new DataSet();
                    string strConfigFileName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.ToString()) + "\\Config.xml";
                    ds.ReadXml(strConfigFileName);

                    Server = ds.Tables[0].Rows[0]["Server"].ToString();
                    User = ds.Tables[0].Rows[0]["User"].ToString();
                    Password = ds.Tables[0].Rows[0]["Password"].ToString();
                    DatabaseName = ds.Tables[0].Rows[0]["DatabaseName"].ToString();

                }
                catch (Exception exc)
                {
                    throw new Exception("Web service Config could not read", exc);
                }

                #endregion

                #region Database Connection

                BloodDonation.Requirements.DataLayer MyDal = new BloodDonation.Requirements.DataLayer(Server, User, Password, DatabaseName);


                #endregion

                #region Process

                DataTable dtParam = new DataTable();
                dtParam.Columns.Add("ParameterName", typeof(string));
                dtParam.Columns.Add("ParameterValue", typeof(object));
                dtParam.Columns.Add("ParameterType", typeof(object));
                DataRow dr = dtParam.NewRow();
                dr["ParameterName"] = "DonorId";
                dr["ParameterValue"] = UserId;
                dr["ParameterType"] = OleDbType.VarChar;
                dtParam.Rows.Add(dr);

                var dtDonor = MyDal.GetDataFromTable("SELECT * FROM DONORS WHERE DonorId = ?", dtParam);

                User user = new User();

                if (dtDonor.Rows.Count != 0)
                {

                    user.DonorId = dtDonor.Rows[0]["DonorId"].ToString();
                    user.BloodType = dtDonor.Rows[0]["BloodType"].ToString();
                    user.Counter = Convert.ToInt32(dtDonor.Rows[0]["DonationCounter"].ToString());
                    user.IsLocationAv = Convert.ToBoolean(dtDonor.Rows[0]["IsLocationAv"].ToString());
                    user.IsAlltimes = Convert.ToBoolean(dtDonor.Rows[0]["IsAllTimes"].ToString());
                    user.IsAvaiable = Convert.ToBoolean(dtDonor.Rows[0]["IsAvaiable"].ToString());
                    user.IsMessageAv = Convert.ToBoolean(dtDonor.Rows[0]["IsMessageAv"].ToString());


                    return user;
                }

                #endregion

                return null;
            }
            catch (Exception exc)
            {

                string hata = exc.Message;
                if (exc.InnerException != null)
                {
                    hata += exc.InnerException.Message;
                }
                BloodUtil.WriteEventLogRecord("BloodDonation", "Blood-Donation", "User-Select-Error : " + hata, System.Diagnostics.EventLogEntryType.Error);

                return null;
            }
        }

        [WebMethod(Description = "BloodDonation User Update")]
        public bool UserUpdateTime(string UserId, int Day, int Time, bool IsAvaiable)
        {

            BloodDonation.Requirements.Utilities BloodUtil = new BloodDonation.Requirements.Utilities();
            try
            {

                BloodUtil.WriteEventLogRecord("BloodDonation", "BloodDonation", "Update-Time-Start", System.Diagnostics.EventLogEntryType.Information);



                #region Config Read

                string Server;
                string User;
                string Password;
                string DatabaseName;

                try
                {
                    DataSet ds = new DataSet();
                    string strConfigFileName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.ToString()) + "\\Config.xml";
                    ds.ReadXml(strConfigFileName);

                    Server = ds.Tables[0].Rows[0]["Server"].ToString();
                    User = ds.Tables[0].Rows[0]["User"].ToString();
                    Password = ds.Tables[0].Rows[0]["Password"].ToString();
                    DatabaseName = ds.Tables[0].Rows[0]["DatabaseName"].ToString();

                }
                catch (Exception exc)
                {
                    throw new Exception("Web service Config could not read", exc);
                }

                #endregion

                #region Database Connection

                BloodDonation.Requirements.DataLayer MyDal = new BloodDonation.Requirements.DataLayer(Server, User, Password, DatabaseName);


                #endregion

                #region Process

                DataTable dtParam = new DataTable();
                dtParam.Columns.Add("ParameterName", typeof(string));
                dtParam.Columns.Add("ParameterValue", typeof(object));
                dtParam.Columns.Add("ParameterType", typeof(object));
                DataRow dr = dtParam.NewRow();
                dr["ParameterName"] = "IsAvaiable";
                dr["ParameterValue"] = IsAvaiable;
                dr["ParameterType"] = OleDbType.Boolean;
                dtParam.Rows.Add(dr);

                dr = dtParam.NewRow();
                dr["ParameterName"] = "UserId";
                dr["ParameterValue"] = UserId;
                dr["ParameterType"] = OleDbType.VarChar;
                dtParam.Rows.Add(dr);

                dr = dtParam.NewRow();
                dr["ParameterName"] = "Day";
                dr["ParameterValue"] = Day;
                dr["ParameterType"] = OleDbType.Integer;
                dtParam.Rows.Add(dr);

                dr = dtParam.NewRow();
                dr["ParameterName"] = "Time";
                dr["ParameterValue"] = Time;
                dr["ParameterType"] = OleDbType.Integer;
                dtParam.Rows.Add(dr);


                MyDal.ExecQueryScalar("UPDATE TIMES SET IsAvaiable = ? WHERE UserId = ? AND Day = ? AND Time = ? ", dtParam);


                #endregion

                return true;
            }
            catch (Exception exc)
            {
                string hata = exc.Message;
                if (exc.InnerException != null)
                {
                    hata += exc.InnerException.Message;
                }
                BloodUtil.WriteEventLogRecord("BloodDonation", "BloodDonation", "Update-Time-Error : " + hata, System.Diagnostics.EventLogEntryType.Error);

                return false;
            }
        }

        [WebMethod(Description = "BloodDonation User Update General")]
        public bool UserUpdateGeneral(string DonorId, string BloodType, int DonationCounter, bool IsLocationAv, bool IsAlltimes, bool IsAvaiable, bool IsMessageAv)
        {

            BloodDonation.Requirements.Utilities BloodUtil = new BloodDonation.Requirements.Utilities();
            try
            {

                BloodUtil.WriteEventLogRecord("BloodDonation", "BloodDonation", "Update-General-Start", System.Diagnostics.EventLogEntryType.Information);



                #region Config Read

                string Server;
                string User;
                string Password;
                string DatabaseName;

                try
                {
                    DataSet ds = new DataSet();
                    string strConfigFileName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.ToString()) + "\\Config.xml";
                    ds.ReadXml(strConfigFileName);

                    Server = ds.Tables[0].Rows[0]["Server"].ToString();
                    User = ds.Tables[0].Rows[0]["User"].ToString();
                    Password = ds.Tables[0].Rows[0]["Password"].ToString();
                    DatabaseName = ds.Tables[0].Rows[0]["DatabaseName"].ToString();

                }
                catch (Exception exc)
                {
                    throw new Exception("Web service Config could not read", exc);
                }

                #endregion

                #region Database Connection

                BloodDonation.Requirements.DataLayer MyDal = new BloodDonation.Requirements.DataLayer(Server, User, Password, DatabaseName);


                #endregion

                #region Process


                DataTable dtParam = new DataTable();
                dtParam.Columns.Add("ParameterName", typeof(string));
                dtParam.Columns.Add("ParameterValue", typeof(object));
                dtParam.Columns.Add("ParameterType", typeof(object));
                DataRow dr = dtParam.NewRow();
                dr["ParameterName"] = "IsAlltimes";
                dr["ParameterValue"] = IsAlltimes;
                dr["ParameterType"] = OleDbType.Boolean;
                dtParam.Rows.Add(dr);
                dr = dtParam.NewRow();
                dr["ParameterName"] = "IsAvaiable";
                dr["ParameterValue"] = IsAvaiable;
                dr["ParameterType"] = OleDbType.Boolean;
                dtParam.Rows.Add(dr);
                dr = dtParam.NewRow();
                dr["ParameterName"] = "IsMessageAv";
                dr["ParameterValue"] = IsMessageAv;
                dr["ParameterType"] = OleDbType.Boolean;
                dtParam.Rows.Add(dr);
                dr = dtParam.NewRow();
                dr["ParameterName"] = "IsLocationAv";
                dr["ParameterValue"] = IsLocationAv;
                dr["ParameterType"] = OleDbType.Boolean;
                dtParam.Rows.Add(dr);
                dr = dtParam.NewRow();
                dr["ParameterName"] = "DonationCounter";
                dr["ParameterValue"] = DonationCounter;
                dr["ParameterType"] = OleDbType.Integer;
                dtParam.Rows.Add(dr);
                dr = dtParam.NewRow();
                dr["ParameterName"] = "BloodType";
                dr["ParameterValue"] = BloodType;
                dr["ParameterType"] = OleDbType.VarChar;
                dtParam.Rows.Add(dr);
                dr = dtParam.NewRow();
                dr["ParameterName"] = "DonorId";
                dr["ParameterValue"] = DonorId;
                dr["ParameterType"] = OleDbType.VarChar;
                dtParam.Rows.Add(dr);


                MyDal.ExecQueryScalar("UPDATE DONORS SET IsAlltimes = ?, IsAvaiable = ?, IsMessageAv = ?, IsLocationAv = ?, DonationCounter = ?, BloodType = ? WHERE DonorId = ? ", dtParam);

                #endregion

                return true;
            }
            catch (Exception exc)
            {
                string hata = exc.Message;
                if (exc.InnerException != null)
                {
                    hata += exc.InnerException.Message;
                }
                BloodUtil.WriteEventLogRecord("BloodDonation", "BloodDonation", "Update-General-Error : " + hata, System.Diagnostics.EventLogEntryType.Error);

                return false;
            }
        }

        [WebMethod(Description = "BloodDonation User Time Settings")]
        public Times[] SelectUserTime(string UserId)
        {
            BloodDonation.Requirements.Utilities BloodUtil = new BloodDonation.Requirements.Utilities();
            try
            {

                BloodUtil.WriteEventLogRecord("BloodDonation", "BloodDonation", "User-Select-Time-Start", System.Diagnostics.EventLogEntryType.Information);



                #region Config Read

                string Server;
                string User;
                string Password;
                string DatabaseName;

                try
                {
                    DataSet ds = new DataSet();
                    string strConfigFileName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.ToString()) + "\\Config.xml";
                    ds.ReadXml(strConfigFileName);

                    Server = ds.Tables[0].Rows[0]["Server"].ToString();
                    User = ds.Tables[0].Rows[0]["User"].ToString();
                    Password = ds.Tables[0].Rows[0]["Password"].ToString();
                    DatabaseName = ds.Tables[0].Rows[0]["DatabaseName"].ToString();

                }
                catch (Exception exc)
                {
                    throw new Exception("Web service Config could not read", exc);
                }

                #endregion

                #region Database Connection

                BloodDonation.Requirements.DataLayer MyDal = new BloodDonation.Requirements.DataLayer(Server, User, Password, DatabaseName);


                #endregion

                #region Process

                DataTable dtParam = new DataTable();
                dtParam.Columns.Add("ParameterName", typeof(string));
                dtParam.Columns.Add("ParameterValue", typeof(object));
                dtParam.Columns.Add("ParameterType", typeof(object));
                DataRow dr = dtParam.NewRow();
                dr["ParameterName"] = "UserId";
                dr["ParameterValue"] = UserId;
                dr["ParameterType"] = OleDbType.VarChar;
                dtParam.Rows.Add(dr);

                var dtTimes = MyDal.GetDataFromTable("SELECT * FROM TIMES WHERE UserId = ?", dtParam);

                Times[] arrayOfTimes = new Times[dtTimes.Rows.Count];

                for (int i = 0; i < dtTimes.Rows.Count; i++)
                {
                    Times time = new Times();
                    time.UserId = dtTimes.Rows[i]["UserId"].ToString();
                    time.Day = Convert.ToInt32(dtTimes.Rows[i]["Day"].ToString());
                    time.Time = Convert.ToInt32(dtTimes.Rows[i]["Time"].ToString());
                    time.IsAvaiable = Convert.ToBoolean(dtTimes.Rows[i]["IsAvaiable"].ToString());

                    arrayOfTimes[i] = time;
                }
                #endregion

                return arrayOfTimes;
            }
            catch (Exception exc)
            {

                string hata = exc.Message;
                if (exc.InnerException != null)
                {
                    hata += exc.InnerException.Message;
                }
                BloodUtil.WriteEventLogRecord("BloodDonation", "Blood-Donation", "User-Select-Time-Error : " + hata, System.Diagnostics.EventLogEntryType.Error);

                return null;
            }
        }

        [WebMethod(Description = "BloodDonation User Delete")]
        public bool UserDelete(string DonorId)
        {

            BloodDonation.Requirements.Utilities BloodUtil = new BloodDonation.Requirements.Utilities();
            try
            {

                BloodUtil.WriteEventLogRecord("BloodDonation", "BloodDonation", "Delete-Start", System.Diagnostics.EventLogEntryType.Information);



                #region Config Read

                string Server;
                string User;
                string Password;
                string DatabaseName;

                try
                {
                    DataSet ds = new DataSet();
                    string strConfigFileName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.ToString()) + "\\Config.xml";
                    ds.ReadXml(strConfigFileName);

                    Server = ds.Tables[0].Rows[0]["Server"].ToString();
                    User = ds.Tables[0].Rows[0]["User"].ToString();
                    Password = ds.Tables[0].Rows[0]["Password"].ToString();
                    DatabaseName = ds.Tables[0].Rows[0]["DatabaseName"].ToString();

                }
                catch (Exception exc)
                {
                    throw new Exception("Web service Config could not read", exc);
                }

                #endregion

                #region Database Connection

                BloodDonation.Requirements.DataLayer MyDal = new BloodDonation.Requirements.DataLayer(Server, User, Password, DatabaseName);


                #endregion

                #region Process

                DataTable dtParam = new DataTable();
                dtParam.Columns.Add("ParameterName", typeof(string));
                dtParam.Columns.Add("ParameterValue", typeof(object));
                dtParam.Columns.Add("ParameterType", typeof(object));
                DataRow dr = dtParam.NewRow();
                dr["ParameterName"] = "DonorId";
                dr["ParameterValue"] = DonorId;
                dr["ParameterType"] = OleDbType.VarChar;
                dtParam.Rows.Add(dr);


                MyDal.ExecQueryScalar("DELETE FROM DONORS WHERE DonorId = ?", dtParam);


                #endregion

                return true;
            }
            catch (Exception exc)
            {
                string hata = exc.Message;
                if (exc.InnerException != null)
                {
                    hata += exc.InnerException.Message;
                }
                BloodUtil.WriteEventLogRecord("BloodDonation", "BloodDonation", "Delete-Error : " + hata, System.Diagnostics.EventLogEntryType.Error);

                return false;
            }
        }

        [WebMethod(Description = "BloodDonation User Location")]
        public bool UserLocationUpdate(string UserId, string Name, bool IsLocationAv, decimal Latitude, decimal Longitude)
        {
            BloodDonation.Requirements.Utilities BloodUtil = new BloodDonation.Requirements.Utilities();
            try
            {

                BloodUtil.WriteEventLogRecord("BloodDonation", "BloodDonation", "Location-Update-Start", System.Diagnostics.EventLogEntryType.Information);



                #region Config Read

                string Server;
                string User;
                string Password;
                string DatabaseName;

                try
                {
                    DataSet ds = new DataSet();
                    string strConfigFileName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.ToString()) + "\\Config.xml";
                    ds.ReadXml(strConfigFileName);

                    Server = ds.Tables[0].Rows[0]["Server"].ToString();
                    User = ds.Tables[0].Rows[0]["User"].ToString();
                    Password = ds.Tables[0].Rows[0]["Password"].ToString();
                    DatabaseName = ds.Tables[0].Rows[0]["DatabaseName"].ToString();

                }
                catch (Exception exc)
                {
                    throw new Exception("Web service Config could not read", exc);
                }

                #endregion

                #region Database Connection

                BloodDonation.Requirements.DataLayer MyDal = new BloodDonation.Requirements.DataLayer(Server, User, Password, DatabaseName);


                #endregion

                #region Process

                DataTable dtParam = new DataTable();
                dtParam.Columns.Add("ParameterName", typeof(string));
                dtParam.Columns.Add("ParameterValue", typeof(object));
                dtParam.Columns.Add("ParameterType", typeof(object));
                DataRow dr = dtParam.NewRow();
                dr["ParameterName"] = "UserId";
                dr["ParameterValue"] = UserId;
                dr["ParameterType"] = OleDbType.VarChar;
                dtParam.Rows.Add(dr);
                dr = dtParam.NewRow();
                dr["ParameterName"] = "Name";
                dr["ParameterValue"] = Name;
                dr["ParameterType"] = OleDbType.VarChar;
                dtParam.Rows.Add(dr);
                dr = dtParam.NewRow();
                dr["ParameterName"] = "IsLocationAv";
                dr["ParameterValue"] = IsLocationAv;
                dr["ParameterType"] = OleDbType.Boolean;
                dtParam.Rows.Add(dr);
                dr = dtParam.NewRow();
                dr["ParameterName"] = "Latitude";
                dr["ParameterValue"] = Latitude;
                dr["ParameterType"] = OleDbType.Decimal;
                dtParam.Rows.Add(dr);

                dr = dtParam.NewRow();
                dr["ParameterName"] = "Longitude";
                dr["ParameterValue"] = Longitude;
                dr["ParameterType"] = OleDbType.Decimal;
                dtParam.Rows.Add(dr);


                MyDal.ExecQueryScalar("Blood_Donation_Location_Upd ?,?,?,?,?", dtParam);


                #endregion

                return true;
            }
            catch (Exception exc)
            {
                string hata = exc.Message;
                if (exc.InnerException != null)
                {
                    hata += exc.InnerException.Message;
                }
                BloodUtil.WriteEventLogRecord("BloodDonation", "BloodDonation", "Location-Update-Error" + hata, System.Diagnostics.EventLogEntryType.Error);

                return false;
            }
        }

        [WebMethod(Description = "BloodDonation User Location Delete")]
        public bool UserLocationDelete(string UserId, string Name)
        {
            BloodDonation.Requirements.Utilities BloodUtil = new BloodDonation.Requirements.Utilities();
            try
            {

                BloodUtil.WriteEventLogRecord("BloodDonation", "BloodDonation", "Location-Delete-Start", System.Diagnostics.EventLogEntryType.Information);



                #region Config Read

                string Server;
                string User;
                string Password;
                string DatabaseName;

                try
                {
                    DataSet ds = new DataSet();
                    string strConfigFileName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.ToString()) + "\\Config.xml";
                    ds.ReadXml(strConfigFileName);

                    Server = ds.Tables[0].Rows[0]["Server"].ToString();
                    User = ds.Tables[0].Rows[0]["User"].ToString();
                    Password = ds.Tables[0].Rows[0]["Password"].ToString();
                    DatabaseName = ds.Tables[0].Rows[0]["DatabaseName"].ToString();

                }
                catch (Exception exc)
                {
                    throw new Exception("Web service Config could not read", exc);
                }

                #endregion

                #region Database Connection

                BloodDonation.Requirements.DataLayer MyDal = new BloodDonation.Requirements.DataLayer(Server, User, Password, DatabaseName);


                #endregion

                #region Process

                DataTable dtParam = new DataTable();
                dtParam.Columns.Add("ParameterName", typeof(string));
                dtParam.Columns.Add("ParameterValue", typeof(object));
                dtParam.Columns.Add("ParameterType", typeof(object));
                DataRow dr = dtParam.NewRow();
                dr["ParameterName"] = "UserId";
                dr["ParameterValue"] = UserId;
                dr["ParameterType"] = OleDbType.VarChar;
                dtParam.Rows.Add(dr);
                dr = dtParam.NewRow();
                dr["ParameterName"] = "Name";
                dr["ParameterValue"] = Name;
                dr["ParameterType"] = OleDbType.VarChar;
                dtParam.Rows.Add(dr);


                MyDal.ExecQueryScalar("BloodDonation_DeleteLocation ?,?", dtParam);


                #endregion

                return true;
            }
            catch (Exception exc)
            {
                string hata = exc.Message;
                if (exc.InnerException != null)
                {
                    hata += exc.InnerException.Message;
                }
                BloodUtil.WriteEventLogRecord("BloodDonation", "BloodDonation", "Location-Delete-Error" + hata, System.Diagnostics.EventLogEntryType.Error);

                return false;
            }
        }

        [WebMethod(Description = "BloodDonation Select User Location")]
        public List<Hospital> SelectUserLocation(string UserId)
        {
            BloodDonation.Requirements.Utilities BloodUtil = new BloodDonation.Requirements.Utilities();
            try
            {

                BloodUtil.WriteEventLogRecord("BloodDonation", "BloodDonation", "User-Select-Location-Start", System.Diagnostics.EventLogEntryType.Information);



                #region Config Read

                string Server;
                string User;
                string Password;
                string DatabaseName;

                try
                {
                    DataSet ds = new DataSet();
                    string strConfigFileName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.ToString()) + "\\Config.xml";
                    ds.ReadXml(strConfigFileName);

                    Server = ds.Tables[0].Rows[0]["Server"].ToString();
                    User = ds.Tables[0].Rows[0]["User"].ToString();
                    Password = ds.Tables[0].Rows[0]["Password"].ToString();
                    DatabaseName = ds.Tables[0].Rows[0]["DatabaseName"].ToString();

                }
                catch (Exception exc)
                {
                    throw new Exception("Web service Config could not read", exc);
                }

                #endregion

                #region Database Connection

                BloodDonation.Requirements.DataLayer MyDal = new BloodDonation.Requirements.DataLayer(Server, User, Password, DatabaseName);


                #endregion

                #region Process

                DataTable dtParam = new DataTable();
                dtParam.Columns.Add("ParameterName", typeof(string));
                dtParam.Columns.Add("ParameterValue", typeof(object));
                dtParam.Columns.Add("ParameterType", typeof(object));
                DataRow dr = dtParam.NewRow();
                dr["ParameterName"] = "UserId";
                dr["ParameterValue"] = UserId;
                dr["ParameterType"] = OleDbType.VarChar;
                dtParam.Rows.Add(dr);

                var dtLocations = MyDal.GetDataFromStoredProcedure("BloodDonation_HospitalForUser", dtParam);

                List<Hospital> listOfLocations = new List<Hospital>();

                foreach (DataRow drLocation in dtLocations.Rows)
                {
                    Hospital location = new Hospital();
                    location.HospitalId = Convert.ToInt32(drLocation["HospitalId"].ToString());
                    location.Name = drLocation["Name"].ToString();
                    location.Latitude = Convert.ToDecimal(drLocation["Latitude"].ToString());
                    location.Longitude = Convert.ToDecimal(drLocation["Longitude"].ToString());

                    listOfLocations.Add(location);
                }

                #endregion

                return listOfLocations;
            }
            catch (Exception exc)
            {

                string hata = exc.Message;
                if (exc.InnerException != null)
                {
                    hata += exc.InnerException.Message;
                }
                BloodUtil.WriteEventLogRecord("BloodDonation", "Blood-Donation", "User-Select-Location-Error : " + hata, System.Diagnostics.EventLogEntryType.Error);

                return null;
            }
        }

        [WebMethod(Description = "BloodDonation Select Hospital")]
        public List<Hospital> SelectHospital()
        {
            BloodDonation.Requirements.Utilities BloodUtil = new BloodDonation.Requirements.Utilities();
            try
            {

                BloodUtil.WriteEventLogRecord("BloodDonation", "BloodDonation", "User-Select-Location-Start", System.Diagnostics.EventLogEntryType.Information);



                #region Config Read

                string Server;
                string User;
                string Password;
                string DatabaseName;

                try
                {
                    DataSet ds = new DataSet();
                    string strConfigFileName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.ToString()) + "\\Config.xml";
                    ds.ReadXml(strConfigFileName);

                    Server = ds.Tables[0].Rows[0]["Server"].ToString();
                    User = ds.Tables[0].Rows[0]["User"].ToString();
                    Password = ds.Tables[0].Rows[0]["Password"].ToString();
                    DatabaseName = ds.Tables[0].Rows[0]["DatabaseName"].ToString();

                }
                catch (Exception exc)
                {
                    throw new Exception("Web service Config could not read", exc);
                }

                #endregion

                #region Database Connection

                BloodDonation.Requirements.DataLayer MyDal = new BloodDonation.Requirements.DataLayer(Server, User, Password, DatabaseName);


                #endregion

                #region Process


                var dtHospital = MyDal.GetDataFromTable("SELECT * FROM HOSPITALS", null);

                List<Hospital> listOfHosital = new List<Hospital>();

                foreach (DataRow drHospital in dtHospital.Rows)
                {
                    Hospital hospital = new Hospital();
                    hospital.HospitalId = Convert.ToInt32(drHospital["HospitalId"].ToString());
                    hospital.Name = drHospital["Name"].ToString();
                    hospital.City = drHospital["City"].ToString();
                    hospital.District = drHospital["District"].ToString();
                    hospital.Latitude = Convert.ToDecimal(drHospital["Latitude"].ToString());
                    hospital.Longitude = Convert.ToDecimal(drHospital["Longitude"].ToString());

                    listOfHosital.Add(hospital);
                }

                #endregion

                return listOfHosital;
            }
            catch (Exception exc)
            {

                string hata = exc.Message;
                if (exc.InnerException != null)
                {
                    hata += exc.InnerException.Message;
                }
                BloodUtil.WriteEventLogRecord("BloodDonation", "Blood-Donation", "User-Select-Location-Error : " + hata, System.Diagnostics.EventLogEntryType.Error);

                return null;
            }
        }

        [WebMethod(Description = "BloodDonation Control Hospital User")]
        public bool ControlHospitalUser(int HospitalId, string UserName, string UserPassword)
        {
            BloodDonation.Requirements.Utilities BloodUtil = new BloodDonation.Requirements.Utilities();
            try
            {

                BloodUtil.WriteEventLogRecord("BloodDonation", "BloodDonation", "User-Control-Start", System.Diagnostics.EventLogEntryType.Information);



                #region Config Read

                string Server;
                string User;
                string Password;
                string DatabaseName;

                try
                {
                    DataSet ds = new DataSet();
                    string strConfigFileName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.ToString()) + "\\Config.xml";
                    ds.ReadXml(strConfigFileName);

                    Server = ds.Tables[0].Rows[0]["Server"].ToString();
                    User = ds.Tables[0].Rows[0]["User"].ToString();
                    Password = ds.Tables[0].Rows[0]["Password"].ToString();
                    DatabaseName = ds.Tables[0].Rows[0]["DatabaseName"].ToString();

                }
                catch (Exception exc)
                {
                    throw new Exception("Web service Config could not read", exc);
                }

                #endregion

                #region Database Connection

                BloodDonation.Requirements.DataLayer MyDal = new BloodDonation.Requirements.DataLayer(Server, User, Password, DatabaseName);


                #endregion

                #region Process

                DataTable dtParam = new DataTable();
                dtParam.Columns.Add("ParameterName", typeof(string));
                dtParam.Columns.Add("ParameterValue", typeof(object));
                dtParam.Columns.Add("ParameterType", typeof(object));
                DataRow dr = dtParam.NewRow();
                dr["ParameterName"] = "HospitalId";
                dr["ParameterValue"] = HospitalId;
                dr["ParameterType"] = OleDbType.Integer;
                dtParam.Rows.Add(dr);
                dr = dtParam.NewRow();
                dr["ParameterName"] = "UserName";
                dr["ParameterValue"] = UserName;
                dr["ParameterType"] = OleDbType.VarChar;
                dtParam.Rows.Add(dr);
                dr = dtParam.NewRow();
                dr["ParameterName"] = "Password";
                dr["ParameterValue"] = UserPassword;
                dr["ParameterType"] = OleDbType.VarChar;
                dtParam.Rows.Add(dr);


                DataTable dtResult = MyDal.GetDataFromTable("SELECT dbo.Blood_Donation_UserControl(?,?,?)", dtParam);


                #endregion

                return Convert.ToBoolean(dtResult.Rows[0][0].ToString());
            }
            catch (Exception exc)
            {
                string hata = exc.Message;
                if (exc.InnerException != null)
                {
                    hata += exc.InnerException.Message;
                }
                BloodUtil.WriteEventLogRecord("BloodDonation", "BloodDonation", "User-Control-Error" + hata, System.Diagnostics.EventLogEntryType.Error);

                return false;
            }
        }

        [WebMethod(Description = "BloodDonation Select User Notification Accepted")]
        public void SelectUserNotificationAccept(int HospitalId, int UserId, string BloodType)
        {
            BloodDonation.Requirements.Utilities BloodUtil = new BloodDonation.Requirements.Utilities();
            try
            {

                BloodUtil.WriteEventLogRecord("BloodDonation", "BloodDonation", "Select-User-Notification-Accepted", System.Diagnostics.EventLogEntryType.Information);



                #region Config Read

                string Server;
                string User;
                string Password;
                string DatabaseName;

                try
                {
                    DataSet ds = new DataSet();
                    string strConfigFileName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.ToString()) + "\\Config.xml";
                    ds.ReadXml(strConfigFileName);

                    Server = ds.Tables[0].Rows[0]["Server"].ToString();
                    User = ds.Tables[0].Rows[0]["User"].ToString();
                    Password = ds.Tables[0].Rows[0]["Password"].ToString();
                    DatabaseName = ds.Tables[0].Rows[0]["DatabaseName"].ToString();

                }
                catch (Exception exc)
                {
                    throw new Exception("Web service Config could not read", exc);
                }

                #endregion

                #region Database Connection

                BloodDonation.Requirements.DataLayer MyDal = new BloodDonation.Requirements.DataLayer(Server, User, Password, DatabaseName);


                #endregion

                #region Process

                int day = ((int)DateTime.Now.DayOfWeek == 0) ? 6 : ((int)DateTime.Now.DayOfWeek) - 1;

                int time = DateTime.Now.Hour;

                DataTable dtParam = new DataTable();
                dtParam.Columns.Add("ParameterName", typeof(string));
                dtParam.Columns.Add("ParameterValue", typeof(object));
                dtParam.Columns.Add("ParameterType", typeof(object));
                DataRow dr = dtParam.NewRow();
                dr["ParameterName"] = "Day";
                dr["ParameterValue"] = day;
                dr["ParameterType"] = OleDbType.Integer;
                dtParam.Rows.Add(dr);
                dr = dtParam.NewRow();
                dr["ParameterName"] = "Time";
                dr["ParameterValue"] = time;
                dr["ParameterType"] = OleDbType.Integer;
                dtParam.Rows.Add(dr);
                dr = dtParam.NewRow();
                dr["ParameterName"] = "BloodType";
                dr["ParameterValue"] = BloodType;
                dr["ParameterType"] = OleDbType.VarChar;
                dtParam.Rows.Add(dr);


                var dtResult = MyDal.GetDataFromStoredProcedure("BloodDonation_UserAvaiable", dtParam);


                dtParam.Clear();

                dr = dtParam.NewRow();
                dr["ParameterName"] = "HospitalId";
                dr["ParameterValue"] = HospitalId;
                dr["ParameterType"] = OleDbType.Integer;
                dtParam.Rows.Add(dr);

                var dtHospital = MyDal.GetDataFromTable("SELECT * FROM HOSPITALS WHERE HospitalId = ?", dtParam);

                Hospital hs = new Hospital();
                hs.HospitalId = Convert.ToInt32(dtHospital.Rows[0]["HospitalId"].ToString());
                hs.Name = dtHospital.Rows[0]["Name"].ToString();
                hs.Latitude = Convert.ToDecimal(dtHospital.Rows[0]["Latitude"].ToString());
                hs.Longitude = Convert.ToDecimal(dtHospital.Rows[0]["Longitude"].ToString());

                foreach (DataRow drUser in dtResult.Rows)
                {

                    string url = @"http://maps.googleapis.com/maps/api/distancematrix/xml?units=imperial&origins=" + hs.Latitude.ToString().Replace(',', '.') + "," + hs.Longitude.ToString().Replace(',', '.') + "&destinations=" + drUser["Latitude"].ToString().Replace(',', '.') + "," + drUser["Longitude"].ToString().Replace(',', '.');

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    WebResponse response = request.GetResponse();
                    Stream dataStream = response.GetResponseStream();
                    StreamReader sreader = new StreamReader(dataStream);
                    string responsereader = sreader.ReadToEnd();
                    response.Close();

                    DataSet ds = new DataSet();
                    ds.ReadXml(new XmlTextReader(new StringReader(responsereader)));
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables["element"].Rows[0]["status"].ToString() == "OK")
                        {
                            if (Convert.ToDecimal(ds.Tables["distance"].Rows[0]["value"].ToString()) < 10000)
                            {


                                dtParam.Clear();

                                dr = dtParam.NewRow();
                                dr["ParameterName"] = "UserId";
                                dr["ParameterValue"] = drUser["UserId"].ToString();
                                dr["ParameterType"] = OleDbType.VarChar;
                                dtParam.Rows.Add(dr);


                                dr = dtParam.NewRow();
                                dr["ParameterName"] = "HospitalId";
                                dr["ParameterValue"] = HospitalId;
                                dr["ParameterType"] = OleDbType.Integer;
                                dtParam.Rows.Add(dr);

                                dr = dtParam.NewRow();
                                dr["ParameterName"] = "HospitalName";
                                dr["ParameterValue"] = hs.Name;
                                dr["ParameterType"] = OleDbType.VarChar;
                                dtParam.Rows.Add(dr);

                                dr = dtParam.NewRow();
                                dr["ParameterName"] = "BloodType";
                                dr["ParameterValue"] = BloodType;
                                dr["ParameterType"] = OleDbType.VarChar;
                                dtParam.Rows.Add(dr);

                                dr = dtParam.NewRow();
                                dr["ParameterName"] = "Latitude";
                                dr["ParameterValue"] = hs.Latitude;
                                dr["ParameterType"] = OleDbType.Decimal;
                                dtParam.Rows.Add(dr);

                                dr = dtParam.NewRow();
                                dr["ParameterName"] = "Longitude";
                                dr["ParameterValue"] = hs.Longitude;
                                dr["ParameterType"] = OleDbType.Decimal;
                                dtParam.Rows.Add(dr);

                                dr = dtParam.NewRow();
                                dr["ParameterName"] = "HospitalUserId";
                                dr["ParameterValue"] = UserId;
                                dr["ParameterType"] = OleDbType.Integer;
                                dtParam.Rows.Add(dr);


                                var dtIndex = MyDal.GetDataFromTable("INSERT INTO NOTIFICATIONS(UserId, HospitalId, HospitalName, BloodType, Latitude, Longitude, HospitalUserId, NotiDate,State) OUTPUT Inserted.NotificationId VALUES(?,?,?,?,?,?,?,getdate(),0)", dtParam);




                                string ExpoToken = drUser["Token"].ToString();
                                dynamic body = new
                                {
                                    to = ExpoToken,
                                    title = "Blood Donation",
                                    body = "Bir kan talebi var",
                                    sound = "default",
                                    data = new
                                    {
                                        NotificationId = Convert.ToInt32(dtIndex.Rows[0]["NotificationId"].ToString())

                                    }
                                };
                                string responseJson = null;
                                using (WebClient client = new WebClient())
                                {
                                    client.Headers.Add("accept", "application/json");
                                    client.Headers.Add("accept-encoding", "gzip, deflate");
                                    client.Headers.Add("Content-Type", "application/json");
                                    responseJson = client.UploadString("https://exp.host/--/api/v2/push/send", JsonConvert.SerializeObject(body));
                                }
                                var json = JsonConvert.DeserializeObject<dynamic>(responseJson);

                            }
                        }
                        else
                        {

                        }
                    }
                }



                #endregion


            }
            catch (Exception exc)
            {
                string hata = exc.Message;
                if (exc.InnerException != null)
                {
                    hata += exc.InnerException.Message;
                }
                BloodUtil.WriteEventLogRecord("BloodDonation", "BloodDonation", "Select-User-Notification-Accepted-Error" + hata, System.Diagnostics.EventLogEntryType.Error);

                //return false;
            }
        }

        [WebMethod(Description = "BloodDonation Select Notification")]
        public Notification SelectNotification(int NotificationId)
        {
            BloodDonation.Requirements.Utilities BloodUtil = new BloodDonation.Requirements.Utilities();
            try
            {

                BloodUtil.WriteEventLogRecord("BloodDonation", "BloodDonation", "Select-Notification-Start", System.Diagnostics.EventLogEntryType.Information);



                #region Config Read

                string Server;
                string User;
                string Password;
                string DatabaseName;

                try
                {
                    DataSet ds = new DataSet();
                    string strConfigFileName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.ToString()) + "\\Config.xml";
                    ds.ReadXml(strConfigFileName);

                    Server = ds.Tables[0].Rows[0]["Server"].ToString();
                    User = ds.Tables[0].Rows[0]["User"].ToString();
                    Password = ds.Tables[0].Rows[0]["Password"].ToString();
                    DatabaseName = ds.Tables[0].Rows[0]["DatabaseName"].ToString();

                }
                catch (Exception exc)
                {
                    throw new Exception("Web service Config could not read", exc);
                }

                #endregion

                #region Database Connection

                BloodDonation.Requirements.DataLayer MyDal = new BloodDonation.Requirements.DataLayer(Server, User, Password, DatabaseName);


                #endregion

                #region Process

                DataTable dtParam = new DataTable();
                dtParam.Columns.Add("ParameterName", typeof(string));
                dtParam.Columns.Add("ParameterValue", typeof(object));
                dtParam.Columns.Add("ParameterType", typeof(object));
                DataRow dr = dtParam.NewRow();
                dr["ParameterName"] = "NotificationId";
                dr["ParameterValue"] = NotificationId;
                dr["ParameterType"] = OleDbType.Integer;
                dtParam.Rows.Add(dr);

                var dtHospital = MyDal.GetDataFromTable("SELECT * FROM NOTIFICATIONS WHERE NotificationId = ?", dtParam);


                if (dtHospital.Rows.Count > 0)
                {
                    Notification notification = new Notification();
                    notification.NotificationId = Convert.ToInt32(dtHospital.Rows[0]["NotificationId"].ToString());
                    notification.UserId = dtHospital.Rows[0]["UserId"].ToString();
                    notification.HospitalId = Convert.ToInt32(dtHospital.Rows[0]["HospitalId"].ToString());
                    notification.HospitalName = dtHospital.Rows[0]["HospitalName"].ToString();
                    notification.BloodType = dtHospital.Rows[0]["BloodType"].ToString();
                    notification.Latitude = Convert.ToDecimal(dtHospital.Rows[0]["Latitude"].ToString());
                    notification.Longitude = Convert.ToDecimal(dtHospital.Rows[0]["Longitude"].ToString());
                    notification.HospitalUserId = Convert.ToInt32(dtHospital.Rows[0]["HospitalUserId"].ToString());
                    notification.NotiDate = Convert.ToDateTime(dtHospital.Rows[0]["NotiDate"].ToString());
                    notification.State = Convert.ToBoolean(dtHospital.Rows[0]["State"].ToString());


                    return notification;
                }


                #endregion

                return null;
            }
            catch (Exception exc)
            {

                string hata = exc.Message;
                if (exc.InnerException != null)
                {
                    hata += exc.InnerException.Message;
                }
                BloodUtil.WriteEventLogRecord("BloodDonation", "Blood-Donation", "Select-Notification-Error : " + hata, System.Diagnostics.EventLogEntryType.Error);

                return null;
            }
        }

        [WebMethod(Description = "BloodDonation Update Read Notification")]
        public bool UpdateReadNotification(int NotificationId)
        {
            BloodDonation.Requirements.Utilities BloodUtil = new BloodDonation.Requirements.Utilities();
            try
            {

                BloodUtil.WriteEventLogRecord("BloodDonation", "BloodDonation", "Update-Read-Notification-Start", System.Diagnostics.EventLogEntryType.Information);



                #region Config Read

                string Server;
                string User;
                string Password;
                string DatabaseName;

                try
                {
                    DataSet ds = new DataSet();
                    string strConfigFileName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.ToString()) + "\\Config.xml";
                    ds.ReadXml(strConfigFileName);

                    Server = ds.Tables[0].Rows[0]["Server"].ToString();
                    User = ds.Tables[0].Rows[0]["User"].ToString();
                    Password = ds.Tables[0].Rows[0]["Password"].ToString();
                    DatabaseName = ds.Tables[0].Rows[0]["DatabaseName"].ToString();

                }
                catch (Exception exc)
                {
                    throw new Exception("Web service Config could not read", exc);
                }

                #endregion

                #region Database Connection

                BloodDonation.Requirements.DataLayer MyDal = new BloodDonation.Requirements.DataLayer(Server, User, Password, DatabaseName);


                #endregion

                #region Process

                DataTable dtParam = new DataTable();
                dtParam.Columns.Add("ParameterName", typeof(string));
                dtParam.Columns.Add("ParameterValue", typeof(object));
                dtParam.Columns.Add("ParameterType", typeof(object));
                DataRow dr = dtParam.NewRow();

                dr["ParameterName"] = "NotificationId";
                dr["ParameterValue"] = NotificationId;
                dr["ParameterType"] = OleDbType.Integer;
                dtParam.Rows.Add(dr);

                MyDal.ExecQueryScalar("UPDATE NOTIFICATIONS SET IsRead = 1 WHERE NotificationId = ?", dtParam);

                #endregion

                return true;
            }


            catch (Exception exc)
            {

                string hata = exc.Message;
                if (exc.InnerException != null)
                {
                    hata += exc.InnerException.Message;
                }
                BloodUtil.WriteEventLogRecord("BloodDonation", "Blood-Donation", "Update-Read-Notification-Error : " + hata, System.Diagnostics.EventLogEntryType.Error);

                return false;
            }
        }

        [WebMethod(Description = "BloodDonation Update Notification")]
        public bool UpdateStateNotification(int NotificationId)
        {
            BloodDonation.Requirements.Utilities BloodUtil = new BloodDonation.Requirements.Utilities();
            try
            {

                BloodUtil.WriteEventLogRecord("BloodDonation", "BloodDonation", "Select-Notification-Start", System.Diagnostics.EventLogEntryType.Information);



                #region Config Read

                string Server;
                string User;
                string Password;
                string DatabaseName;

                try
                {
                    DataSet ds = new DataSet();
                    string strConfigFileName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.ToString()) + "\\Config.xml";
                    ds.ReadXml(strConfigFileName);

                    Server = ds.Tables[0].Rows[0]["Server"].ToString();
                    User = ds.Tables[0].Rows[0]["User"].ToString();
                    Password = ds.Tables[0].Rows[0]["Password"].ToString();
                    DatabaseName = ds.Tables[0].Rows[0]["DatabaseName"].ToString();

                }
                catch (Exception exc)
                {
                    throw new Exception("Web service Config could not read", exc);
                }

                #endregion

                #region Database Connection

                BloodDonation.Requirements.DataLayer MyDal = new BloodDonation.Requirements.DataLayer(Server, User, Password, DatabaseName);


                #endregion

                #region Process

                DataTable dtParam = new DataTable();
                dtParam.Columns.Add("ParameterName", typeof(string));
                dtParam.Columns.Add("ParameterValue", typeof(object));
                dtParam.Columns.Add("ParameterType", typeof(object));
                DataRow dr = dtParam.NewRow();
                dr["ParameterName"] = "NotificationId";
                dr["ParameterValue"] = NotificationId;
                dr["ParameterType"] = OleDbType.Integer;
                dtParam.Rows.Add(dr);


                var dtHospital = MyDal.GetDataFromTable("UPDATE NOTIFICATIONS SET STATE = 1 WHERE NotificationId = ?", dtParam);


                #endregion

                return true;
            }
            catch (Exception exc)
            {

                string hata = exc.Message;
                if (exc.InnerException != null)
                {
                    hata += exc.InnerException.Message;
                }
                BloodUtil.WriteEventLogRecord("BloodDonation", "Blood-Donation", "Select-Notification-Error : " + hata, System.Diagnostics.EventLogEntryType.Error);

                return false;
            }
        }

        [WebMethod(Description = "BloodDonation Delete User All Locations")]
        public bool DeleteAllUserLocations(string UserId)
        {
            BloodDonation.Requirements.Utilities BloodUtil = new BloodDonation.Requirements.Utilities();
            try
            {

                BloodUtil.WriteEventLogRecord("BloodDonation", "BloodDonation", "Delete-User-All-Locations-Start", System.Diagnostics.EventLogEntryType.Information);



                #region Config Read

                string Server;
                string User;
                string Password;
                string DatabaseName;

                try
                {
                    DataSet ds = new DataSet();
                    string strConfigFileName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.ToString()) + "\\Config.xml";
                    ds.ReadXml(strConfigFileName);

                    Server = ds.Tables[0].Rows[0]["Server"].ToString();
                    User = ds.Tables[0].Rows[0]["User"].ToString();
                    Password = ds.Tables[0].Rows[0]["Password"].ToString();
                    DatabaseName = ds.Tables[0].Rows[0]["DatabaseName"].ToString();

                }
                catch (Exception exc)
                {
                    throw new Exception("Web service Config could not read", exc);
                }

                #endregion

                #region Database Connection

                BloodDonation.Requirements.DataLayer MyDal = new BloodDonation.Requirements.DataLayer(Server, User, Password, DatabaseName);


                #endregion

                #region Process


                DataTable dtParam = new DataTable();
                dtParam.Columns.Add("ParameterName", typeof(string));
                dtParam.Columns.Add("ParameterValue", typeof(object));
                dtParam.Columns.Add("ParameterType", typeof(object));
                DataRow dr = dtParam.NewRow();
                dr["ParameterName"] = "UserId";
                dr["ParameterValue"] = UserId;
                dr["ParameterType"] = OleDbType.VarChar;
                dtParam.Rows.Add(dr);


                MyDal.ExecQueryScalar("DELETE LOCATIONS WHERE UserId = ?", dtParam);



                #endregion

                return true;
            }
            catch (Exception exc)
            {
                string hata = exc.Message;
                if (exc.InnerException != null)
                {
                    hata += exc.InnerException.Message;
                }
                BloodUtil.WriteEventLogRecord("BloodDonation", "BloodDonation", "Delete-User-All-Locations-Error" + hata, System.Diagnostics.EventLogEntryType.Error);

                return false;
            }
        }

        [WebMethod(Description = "BloodDonation Delete User All Locations")]
        public bool UpdateAllUserTimes(string UserId, bool IsAvaiable)
        {
            BloodDonation.Requirements.Utilities BloodUtil = new BloodDonation.Requirements.Utilities();
            try
            {

                BloodUtil.WriteEventLogRecord("BloodDonation", "BloodDonation", "Update-User-All-Times-Start", System.Diagnostics.EventLogEntryType.Information);



                #region Config Read

                string Server;
                string User;
                string Password;
                string DatabaseName;

                try
                {
                    DataSet ds = new DataSet();
                    string strConfigFileName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.ToString()) + "\\Config.xml";
                    ds.ReadXml(strConfigFileName);

                    Server = ds.Tables[0].Rows[0]["Server"].ToString();
                    User = ds.Tables[0].Rows[0]["User"].ToString();
                    Password = ds.Tables[0].Rows[0]["Password"].ToString();
                    DatabaseName = ds.Tables[0].Rows[0]["DatabaseName"].ToString();

                }
                catch (Exception exc)
                {
                    throw new Exception("Web service Config could not read", exc);
                }

                #endregion

                #region Database Connection

                BloodDonation.Requirements.DataLayer MyDal = new BloodDonation.Requirements.DataLayer(Server, User, Password, DatabaseName);


                #endregion

                #region Process


                DataTable dtParam = new DataTable();
                dtParam.Columns.Add("ParameterName", typeof(string));
                dtParam.Columns.Add("ParameterValue", typeof(object));
                dtParam.Columns.Add("ParameterType", typeof(object));
                DataRow dr = dtParam.NewRow();
                dr = dtParam.NewRow();
                dr["ParameterName"] = "IsAvaiable";
                dr["ParameterValue"] = IsAvaiable;
                dr["ParameterType"] = OleDbType.Boolean;
                dtParam.Rows.Add(dr);
                dr = dtParam.NewRow();
                dr["ParameterName"] = "UserId";
                dr["ParameterValue"] = UserId;
                dr["ParameterType"] = OleDbType.VarChar;
                dtParam.Rows.Add(dr);


                MyDal.ExecQueryScalar("UPDATE TIMES SET IsAvaiable = ? WHERE UserId = ?", dtParam);



                #endregion

                return true;
            }
            catch (Exception exc)
            {
                string hata = exc.Message;
                if (exc.InnerException != null)
                {
                    hata += exc.InnerException.Message;
                }
                BloodUtil.WriteEventLogRecord("BloodDonation", "BloodDonation", "Update-User-All-Times-Error" + hata, System.Diagnostics.EventLogEntryType.Error);

                return false;
            }
        }

        [WebMethod(Description = "BloodDonation Add User Token")]
        public bool AddToken(string UserId, string Token)
        {
            BloodDonation.Requirements.Utilities BloodUtil = new BloodDonation.Requirements.Utilities();
            try
            {

                BloodUtil.WriteEventLogRecord("BloodDonation", "BloodDonation", "Token-Start", System.Diagnostics.EventLogEntryType.Information);



                #region Config Read

                string Server;
                string User;
                string Password;
                string DatabaseName;

                try
                {
                    DataSet ds = new DataSet();
                    string strConfigFileName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.ToString()) + "\\Config.xml";
                    ds.ReadXml(strConfigFileName);

                    Server = ds.Tables[0].Rows[0]["Server"].ToString();
                    User = ds.Tables[0].Rows[0]["User"].ToString();
                    Password = ds.Tables[0].Rows[0]["Password"].ToString();
                    DatabaseName = ds.Tables[0].Rows[0]["DatabaseName"].ToString();

                }
                catch (Exception exc)
                {
                    throw new Exception("Web service Config could not read", exc);
                }

                #endregion

                #region Database Connection

                BloodDonation.Requirements.DataLayer MyDal = new BloodDonation.Requirements.DataLayer(Server, User, Password, DatabaseName);


                #endregion

                #region Process


                DataTable dtParam = new DataTable();
                dtParam.Columns.Add("ParameterName", typeof(string));
                dtParam.Columns.Add("ParameterValue", typeof(object));
                dtParam.Columns.Add("ParameterType", typeof(object));
                DataRow dr = dtParam.NewRow();
                dr = dtParam.NewRow();
                dr["ParameterName"] = "UserId";
                dr["ParameterValue"] = UserId;
                dr["ParameterType"] = OleDbType.VarChar;
                dtParam.Rows.Add(dr);
                dr = dtParam.NewRow();
                dr["ParameterName"] = "Token";
                dr["ParameterValue"] = Token;
                dr["ParameterType"] = OleDbType.VarChar;
                dtParam.Rows.Add(dr);


                MyDal.ExecQueryScalar("INSERT INTO TOKENS(UserId,Token) VALUES(?,?)", dtParam);



                #endregion

                return true;
            }
            catch (Exception exc)
            {
                string hata = exc.Message;
                if (exc.InnerException != null)
                {
                    hata += exc.InnerException.Message;
                }
                BloodUtil.WriteEventLogRecord("BloodDonation", "BloodDonation", "Token-Error" + hata, System.Diagnostics.EventLogEntryType.Error);

                return false;
            }
        }

        [WebMethod(Description = "BloodDonation Notification Accepted User List")]
        public DataTable NotificationAccept(int HospitalId)
        {
            BloodDonation.Requirements.Utilities BloodUtil = new BloodDonation.Requirements.Utilities();
            try
            {

                BloodUtil.WriteEventLogRecord("BloodDonation", "BloodDonation", "Noti-Accept-Start", System.Diagnostics.EventLogEntryType.Information);



                #region Config Read

                string Server;
                string User;
                string Password;
                string DatabaseName;

                try
                {
                    DataSet ds = new DataSet();
                    string strConfigFileName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.ToString()) + "\\Config.xml";
                    ds.ReadXml(strConfigFileName);

                    Server = ds.Tables[0].Rows[0]["Server"].ToString();
                    User = ds.Tables[0].Rows[0]["User"].ToString();
                    Password = ds.Tables[0].Rows[0]["Password"].ToString();
                    DatabaseName = ds.Tables[0].Rows[0]["DatabaseName"].ToString();

                }
                catch (Exception exc)
                {
                    throw new Exception("Web service Config could not read", exc);
                }

                #endregion

                #region Database Connection

                BloodDonation.Requirements.DataLayer MyDal = new BloodDonation.Requirements.DataLayer(Server, User, Password, DatabaseName);


                #endregion

                #region Process


                DataTable dtParam = new DataTable();
                dtParam.Columns.Add("ParameterName", typeof(string));
                dtParam.Columns.Add("ParameterValue", typeof(object));
                dtParam.Columns.Add("ParameterType", typeof(object));
                DataRow dr = dtParam.NewRow();
                dr = dtParam.NewRow();
                dr["ParameterName"] = "HospitalId";
                dr["ParameterValue"] = HospitalId;
                dr["ParameterType"] = OleDbType.Integer;
                dtParam.Rows.Add(dr);


                var dtResult = MyDal.GetDataFromStoredProcedure("BloodDonation_Notification_Info", dtParam);

                dtResult.TableName = "AcceptedNotis";

                #endregion

                return dtResult;
            }
            catch (Exception exc)
            {
                string hata = exc.Message;
                if (exc.InnerException != null)
                {
                    hata += exc.InnerException.Message;
                }
                BloodUtil.WriteEventLogRecord("BloodDonation", "BloodDonation", "Noti-Accept-Error" + hata, System.Diagnostics.EventLogEntryType.Error);

                return null;
            }
        }

        [WebMethod(Description = "BloodDonation Notification Finished")]
        public bool NotificationFinished(int HospitalId, string BloodType)
        {
            BloodDonation.Requirements.Utilities BloodUtil = new BloodDonation.Requirements.Utilities();
            try
            {

                BloodUtil.WriteEventLogRecord("BloodDonation", "BloodDonation", "Noti-Finish-Start", System.Diagnostics.EventLogEntryType.Information);



                #region Config Read

                string Server;
                string User;
                string Password;
                string DatabaseName;

                try
                {
                    DataSet ds = new DataSet();
                    string strConfigFileName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.ToString()) + "\\Config.xml";
                    ds.ReadXml(strConfigFileName);

                    Server = ds.Tables[0].Rows[0]["Server"].ToString();
                    User = ds.Tables[0].Rows[0]["User"].ToString();
                    Password = ds.Tables[0].Rows[0]["Password"].ToString();
                    DatabaseName = ds.Tables[0].Rows[0]["DatabaseName"].ToString();

                }
                catch (Exception exc)
                {
                    throw new Exception("Web service Config could not read", exc);
                }

                #endregion

                #region Database Connection

                BloodDonation.Requirements.DataLayer MyDal = new BloodDonation.Requirements.DataLayer(Server, User, Password, DatabaseName);


                #endregion

                #region Process


                DataTable dtParam = new DataTable();
                dtParam.Columns.Add("ParameterName", typeof(string));
                dtParam.Columns.Add("ParameterValue", typeof(object));
                dtParam.Columns.Add("ParameterType", typeof(object));
                DataRow dr = dtParam.NewRow();
                dr["ParameterName"] = "HospitalId";
                dr["ParameterValue"] = HospitalId;
                dr["ParameterType"] = OleDbType.Integer;
                dtParam.Rows.Add(dr);
                dr = dtParam.NewRow();
                dr["ParameterName"] = "BloodType";
                dr["ParameterValue"] = BloodType;
                dr["ParameterType"] = OleDbType.VarChar;
                dtParam.Rows.Add(dr);


                MyDal.ExecQueryScalar("UPDATE NOTIFICATIONS SET IsFinished = 1 WHERE HospitalId = ? AND BloodType = ?", dtParam);

                #endregion

                return true;
            }
            catch (Exception exc)
            {
                string hata = exc.Message;
                if (exc.InnerException != null)
                {
                    hata += exc.InnerException.Message;
                }
                BloodUtil.WriteEventLogRecord("BloodDonation", "BloodDonation", "Noti-Finish-Error" + hata, System.Diagnostics.EventLogEntryType.Error);

                return false;
            }
        }

        [WebMethod(Description = "BloodDonation Send Message")]
        public bool SendMessage(string UserId, int HospitalId, bool IsPerson, string Content)
        {
            BloodDonation.Requirements.Utilities BloodUtil = new BloodDonation.Requirements.Utilities();
            try
            {

                BloodUtil.WriteEventLogRecord("BloodDonation", "BloodDonation", "Send-Message-Start", System.Diagnostics.EventLogEntryType.Information);



                #region Config Read

                string Server;
                string User;
                string Password;
                string DatabaseName;

                try
                {
                    DataSet ds = new DataSet();
                    string strConfigFileName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.ToString()) + "\\Config.xml";
                    ds.ReadXml(strConfigFileName);

                    Server = ds.Tables[0].Rows[0]["Server"].ToString();
                    User = ds.Tables[0].Rows[0]["User"].ToString();
                    Password = ds.Tables[0].Rows[0]["Password"].ToString();
                    DatabaseName = ds.Tables[0].Rows[0]["DatabaseName"].ToString();

                }
                catch (Exception exc)
                {
                    throw new Exception("Web service Config could not read", exc);
                }

                #endregion

                #region Database Connection

                BloodDonation.Requirements.DataLayer MyDal = new BloodDonation.Requirements.DataLayer(Server, User, Password, DatabaseName);


                #endregion

                #region Process

                DataTable dtParam = new DataTable();
                dtParam.Columns.Add("ParameterName", typeof(string));
                dtParam.Columns.Add("ParameterValue", typeof(object));
                dtParam.Columns.Add("ParameterType", typeof(object));
                DataRow dr = dtParam.NewRow();
                dr["ParameterName"] = "UserId";
                dr["ParameterValue"] = UserId;
                dr["ParameterType"] = OleDbType.VarChar;
                dtParam.Rows.Add(dr);
                dr = dtParam.NewRow();
                dr["ParameterName"] = "HospitalId";
                dr["ParameterValue"] = HospitalId;
                dr["ParameterType"] = OleDbType.Integer;
                dtParam.Rows.Add(dr);
                dr = dtParam.NewRow();
                dr["ParameterName"] = "IsPerson";
                dr["ParameterValue"] = IsPerson;
                dr["ParameterType"] = OleDbType.Boolean;
                dtParam.Rows.Add(dr);
                dr = dtParam.NewRow();
                dr["ParameterName"] = "Content";
                dr["ParameterValue"] = Content;
                dr["ParameterType"] = OleDbType.VarChar;
                dtParam.Rows.Add(dr);
                dr = dtParam.NewRow();
                dr["ParameterName"] = "MessageDate";
                dr["ParameterValue"] = DateTime.Now;
                dr["ParameterType"] = OleDbType.Date;
                dtParam.Rows.Add(dr);

                MyDal.ExecQueryScalar("INSERT INTO MESSAGES(UserId, HospitalId, IsPerson, Content, MessageDate) VALUES(?,?,?,?,?)", dtParam);

                if (!IsPerson)
                {

                    dtParam.Clear();

                    dr = dtParam.NewRow();
                    dr["ParameterName"] = "UserId";
                    dr["ParameterValue"] = UserId;
                    dr["ParameterType"] = OleDbType.VarChar;
                    dtParam.Rows.Add(dr);

                    var drUser = MyDal.GetDataFromTable("SELECT Token FROM TOKENS WHERE UserId = ?", dtParam);

                    string ExpoToken = drUser.Rows[0]["Token"].ToString();
                    dynamic body = new
                    {
                        to = ExpoToken,
                        title = "Blood Donation",
                        body = "Yeni Bir Mesaj Var",
                        sound = "default",
                        data = new
                        {
                            HospitalId = HospitalId,
                            Content = Content


                        }
                    };
                    string responseJson = null;
                    using (WebClient client = new WebClient())
                    {
                        client.Headers.Add("accept", "application/json");
                        client.Headers.Add("accept-encoding", "gzip, deflate");
                        client.Headers.Add("Content-Type", "application/json");
                        responseJson = client.UploadString("https://exp.host/--/api/v2/push/send", JsonConvert.SerializeObject(body));
                    }
                    var json = JsonConvert.DeserializeObject<dynamic>(responseJson);
                }

                #endregion

                return true;
            }
            catch (Exception exc)
            {

                string hata = exc.Message;
                if (exc.InnerException != null)
                {
                    hata += exc.InnerException.Message;
                }
                BloodUtil.WriteEventLogRecord("BloodDonation", "Blood-Donation", "Send-Message-Error : " + hata, System.Diagnostics.EventLogEntryType.Error);

                return false;
            }
        }

        [WebMethod(Description = "BloodDonation Get Active Notification")]
        public Notification GetActiveNotification(string UserId)
        {
            BloodDonation.Requirements.Utilities BloodUtil = new BloodDonation.Requirements.Utilities();
            try
            {

                BloodUtil.WriteEventLogRecord("BloodDonation", "BloodDonation", "Active-Notification-Start", System.Diagnostics.EventLogEntryType.Information);



                #region Config Read

                string Server;
                string User;
                string Password;
                string DatabaseName;

                try
                {
                    DataSet ds = new DataSet();
                    string strConfigFileName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.ToString()) + "\\Config.xml";
                    ds.ReadXml(strConfigFileName);

                    Server = ds.Tables[0].Rows[0]["Server"].ToString();
                    User = ds.Tables[0].Rows[0]["User"].ToString();
                    Password = ds.Tables[0].Rows[0]["Password"].ToString();
                    DatabaseName = ds.Tables[0].Rows[0]["DatabaseName"].ToString();

                }
                catch (Exception exc)
                {
                    throw new Exception("Web service Config could not read", exc);
                }

                #endregion

                #region Database Connection

                BloodDonation.Requirements.DataLayer MyDal = new BloodDonation.Requirements.DataLayer(Server, User, Password, DatabaseName);


                #endregion

                #region Process

                DataTable dtParam = new DataTable();
                dtParam.Columns.Add("ParameterName", typeof(string));
                dtParam.Columns.Add("ParameterValue", typeof(object));
                dtParam.Columns.Add("ParameterType", typeof(object));
                DataRow dr = dtParam.NewRow();
                dr["ParameterName"] = "UserId";
                dr["ParameterValue"] = UserId;
                dr["ParameterType"] = OleDbType.VarChar;
                dtParam.Rows.Add(dr);

                var dtResult = MyDal.GetDataFromTable("SELECT * FROM NOTIFICATIONS WHERE UserId = ? AND IsFinished = 0 AND State = 0 AND IsRead = 0", dtParam);


                if (dtResult.Rows.Count == 0)
                {
                    return null;
                }
                Notification notiication = new Notification();

                notiication.NotificationId = Convert.ToInt32(dtResult.Rows[0]["NotificationId"].ToString());

                #endregion

                return notiication;
            }
            catch (Exception exc)
            {

                string hata = exc.Message;
                if (exc.InnerException != null)
                {
                    hata += exc.InnerException.Message;
                }
                BloodUtil.WriteEventLogRecord("BloodDonation", "Blood-Donation", "Active-Notification-Error : " + hata, System.Diagnostics.EventLogEntryType.Error);

                return null;
            }
        }

        [WebMethod(Description = "BloodDonation Get Not Finished Notification")]
        public Notification GetNotFinishedNotification(string UserId)
        {
            BloodDonation.Requirements.Utilities BloodUtil = new BloodDonation.Requirements.Utilities();
            try
            {

                BloodUtil.WriteEventLogRecord("BloodDonation", "BloodDonation", "Not-Finished-Notification-Start", System.Diagnostics.EventLogEntryType.Information);



                #region Config Read

                string Server;
                string User;
                string Password;
                string DatabaseName;

                try
                {
                    DataSet ds = new DataSet();
                    string strConfigFileName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.ToString()) + "\\Config.xml";
                    ds.ReadXml(strConfigFileName);

                    Server = ds.Tables[0].Rows[0]["Server"].ToString();
                    User = ds.Tables[0].Rows[0]["User"].ToString();
                    Password = ds.Tables[0].Rows[0]["Password"].ToString();
                    DatabaseName = ds.Tables[0].Rows[0]["DatabaseName"].ToString();

                }
                catch (Exception exc)
                {
                    throw new Exception("Web service Config could not read", exc);
                }

                #endregion

                #region Database Connection

                BloodDonation.Requirements.DataLayer MyDal = new BloodDonation.Requirements.DataLayer(Server, User, Password, DatabaseName);


                #endregion

                #region Process

                DataTable dtParam = new DataTable();
                dtParam.Columns.Add("ParameterName", typeof(string));
                dtParam.Columns.Add("ParameterValue", typeof(object));
                dtParam.Columns.Add("ParameterType", typeof(object));
                DataRow dr = dtParam.NewRow();
                dr["ParameterName"] = "UserId";
                dr["ParameterValue"] = UserId;
                dr["ParameterType"] = OleDbType.VarChar;
                dtParam.Rows.Add(dr);

                var dtResult = MyDal.GetDataFromTable("SELECT * FROM NOTIFICATIONS WHERE UserId = ? AND IsFinished = 0 AND State = 1", dtParam);


                if (dtResult.Rows.Count == 0)
                {
                    return null;
                }
                Notification notiication = new Notification();

                notiication.NotificationId = Convert.ToInt32(dtResult.Rows[0]["NotificationId"].ToString());

                #endregion

                return notiication;
            }
            catch (Exception exc)
            {

                string hata = exc.Message;
                if (exc.InnerException != null)
                {
                    hata += exc.InnerException.Message;
                }
                BloodUtil.WriteEventLogRecord("BloodDonation", "Blood-Donation", "Not-Finished-Notification-Error : " + hata, System.Diagnostics.EventLogEntryType.Error);

                return null;
            }
        }

        [WebMethod(Description = "BloodDonation Get All Messages")]
        public List<Message> GetAllMessages(string UserId, int HospitalId)
        {
            BloodDonation.Requirements.Utilities BloodUtil = new BloodDonation.Requirements.Utilities();
            try
            {

                BloodUtil.WriteEventLogRecord("BloodDonation", "BloodDonation", "Get-Messages-Start", System.Diagnostics.EventLogEntryType.Information);



                #region Config Read

                string Server;
                string User;
                string Password;
                string DatabaseName;

                try
                {
                    DataSet ds = new DataSet();
                    string strConfigFileName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.ToString()) + "\\Config.xml";
                    ds.ReadXml(strConfigFileName);

                    Server = ds.Tables[0].Rows[0]["Server"].ToString();
                    User = ds.Tables[0].Rows[0]["User"].ToString();
                    Password = ds.Tables[0].Rows[0]["Password"].ToString();
                    DatabaseName = ds.Tables[0].Rows[0]["DatabaseName"].ToString();

                }
                catch (Exception exc)
                {
                    throw new Exception("Web service Config could not read", exc);
                }

                #endregion

                #region Database Connection

                BloodDonation.Requirements.DataLayer MyDal = new BloodDonation.Requirements.DataLayer(Server, User, Password, DatabaseName);


                #endregion

                #region Process

                DataTable dtParam = new DataTable();
                dtParam.Columns.Add("ParameterName", typeof(string));
                dtParam.Columns.Add("ParameterValue", typeof(object));
                dtParam.Columns.Add("ParameterType", typeof(object));
                DataRow dr = dtParam.NewRow();
                dr["ParameterName"] = "UserId";
                dr["ParameterValue"] = UserId;
                dr["ParameterType"] = OleDbType.VarChar;
                dtParam.Rows.Add(dr);
                dr = dtParam.NewRow();
                dr["ParameterName"] = "HospitalId";
                dr["ParameterValue"] = HospitalId;
                dr["ParameterType"] = OleDbType.Integer;
                dtParam.Rows.Add(dr);
                dr = dtParam.NewRow();
                dr["ParameterName"] = "Day1";
                dr["ParameterValue"] = DateTime.Now.AddDays(-1);
                dr["ParameterType"] = OleDbType.Date;
                dtParam.Rows.Add(dr);
                dr = dtParam.NewRow();
                dr["ParameterName"] = "Day2";
                dr["ParameterValue"] = DateTime.Now;
                dr["ParameterType"] = OleDbType.Date;
                dtParam.Rows.Add(dr);


                var dtResult = MyDal.GetDataFromTable("SELECT * FROM MESSAGES WHERE UserId = ? AND HospitalId = ? AND MessageDate BETWEEN ? AND ?", dtParam);

                List<Message> listOfMessages = new List<Message>();

                foreach (DataRow drRes in dtResult.Rows)
                {
                    Message message = new Message();
                    message.MessageId = Convert.ToInt32(drRes["MessageId"].ToString());
                    message.HospitalId = Convert.ToInt32(drRes["HospitalId"].ToString());
                    message.UserId = drRes["UserId"].ToString();
                    message.Content = drRes["Content"].ToString();
                    message.IsPerson = Convert.ToBoolean(drRes["IsPerson"].ToString());

                    listOfMessages.Add(message);
                }

                #endregion

                return listOfMessages;
            }
            catch (Exception exc)
            {

                string hata = exc.Message;
                if (exc.InnerException != null)
                {
                    hata += exc.InnerException.Message;
                }
                BloodUtil.WriteEventLogRecord("BloodDonation", "Blood-Donation", "Get-Messages-Error : " + hata, System.Diagnostics.EventLogEntryType.Error);

                return null;
            }
        }
    }
}
