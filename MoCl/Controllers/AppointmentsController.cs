using MoCl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Net;

namespace MoCl.Controllers
{
    public class AppointmentsController : Controller
    {
        MOCL_DBEntities db = new MOCL_DBEntities();
        // GET: Appointments
        public ActionResult Index(DateTime? Date, int DoctorId)
        {
            if (Date == null)
            {
                Date = DateTime.Now.Date;
            }
            ViewBag.Date = Date;
            ViewBag.DoctorId = DoctorId;
            return View();
        }

        [HttpGet]
        public ActionResult DoctorDayAppointments(int DoctorId, String SelectedDate)
        {
            if(SelectedDate != null)
            {
                ViewBag.DoctorId = DoctorId;
                ViewBag.SelectedDate = SelectedDate;
                DateTime Date = DateTime.Parse(SelectedDate);
                string  SelectedDay = Date.DayOfWeek.ToString();
                return View(db.DOCTORS_APPOINTMENT_DETAILS.Where(a => a.DOCTORS_AVAILABLE.Day.Trim() == SelectedDay && a.DoctorId == DoctorId).ToList());
            }
            else { return new HttpNotFoundResult(); }
        }

        [HttpGet]
        public ActionResult BookAppointment(int? AppointmentId, String AppointmentDate)
        {
            //if (Session["PatientName"] != null)
            //{
                if (AppointmentId != null)
                {
                    string CurrentURL = System.Web.HttpContext.Current.Request.Url.ToString();
                    //if (Session["PatientName"] != null)
                    //{
                        ViewBag.AppointmentId = AppointmentId;
                        ViewBag.AppointmentDate = AppointmentDate;
                        return View();
                  //  }
                    //else
                    //{
                    //    return RedirectToAction("PatientLogin", "Account", new { returnUrl = CurrentURL });
                    //}
                }
                else { return new HttpNotFoundResult(); }
            //}
            //else return RedirectToAction("PatientLogin", "Account");
        }

        [HttpPost]
        public ActionResult BookAppointment(PATIENTS_APPOINTMENTS PA)
        {
            if (ModelState.IsValid)
            {
                string CurrentURL = System.Web.HttpContext.Current.Request.QueryString.ToString();
                if (Session["PatientName"] != null)
                {
                    db.PATIENTS_APPOINTMENTS.Add(PA);
                    db.SaveChanges();
                    return RedirectToAction("PatientDashboard", "Account");
                }
                else
                {
                    return RedirectToAction("PatientLogin", "Account", new { returnUrl = CurrentURL });
                }
            }
            else return Redirect("Index");
        }

        [HttpGet]
        public ActionResult AddDoctorDayAvailable()
        {
            if (Session["DoctorName"] != null) { return View(); }
            else return RedirectToAction("DoctorLogin","Account");
           
        }

        [HttpPost]
        public ActionResult AddDoctorDayAvailable(DOCTORS_AVAILABLE DA)
        {
            if (ModelState.IsValid)
            {
                if (Session["DoctorName"] != null)
                {
                    db.DOCTORS_AVAILABLE.Add(DA);
                    db.SaveChanges();
                    
                    int DAId = db.DOCTORS_AVAILABLE.Max(u => u.Id);
                    int DocId = (int)db.DOCTORS_AVAILABLE.Find(DAId).DoctorId;
                    TimeSpan ST = (TimeSpan)db.DOCTORS_AVAILABLE.Find(DAId).StartingTime;
                    TimeSpan ET = (TimeSpan)db.DOCTORS_AVAILABLE.Find(DAId).EndingTime;
                    TimeSpan Dur = (TimeSpan)db.DOCTORS_AVAILABLE.Find(DAId).AppointmentDuration;
                    TimeSpan diff = ET - ST;
                    double Count = diff.TotalMinutes / Dur.TotalMinutes;
                   // int d=5;
                    for(int i=0; i< Count; i++)
                    {
                        TimeSpan newTime = ST + Dur;
                        DOCTORS_APPOINTMENT_DETAILS dad = new DOCTORS_APPOINTMENT_DETAILS()
                        {
                            DoctorId = DocId,
                            DoctorAvilableId = DAId,
                            Starting = ST,
                            Ending = newTime
                        };
                        db.DOCTORS_APPOINTMENT_DETAILS.Add(dad);
                        db.SaveChanges();
                        ST = newTime;
                    }

                    return RedirectToAction("Index","Appointments", new {DoctorId = Session["DoctorId"] });
                }
                else return RedirectToAction("DoctorLogin","Account");
            }
            else return new HttpNotFoundResult();
        }

