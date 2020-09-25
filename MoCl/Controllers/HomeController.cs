using MoCl.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MoCl.APIClasses;
using System.Collections;

namespace MoCl.Controllers
{
    public class HomeController : Controller
    {
        MOCL_DBEntities db = new MOCL_DBEntities();
        public ActionResult Index(string Country)
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult GetDoctorsWithParameter1(string preFix)
        {
            if (preFix != "")
            {
                preFix = preFix.Trim();
                List<DOCTOR_c> allDoctors = new List<DOCTOR_c>();
                IEnumerable<DOCTOR> ff = db.DOCTORS.Where(a => a.FullName.Contains(preFix)).ToList();
                if (ff.Count()>0)
                {
                    foreach (var item in ff)
                    {
                        DOCTOR_c dd = new DOCTOR_c();

                        string city = item.CITy1.City_ar == null ? "city" : item.CITy1.City_ar;
                        string Specialty1 = item.SPECIALTY.Specialty1 == null ? "Specialty1" : item.SPECIALTY.Specialty1;
                        string Specialty2 = item.SPECIALTY.Specialty1 == null ? "Specialty2" : item.SPECIALTY.Specialty1;
                        string Specialty3 = item.SPECIALTY.Specialty1 == null ? "Specialty3" : item.SPECIALTY.Specialty1;
                        
                        dd = new DOCTOR_c
                        {
                            Id = item.Id,
                            FirstName = item.FirstName,
                            LastName = item.LastName,
                            FullName = item.FullName,
                            City = city,
                            Specialty1 = Specialty1,
                            Specialty2 = Specialty2,
                            Specialty3 = Specialty3,
                            Address = item.Address,
                            Phone = item.Phone
                        };
                        allDoctors.Add(dd);
                    }
                    IEnumerable<DOCTOR_c> df = allDoctors;
                    return Json(df, JsonRequestBehavior.AllowGet);
                }
                else { return Json("null", JsonRequestBehavior.AllowGet); }
            }
            else
            {
                return Json("null", JsonRequestBehavior.AllowGet);
            }
          
        }
        public JsonResult GetAllSpecializations()
        {
            int count = 0;
                List<SPECIALIZATION> AllSpecializations = new List<SPECIALIZATION>();
                foreach (var item in db.SPECIALIZATIONS)
                {
                    count++;
                    AllSpecializations.Add(new SPECIALIZATION
                    {
                        Id = item.Id,
                        Specialization1 = item.Specialization1
                    });
                }
                if (count > 0)
                {
                    return Json(AllSpecializations, JsonRequestBehavior.AllowGet);
                }
                else { return Json("null", JsonRequestBehavior.AllowGet); }


        }
        public JsonResult GetSpecialtyWithParameter(string preFix)
        {

            int count = 0;
            if (preFix != "")
            {
                int pre = int.Parse(preFix);
                List<SPECIALTY> allDoctors = new List<SPECIALTY>();
                foreach (var item in db.SPECIALTies.Where(a => a.SpecializationId == pre))
                {
                    count++;
                    allDoctors.Add(new SPECIALTY
                    {
                        Id = item.Id,
                        Specialty1 = item.Specialty1
                    });
                }
                if (count > 0)
                {
                    return Json(allDoctors, JsonRequestBehavior.AllowGet);
                }
                else { return Json("null", JsonRequestBehavior.AllowGet); }

            }
            else
            {
                return Json("null", JsonRequestBehavior.AllowGet);
            }


        }
        public JsonResult GetDoctorsWith3Parameters1(string x, string y, string z)
        {
            int pre1 = int.Parse(x);
            int pre2 = int.Parse(y);
            int pre3 = int.Parse(z);
            
            if (pre3 != 0)
            {
                List<DOCTOR_c> allDoctors = new List<DOCTOR_c>();
                IEnumerable<DOCTOR> ff = db.DOCTORS.Where(a => a.City == pre1 && (a.Specialty1 == pre3 || a.Specialty2 == pre3 || a.Specialty3 == pre3)).ToList();
                if (ff.Count() > 0)
                {
                    foreach (var item in ff)
                    {
                        DOCTOR_c dd = new DOCTOR_c();

                        string city = item.CITy1.City_ar == null ? "city" : item.CITy1.City_ar;
                        string Specialty1 = item.SPECIALTY.Specialty1 == null ? "Specialty1" : item.SPECIALTY.Specialty1;
                        string Specialty2 = item.SPECIALTY.Specialty1 == null ? "Specialty2" : item.SPECIALTY.Specialty1;
                        string Specialty3 = item.SPECIALTY.Specialty1 == null ? "Specialty3" : item.SPECIALTY.Specialty1;

                        dd = new DOCTOR_c
                        {
                            Id = item.Id,
                            FirstName = item.FirstName,
                            LastName = item.LastName,
                            FullName = item.FullName,
                            City = city,
                            Specialty1 = Specialty1,
                            Specialty2 = Specialty2,
                            Specialty3 = Specialty3,
                            Address = item.Address,
                            Phone = item.Phone
                        };
                        allDoctors.Add(dd);
                    }
                    IEnumerable<DOCTOR_c> df = allDoctors;
                    return Json(df, JsonRequestBehavior.AllowGet);
                }
                else { return Json("null", JsonRequestBehavior.AllowGet); }
                //List<DOCTOR_c> allDoctors = new List<DOCTOR_c>();
                //foreach (var item in db.DOCTORS.Where(a => a.City == pre1 && (a.Specialty1 == pre3 || a.Specialty2 == pre3 || a.Specialty3 == pre3)))
                //{
                //    count++;
                //    allDoctors.Add(new DOCTOR_c
                //    {
                //        Id = item.Id,
                //        FirstName = item.FirstName,
                //        LastName = item.LastName,
                //        FullName = item.FullName,
                //        City = item.CITy1.City_ar,
                //        Specialty1 = item.SPECIALTY.Specialty1,
                //        Specialty2 = item.SPECIALTY.Specialty1,
                //        Specialty3 = item.SPECIALTY.Specialty1,
                //        Address = item.Address,
                //        Phone = item.Phone
                //    });
                //}
                //if (count > 0)
                //{
                //    return Json(allDoctors, JsonRequestBehavior.AllowGet);
                //}
                //else { return Json("null", JsonRequestBehavior.AllowGet); }

            }


            else if(pre3 == 0)
            {
                List<DOCTOR_c> allDoctors = new List<DOCTOR_c>();
                IEnumerable<DOCTOR> ff = db.DOCTORS.Where(a => a.City == pre1 && a.specialization == pre2).ToList();
                if (ff.Count() > 0)
                {
                    foreach (var item in ff)
                    {
                        DOCTOR_c dd = new DOCTOR_c();

                        string city = item.CITy1.City_ar == null ? "city" : item.CITy1.City_ar;
                        string Specialty1 = item.SPECIALTY.Specialty1 == null ? "Specialty1" : item.SPECIALTY.Specialty1;
                        string Specialty2 = item.SPECIALTY.Specialty1 == null ? "Specialty2" : item.SPECIALTY.Specialty1;
                        string Specialty3 = item.SPECIALTY.Specialty1 == null ? "Specialty3" : item.SPECIALTY.Specialty1;

                        dd = new DOCTOR_c
                        {
                            Id = item.Id,
                            FirstName = item.FirstName,
                            LastName = item.LastName,
                            FullName = item.FullName,
                            City = city,
                            Specialty1 = Specialty1,
                            Specialty2 = Specialty2,
                            Specialty3 = Specialty3,
                            Address = item.Address,
                            Phone = item.Phone
                        };
                        allDoctors.Add(dd);
                    }
                    IEnumerable<DOCTOR_c> df = allDoctors;
                    return Json(df, JsonRequestBehavior.AllowGet);
                }
                else { return Json("null", JsonRequestBehavior.AllowGet); }

                //List<APIDoctor> allDoctors = new List<APIDoctor>();
                //foreach (var item in db.DOCTORS.Where(a => a.City == pre1 && a.specialization == pre2))
                //{
                //    count++;
                //    allDoctors.Add(new APIDoctor
                //    {
                //        Id = item.Id,
                //        FirstName = item.FirstName,
                //        LastName = item.LastName,
                //        FullName = item.FullName,
                //        City = item.CITy1.City_ar,
                //        Specialty1 = item.SPECIALTY.Specialty1,
                //        Specialty2 = item.SPECIALTY.Specialty1,
                //        Specialty3 = item.SPECIALTY.Specialty1,
                //        Address = item.Address,
                //        Phone = item.Phone
                //    });
                //}
                //if (count > 0)
                //{
                //    return Json(allDoctors, JsonRequestBehavior.AllowGet);
                //}
                //else { return Json("null", JsonRequestBehavior.AllowGet); }
            }
            else
            {
                return Json("null", JsonRequestBehavior.AllowGet);
            }


        }
       
    }
}