using BloodDonation.HospitalClient.Models;
using BloodDonation.HospitalClient.WebService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace BloodDonation.HospitalClient.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            //if (Session["user"] == null)
            //{
            //    return RedirectToAction("Login", "Login");
            //}
            WebServiceClient ws = new WebServiceClient();
            int HospitalId;
            DataSet ds = new DataSet();
            List<BloodModel> modelList = new List<BloodModel>();

            try
            {
                string strConfigFileName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.ToString()) + "\\Information.xml";
                ds.ReadXml(strConfigFileName);
                HospitalId = Convert.ToInt32(ds.Tables[0].Rows[0]["HospitalId"].ToString());

                DataTable dtResult = ws.NotificationAccept(HospitalId);




                //var BloodTypes = dtResult.AsEnumerable().GroupBy(row => row.Field<String>("BloodType")).Select(grp => new { BloodType = grp.Key, Count = grp.Count() });  //


                #region Fill Table
                foreach (DataRow dr in dtResult.Rows)
                {
                    bool control = false;

                    foreach (BloodModel bloodmodel in modelList)
                    {
                        if (bloodmodel.BloodType == dr["BloodType"].ToString())
                        {
                            control = true;
                            if (Convert.ToBoolean(dr["State"].ToString()))
                            {
                                bloodmodel.Counter++;
                            }
                        }
                        else
                        {
                            control = false;
                        }
                    }
                    if (!control)
                    {

                        BloodModel model = new BloodModel();
                        model.BloodType = dr["BloodType"].ToString();
                        if (Convert.ToBoolean(dr["State"].ToString()))
                        {
                            model.Counter = 1;
                        }
                        else
                        {
                            model.Counter = 0;
                        }
                        modelList.Add(model);
                    }


                }
                #endregion

                List<SelectListItem> li = new List<SelectListItem>();
                li.Add(new SelectListItem() { Text = "A+", Value = "A+" });
                li.Add(new SelectListItem() { Text = "A-", Value = "A-" });
                li.Add(new SelectListItem() { Text = "B+", Value = "B+" });
                li.Add(new SelectListItem() { Text = "B-", Value = "B-" });
                li.Add(new SelectListItem() { Text = "AB+", Value = "AB+" });
                li.Add(new SelectListItem() { Text = "AB-", Value = "AB-" });
                li.Add(new SelectListItem() { Text = "0+", Value = "0+" });
                li.Add(new SelectListItem() { Text = "0-", Value = "0-" });
                ViewBag.BloodType = li;

            }
            catch
            {
                ModelState.AddModelError("", "Kan Talebi Ekranı Yüklenirken Hata ile Karşılaşıldı.");
            }
            return View(modelList);
        }
        [HttpPost]
        public ActionResult Index(string BloodType)
        {
            WebServiceClient ws = new WebServiceClient();
            int HospitalId, UserId;
            List<BloodModel> modelList = new List<BloodModel>();


            DataSet ds = new DataSet();
            try
            {

                string strConfigFileName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.ToString()) + "\\Information.xml";
                ds.ReadXml(strConfigFileName);
                HospitalId = Convert.ToInt32(ds.Tables[0].Rows[0]["HospitalId"].ToString());
                UserId = Convert.ToInt32(ds.Tables[0].Rows[0]["UserId"].ToString());


                bool BloodControl = false, stateControl = false;

                DataTable dtResult = ws.NotificationAccept(HospitalId);

                if (BloodType == "")
                {
                    BloodControl = true;
                    stateControl = true;
                }

                foreach (DataRow dr in dtResult.Rows)
                {
                    if (BloodType.Equals(dr["BloodType"]))
                    {
                        BloodControl = true;
                    }
                }

                if (!BloodControl)
                {
                    ws.SelectUserNotificationAccept(HospitalId, UserId, BloodType);
                }
                else
                {
                    if (stateControl)
                    {
                        ViewBag.JsMessage = "Kan grubu alanı zorunludur!";
                    }
                    else
                    {
                        ViewBag.JsMessage = "Var olan bir kan talebi girdiniz!";
                    }
                }


                List<SelectListItem> li = new List<SelectListItem>();
                li.Add(new SelectListItem() { Text = "A+", Value = "A+" });
                li.Add(new SelectListItem() { Text = "A-", Value = "A-" });
                li.Add(new SelectListItem() { Text = "B+", Value = "B+" });
                li.Add(new SelectListItem() { Text = "B-", Value = "B-" });
                li.Add(new SelectListItem() { Text = "AB+", Value = "AB+" });
                li.Add(new SelectListItem() { Text = "AB-", Value = "AB-" });
                li.Add(new SelectListItem() { Text = "0+", Value = "0+" });
                li.Add(new SelectListItem() { Text = "0-", Value = "0-" });
                ViewBag.BloodType = li;

                #region Fill Table

                dtResult = ws.NotificationAccept(HospitalId);

                foreach (DataRow dr in dtResult.Rows)
                {
                    bool control = false;

                    foreach (BloodModel bloodmodel in modelList)
                    {
                        if (bloodmodel.BloodType == dr["BloodType"].ToString())
                        {
                            control = true;
                            if (Convert.ToBoolean(dr["State"].ToString()))
                            {
                                bloodmodel.Counter++;
                            }
                        }
                        else
                        {
                            control = false;
                        }
                    }
                    if (!control)
                    {

                        BloodModel model = new BloodModel();
                        model.BloodType = dr["BloodType"].ToString();
                        if (Convert.ToBoolean(dr["State"].ToString()))
                        {
                            model.Counter = 1;
                        }
                        else
                        {
                            model.Counter = 0;
                        }
                        modelList.Add(model);
                    }


                }
                #endregion

            }
            catch (Exception exc)
            {
                ModelState.AddModelError("", "Kan talebi oluşturulurken bir hata ile karşılaşıldı.");
                throw exc;
            }

            return View(modelList);
        }


        public ActionResult Delete(string id)
        {
            BloodModel model = new BloodModel();
            if (!id.Contains("-"))
            {
                id = id + "+";
            }
            model.BloodType = id;
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(string id)
        {
            BloodModel model = new BloodModel();
            WebServiceClient ws = new WebServiceClient();
            int HospitalId;
            DataSet ds = new DataSet();
            if (!id.Contains("-"))
            {
                id = id + "+";
            }
            model.BloodType = id;
            try
            {
                string strConfigFileName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.ToString()) + "\\Information.xml";
                ds.ReadXml(strConfigFileName);
                HospitalId = Convert.ToInt32(ds.Tables[0].Rows[0]["HospitalId"].ToString());

                ws.NotificationFinished(HospitalId, model.BloodType);

            }
            catch
            {
                ModelState.AddModelError("", "Kan Talebini Silerken Hata ile Karşılaşıldı");
            }

            return Redirect("~/Home/Index");
        }

        public ActionResult Details(string id)
        {
            List<UserModel> modelList = new List<UserModel>();
            if (!id.Contains("-"))
            {
                id = id + "+";
            }

            WebServiceClient ws = new WebServiceClient();
            int HospitalId;
            DataSet ds = new DataSet();

            try
            {
                string strConfigFileName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.ToString()) + "\\Information.xml";
                ds.ReadXml(strConfigFileName);
                HospitalId = Convert.ToInt32(ds.Tables[0].Rows[0]["HospitalId"].ToString());

                DataTable dtResult = ws.NotificationAccept(HospitalId);


                foreach (DataRow dr in dtResult.Rows)
                {
                    if(dr["BloodType"].ToString() == id && Convert.ToBoolean(dr["State"].ToString()))
                    {
                        UserModel model = new UserModel();
                        model.UserId = dr["UserId"].ToString();
                        model.BloodType = dr["BloodType"].ToString();
                        model.IsMessageAv = Convert.ToBoolean(dr["IsMessageAv"].ToString());

                        modelList.Add(model);
                    }
                }

                return View(modelList);

            }catch
            {
                ModelState.AddModelError("", "Kabul eden kullanıcılar listelenemiyor");
                return Redirect("~/Home/Index");
            }
        }

        public ActionResult Message(string id)
        {
            List<MessageModel> modelList = new List<MessageModel>();
            WebServiceClient ws = new WebServiceClient();
            int HospitalId;
            DataSet ds = new DataSet();

            try
            {
                string strConfigFileName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.ToString()) + "\\Information.xml";
                ds.ReadXml(strConfigFileName);
                HospitalId = Convert.ToInt32(ds.Tables[0].Rows[0]["HospitalId"].ToString());

                var dtResult = ws.GetAllMessages(id,HospitalId);

                

                foreach(var messageModel in dtResult)
                {
                    MessageModel message = new MessageModel();
                    message.MessageId = messageModel.MessageId;
                    message.UserId = messageModel.UserId;
                    message.HospitalId = messageModel.HospitalId;
                    message.IsPerson = messageModel.IsPerson;
                    message.Content = messageModel.Content;
                    modelList.Add(message);
                }

                MessageModel messageView = new MessageModel();
                messageView.HospitalId = HospitalId;
                messageView.UserId = id;
                messageView.IsPerson = false;

                ViewData["modelSend"] = messageView;


                return View(modelList);

            }
            catch
            {
                ModelState.AddModelError("", "Kabul eden kullanıcılar listelenemiyor");
                return Redirect("~/Home/Index");
            }
        }
        [HttpPost]
        public ActionResult Message(string UserId,int HospitalId, string Content)
        {
            if (!string.IsNullOrEmpty(Content))
            {
                WebServiceClient ws = new WebServiceClient();
                ws.SendMessage(UserId, HospitalId,false,Content);
            }
            return RedirectToAction("Message","Home" , UserId);
        }


    }
}