        [HttpGet]
        public ActionResult DoctorBookAppointment(int? AppointmentId, string AppointmentDate, int DoctorId)
        {
            if (Session["DoctorName"] != null)
            {
                if (AppointmentId != null)
                {
                    string CurrentURL = System.Web.HttpContext.Current.Request.Url.ToString();
                    ViewBag.AppointmentId = AppointmentId;
                    ViewBag.AppointmentDate = AppointmentDate;
                    ViewBag.DoctorId = DoctorId;
                    return View();
                }
                else { return new HttpNotFoundResult(); }
            }
            else
            {
                return RedirectToAction("DoctorLogin", "Account");
            }
        }
        [HttpPost]
        public ActionResult DoctorBookAppointment(PATIENTS_APPOINTMENTS PAA)
        {
            if (ModelState.IsValid)
            {
                string CurrentURL = System.Web.HttpContext.Current.Request.QueryString.ToString();
                if (Session["PatientName"] != null)
                {
                    db.PATIENTS_APPOINTMENTS.Add(PAA);
                    db.SaveChanges();
                    return RedirectToAction("PatientDashboard", "Account");
                }
                else
                {
                    return RedirectToAction("PatientLogin", "Account", new { returnUrl = CurrentURL });
                }
            }
            else return Redirect("Index");
        }
        public JsonResult SearchPatienthWithParameter(string phone)
        {
            PATIENT pat=null;
            int count = 0;
            if (phone != "")
            {
              
              //  List<SPECIALTY> allDoctors = new List<SPECIALTY>();
                foreach (var item in db.PATIENTS.Where(a => a.MobaileNumber == phone))
                {
                    count++;
                    pat = new PATIENT
                    {
                        Id = item.Id,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        FatherName = item.FatherName
                    };
                }
                if (count > 0)
                {
                    return Json(pat, JsonRequestBehavior.AllowGet);
                }
                else { return Json("null", JsonRequestBehavior.AllowGet); }

            }
            else
            {
                return Json("null", JsonRequestBehavior.AllowGet);
            }


        }

        [HttpPost]
        public JsonResult AjaxBookAppointment(int PatId, int AppId, string AppDate, int Stat, string Boby, string BoDate)
        {
            DateTime AppointDate = Convert.ToDateTime(AppDate);
            DateTime BookDate = Convert.ToDateTime(BoDate);
            if (ModelState.IsValid)
            {
                PATIENTS_APPOINTMENTS PA = new PATIENTS_APPOINTMENTS()
                {
                    PatientId = PatId,
                    AppointmentId = AppId,
                    AppointmentDate = AppointDate,
                    Status = Stat,
                    BookedBy = Boby,
                    BookingDate = BookDate
                };
            
                try
                {
                    db.PATIENTS_APPOINTMENTS.Add(PA);
                    db.SaveChanges();

                    return Json("تم حجز الموعد.", JsonRequestBehavior.AllowGet);
                }
                catch { return Json("فشل التسجيل !!", JsonRequestBehavior.AllowGet); }

            }else return Json("فشل التسجيل !!", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddAvailableDayHelp()
        {
            return View();
        }
        [HttpGet]
        public ActionResult EditDoctorDayAvailable()
        {
            return View();
        }
        [HttpGet]
        public ActionResult DeleteAppointment(int? AppId)
        {
            if (AppId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PATIENTS_APPOINTMENTS personalDetail = db.PATIENTS_APPOINTMENTS.Find(AppId);
            if (personalDetail == null)
            {
                return HttpNotFound();
            }
            return View(personalDetail);
        }

        [HttpGet]
        public ActionResult DeleteConfirmed(int Id)
        {
            PATIENTS_APPOINTMENTS personalDetail = db.PATIENTS_APPOINTMENTS.Find(Id);
            db.PATIENTS_APPOINTMENTS.Remove(personalDetail);
            db.SaveChanges();
            return RedirectToAction("Index","Home","Index");
        }
    }
}