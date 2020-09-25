using MoCl.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoCl.Controllers
{
    public class AccountController : Controller
    {
        private MOCL_DBEntities db = new MOCL_DBEntities();
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PatientRegistration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PatientRegistration(PATIENT Pat)
        {
            if(ModelState.IsValid)
            {
                db.PATIENTS.Add(Pat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpGet]
        public ActionResult PatientLogin()
        {
            if (Session["PatientName"] == null)
            {
                return View();
            }
            else return RedirectToAction("PatientDashboard", "Account");
          
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PatientLogin(PATIENT Pat, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                    var details = (from PatientList in db.PATIENTS
                                   where PatientList.MobaileNumber == Pat.MobaileNumber && PatientList.Id == Pat.Id
                                   select new { PatientList.Id, PatientList.FirstName, PatientList.LastName }).ToList();
                    if (details.FirstOrDefault() != null)
                    {
                        Session["PatientId"] = details.FirstOrDefault().Id;
                        Session["PatientName"] = details.FirstOrDefault().FirstName + " " + details.FirstOrDefault().LastName;
                    if (returnUrl != null) { return Redirect(returnUrl); }
                    else { return Redirect("PatientDashboard"); }
                    }
            }
            else { ModelState.AddModelError("","Error"); }
            return View(Pat);
        }

        [HttpGet]
        public ActionResult DoctorRegistration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DoctorRegistration(DOCTOR Doc)
        {
            if (ModelState.IsValid)
            {
                db.DOCTORS.Add(Doc);
                db.SaveChanges();
                return RedirectToAction("");
            }
            return View();
        }

        [HttpGet]
        public ActionResult DoctorLogin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DoctorLogin(DOCTOR Doc)
        {
            if (ModelState.IsValid)
            {
                var details = (from DoctorList in db.DOCTORS
                               where DoctorList.UserName == Doc.UserName && DoctorList.Password == Doc.Password
                               select new { DoctorList.Id, DoctorList.FirstName, DoctorList.LastName }).ToList();
                if (details.FirstOrDefault() != null)
                {
                    Session["DoctorId"] = details.FirstOrDefault().Id;
                    Session["DoctorName"] = details.FirstOrDefault().FirstName + " " + details.FirstOrDefault().LastName;
                    return RedirectToAction("DoctorDashboard","Account");
                }
            }
            else { ModelState.AddModelError("", "Error"); }
            return View(Doc);
        }

        [HttpGet]
        public ActionResult DoctorDashboard()
        {
            if (Session["DoctorName"] != null)
            {
                return View();
            }
            else return RedirectToAction("DoctorLogin", "Account");
           
        }

        [HttpGet]
        public ActionResult PatientDashboard()
        {
            if (Session["PatientName"] != null)
            {
                return View();
            }
            else return RedirectToAction("PatientLogin", "Account");

        }

        [HttpGet]
        public ActionResult PatientLogout()
        {
            Session["PatientName"] = null;
            Session["PatientId"] = null;
            return RedirectToAction("Index","Home");
        }
        public ActionResult DoctorLogout()
        {
            Session["DoctorName"] = null;
            Session["DoctorId"] = null;
            return RedirectToAction("Index", "Home");
        }

        public JsonResult PatientAjaxLogin(int PatId, string PatMob)
        {
            if (ModelState.IsValid)
            {
                var details = (from PatientList in db.PATIENTS
                               where PatientList.MobaileNumber == PatMob && PatientList.Id == PatId
                               select new { PatientList.Id, PatientList.FirstName, PatientList.LastName }).ToList();
                if (details.FirstOrDefault() != null)
                {
                    Session["PatientId"] = details.FirstOrDefault().Id;
                    Session["PatientName"] = details.FirstOrDefault().FirstName + " " + details.FirstOrDefault().LastName;
                }
            }
            return Json(Session["PatientId"], JsonRequestBehavior.AllowGet);
        }
        public JsonResult PatientAjaxLogout()
        {
            Session["PatientId"] = null;
            Session["PatientName"] = null;
            return Json("LogedOut", JsonRequestBehavior.AllowGet);
        }
        public JsonResult PatientAjaxFindPhone(string Phone)
        {
            string result = "Not Found";
            var details = (from PatientList in db.PATIENTS
                           where PatientList.MobaileNumber == Phone.Trim()
                           select new { PatientList.Id, PatientList.FirstName, PatientList.LastName }).ToList();
            if (details.FirstOrDefault() != null)
            {
                result = "Found";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult PatientAjaxRegistration(string FirstName, string LastName, string Phone, string Country, string City, DateTime BirthDate, string Password)
        {
            PATIENT pat = new PATIENT()
            {
                FirstName = FirstName,
                LastName = LastName,
                MobaileNumber = Phone,
                Country = Country,
                City = City,
                BirthDate = BirthDate,
                Password = Password
            };
            
              
               
                var details = (from PatientList in db.PATIENTS
                               where PatientList.MobaileNumber == pat.MobaileNumber.Trim()
                               select new { PatientList.Id, PatientList.FirstName, PatientList.LastName }).ToList();
            if (details.FirstOrDefault() != null)
            {
                return Json("Found", JsonRequestBehavior.AllowGet);
            }
            else
            {
                try
                {
                    db.PATIENTS.Add(pat);
                    db.SaveChanges();

                    return Json("تم التسجيل بنجاح.", JsonRequestBehavior.AllowGet);
                }
                catch { return Json("فشل التسجيل !!", JsonRequestBehavior.AllowGet); }
               
            }
            

        }
        public JsonResult DoctorAjaxLogin(string Username, string Password)
        {
            if (ModelState.IsValid)
            {
                var details = (from DoctorList in db.DOCTORS
                               where DoctorList.UserName == Username && DoctorList.Password == Password
                               select new { DoctorList.Id, DoctorList.FirstName, DoctorList.LastName }).ToList();
                if (details.FirstOrDefault() != null)
                {
                    Session["DoctorId"] = details.FirstOrDefault().Id;
                    Session["DoctorName"] = details.FirstOrDefault().FirstName.Trim() + " " + details.FirstOrDefault().LastName.Trim();
                }
            }
            return Json(Session["DoctorId"], JsonRequestBehavior.AllowGet);
        }
        public JsonResult PatientAjaxRegistration1(string Mobile, string FirstName, string LastName, string FatherName)
        {
            PATIENT pat = new PATIENT()
            {
                FirstName = FirstName,
                LastName = LastName,
                FatherName = FatherName,
                MobaileNumber = Mobile
            };
            try
            {
                db.PATIENTS.Add(pat);
                db.SaveChanges();
                int PatId = db.PATIENTS.Max(a => a.Id);

                return Json(PatId, JsonRequestBehavior.AllowGet);
            }
            catch { return Json("فشل التسجيل !!", JsonRequestBehavior.AllowGet); }

        }
        public JsonResult PatientAjaxRegistration2(string Mobile, string FirstName, string LastName, string FatherName)
        {
            PATIENT pat = new PATIENT()
            {
                FirstName = FirstName,
                LastName = LastName,
                FatherName = FatherName,
                MobaileNumber = Mobile
            };

            var details = (from PatientList in db.PATIENTS
                           where PatientList.MobaileNumber == pat.MobaileNumber.Trim()
                           select new { PatientList.Id, PatientList.FirstName, PatientList.LastName }).ToList();
            if (details.FirstOrDefault() != null)
            {
                return Json("Found", JsonRequestBehavior.AllowGet);
            }
            else
            {
                try
                {
                    db.PATIENTS.Add(pat);
                    db.SaveChanges();
                    int PatId = db.PATIENTS.Max(a => a.Id);

                    return Json(PatId, JsonRequestBehavior.AllowGet);
                }
                catch { return Json(0, JsonRequestBehavior.AllowGet); }

            }

        }
        [HttpGet]
        public ActionResult DoctorRegisteration()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DoctorRegisteration(DOCTOR Doc)
        {
            if (ModelState.IsValid)
            {
                db.DOCTORS.Add(Doc);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else { return HttpNotFound(); }
        }

        public JsonResult DoctorAjaxRegistration(string FirstName, string LastName, string FatherName, int Specialization, int Specialty1, int? Specialty2, int? Specialty3, string Country,  int City, string Address, string Phone, string Mobile, string Password)
        {
            if(Specialty2 == 0) { Specialty2 = null; }
            if (Specialty3 == 0) { Specialty3 = null; }
            int id = db.DOCTORS.Max(a => a.Id)+1;
            string username = FirstName + "_" + id;
            DOCTOR doc = new DOCTOR()
            {
                FirstName = FirstName,
                LastName = LastName,
                FatherName = FatherName,
                FullName = FirstName + " " + FatherName + " " + LastName,
                specialization = Specialization,
                Specialty1 = Specialty1,
                Specialty2 = Specialty2,
                Specialty3 = Specialty3,
                Country = Country,
                City = City,
                Address = Address,
                Phone = Phone,
                Mobile = Mobile,
                UserName = username,
                Password = Password
            };
            try
            {
                db.DOCTORS.Add(doc);
                db.SaveChanges();

                return Json(username, JsonRequestBehavior.AllowGet);
            }
            catch { return Json(0, JsonRequestBehavior.AllowGet); }

        }

        [HttpGet]
        public ActionResult PatientDoctors(int? PatId)
        {
            if(Session["PatientName"] == null)
            {
                return Redirect("/Account/PatientLogin");
            }
            else
            {
                var lp = (from a in db.PATIENTS_APPOINTMENTS where a.PatientId == 2 select a.DOCTORS_APPOINTMENT_DETAILS.DoctorId).ToList();

                List<DOCTOR> doclist = new List<DOCTOR>();
                foreach (var item in lp)
                {
                    if (!doclist.Contains(db.DOCTORS.Find(item)))
                    {
                        doclist.Add(db.DOCTORS.Find(item));
                    }
                }
                return View(doclist);
            }
           
        }

        [HttpGet]
        public ActionResult PatientAppointments(int? PatId)
        {
            if (Session["PatientName"] == null)
            {
                return Redirect("/Account/PatientLogin");
            }
            else
            {
                var list = db.PATIENTS_APPOINTMENTS.Where(a => a.PatientId == PatId).OrderBy(a=>a.AppointmentDate).ToList();
                return View(list);
            }

        }

        [HttpGet]
        public ActionResult DoctorEditInfo(int? DocId)
        {
            if (Session["DoctorName"].ToString()=="" || Session["DoctorName"] == null)
            {
              return Redirect("/Account/DoctorLogin");
            }
            else
            {
                return View(db.DOCTORS.Find(DocId));
            }
         
        }
        [HttpPost]
        public ActionResult DoctorEditInfo(DOCTOR Doc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Doc).State = EntityState.Modified; ;
                db.SaveChanges();
                return Redirect("/Account/DoctorDashboard");
            }
            else
            {
                return HttpNotFound();
            }
        }
        [HttpGet]
        public ActionResult DoctorPatients(int? DocId)
        {
            if (Session["DoctorName"] == null || Session["DoctorName"].ToString() == "")
            {
                return Redirect("/Account/DoctorLogin");
            }
            else
            {
                var oo = db.PATIENTS_APPOINTMENTS.Where(a => a.DOCTORS_APPOINTMENT_DETAILS.DoctorId == DocId).ToList();
                List<PATIENT> patlist = new List<PATIENT>();
                foreach (var item in oo)
                {
                    if (item.PatientId != null && !patlist.Contains(db.PATIENTS.Find(item.PatientId))) { patlist.Add(db.PATIENTS.Find(item.PatientId)); }
                }
                return View(patlist);
            }
           
        }
    }
}