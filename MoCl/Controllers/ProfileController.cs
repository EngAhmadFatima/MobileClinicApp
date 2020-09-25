using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoCl.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DoctorPublicProfile(int? DoctorId)
        {
            if(DoctorId != null)
            {
                ViewBag.DoctorId = DoctorId;
                return View();
            }
            else
            {
                return RedirectToAction("Index","Index","Home");
            }
        }

        [HttpGet]
        public ActionResult PatientPublicProfile(int? PatientId, int? DoctorId)
        {
            if(Session["DoctorId"] != null && (int)Session["DoctorId"] == DoctorId)
            {
                if (PatientId != null || DoctorId != null)
                {
                    ViewBag.PatientId = PatientId;
                    ViewBag.DoctorId = DoctorId;
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}