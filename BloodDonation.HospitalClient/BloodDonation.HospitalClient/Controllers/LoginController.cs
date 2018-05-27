using BloodDonation.HospitalClient.WebService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BloodDonation.HospitalClient.Controllers
{
    public class LoginController : Controller
    {

        public ActionResult Login()
        {



            DataSet ds = new DataSet();
            try
            {
                string strConfigFileName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.ToString()) + "\\Information.xml";
                ds.ReadXml(strConfigFileName);
                ViewBag.Hospital = ds.Tables[0].Rows[0]["HospitalName"].ToString();
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return View();


        }

        [HttpPost]
        public ActionResult Login(string UserName, string Password)
        {
            if (ModelState.IsValid)
            {
                int HospitalId;
                DataSet ds = new DataSet();
                try
                {
                    string strConfigFileName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.ToString()) + "\\Information.xml";
                    ds.ReadXml(strConfigFileName);
                    HospitalId = Convert.ToInt32(ds.Tables[0].Rows[0]["HospitalId"].ToString());

                    WebServiceClient ws = new WebServiceClient();
                    bool result = ws.ControlHospitalUser(HospitalId, UserName, Password);
                    if (result)
                    {
                        Session["user"] = UserName;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı!");
                    }
                }
                catch (Exception exc)
                {
                    throw exc;
                }
            }
            return View();
        }
    }
}