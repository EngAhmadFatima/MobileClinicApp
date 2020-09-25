using MoCl.APIClasses;
using MoCl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MoCl.Controllers
{
    public class AppointmentsAPIController : ApiController
    {
        MOCL_DBEntities db = new MOCL_DBEntities();


        // --> api/AppointmentsAPI/GetDayesOfCalender/Date/DoctorId
        [HttpGet]
        public IEnumerable<DayesOfAppointmentCalender> GetDayesOfCalender(string Prefix, string Profix)
        {
            //string Prefix = "2018-02-03";
            //string Profix = "2";
            DateTime TheDate = DateTime.Parse(Prefix);
            int DoctorId = int.Parse(Profix.Trim());
            var model = new List<DayesOfAppointmentCalender>();
            //List<string> AvilableDayes = db.DOCTORS_AVAILABLE.Where(a => a.Id == DoctorId).Select(x => new DOCTORS_AVAILABLE {
            //    Day = x.Day
            //}).ToList();
            List<string> AvilableDayes = (from a in db.DOCTORS_AVAILABLE where a.DoctorId== DoctorId select a.Day.Trim()).ToList();


            for (int i = 1; i < DateTime.DaysInMonth(TheDate.Year, TheDate.Month)+1;i++)
            {
                var DAC = new DayesOfAppointmentCalender();
                DAC.DayNum = i;
                DateTime newdate = new DateTime(TheDate.Year, TheDate.Month, i,0,0,0,DateTimeKind.Local);
                string DayName = newdate.DayOfWeek.ToString();
                DAC.Available = AvilableDayes.Contains(DayName) ? true : false;
             //   int AppCount = db.DOCTORS_APPOINTMENT_DETAILS.Where(a => a.DOCTORS_AVAILABLE.DoctorId == DoctorId && a.DOCTORS_AVAILABLE.Day == DayName).Count();
                int AppCount= (from a in db.DOCTORS_APPOINTMENT_DETAILS where a.DoctorId == DoctorId && a.DOCTORS_AVAILABLE.Day == DayName select a.Id).Count();
                DAC.NumOfAppointments = AppCount;
                //  int BookedCount = db.PATIENTS_APPOINTMENTS.Where(a => a.DOCTORS_APPOINTMENT_DETAILS.DOCTOR.Id == DoctorId && a.DOCTORS_APPOINTMENT_DETAILS.DOCTORS_AVAILABLE.Day == DayName).Count();
                string ff = newdate.ToShortDateString();
                int BookedCount =  (from a in db.PATIENTS_APPOINTMENTS where a.DOCTORS_APPOINTMENT_DETAILS.DoctorId == DoctorId && a.AppointmentDate == newdate.Date select a.Id).Count();
                DAC.NumOfBokked = BookedCount;
                model.Add(DAC);
            }

            return model;
        }

        // --> api/AppointmentsAPI/GetDayAppointments/DoctorId/AppointmentDate
        [HttpGet]
        public IEnumerable<APIDayAppointments> GetDayAppointments(string Prefix, string Profix)
        {
            int DoctorId = int.Parse(Prefix.Trim());
            DateTime Date = DateTime.Parse(Profix.Trim());

            APIDayAppointments app = new APIDayAppointments();
            List<APIDayAppointments> appList = new List<APIDayAppointments>();
            appList = db.DOCTORS_APPOINTMENT_DETAILS.Where(x => x.DoctorId == DoctorId && x.DOCTORS_AVAILABLE.Day== Date.DayOfWeek.ToString()).Select(r => new APIDayAppointments {
                Id = r.Id,
                Starting = r.Starting,
                Ending = r.Ending
            }).ToList();
            
            foreach(var item in appList)
            {
                int? dd = null;

                try
                {
                    dd = db.PATIENTS_APPOINTMENTS.Where(a => a.AppointmentId == item.Id && a.AppointmentDate == Date.Date).FirstOrDefault().Status;
                    item.State = "محجوز";
                }
                catch
                {
                    item.State = "متوفر";
                }
            }


            return appList;
        }

        // --> api/AppointmentsAPI/IfDoctorDayAvailable/Date/DoctorId/
        [HttpGet]
        public string IfDoctorDayAvailable(string Prefix, string Profix)
        {
            DateTime AppDate = DateTime.Parse(Prefix);
            int DoctorId = int.Parse(Profix.Trim());
            string DayName = AppDate.DayOfWeek.ToString();
            int count = 0;
            foreach(var item in db.DOCTORS_AVAILABLE)
            {
                if(item.Id == DoctorId)
                {
                    count++;
                }
            }
            if (count > 0)
            {
                List<string> AvilableDayes = (from a in db.DOCTORS_AVAILABLE where a.DoctorId == DoctorId select a.Day.Trim()).ToList();
                if (AvilableDayes.Count() > 0)
                {
                    return AvilableDayes.Contains(DayName) ? "true" : "false";
                }
                else
                {
                    return "false";
                }
            }
            else
            {
                return "false";
            }
         
           
        }

        // --> api/AppointmentsAPI/GetPatientAppointments/PatientId
        [HttpGet]
        public IEnumerable<APIPatientAppointments> GetPatientAppointments(string Prefix)
        {
            int PatientId = int.Parse(Prefix.Trim());
            List<APIPatientAppointments> ListApp = new List<APIPatientAppointments>();
            IEnumerable<PATIENTS_APPOINTMENTS> gg = db.PATIENTS_APPOINTMENTS.Where(a => a.PatientId == PatientId).ToList();
            if(gg.Count() > 0)
            {
                foreach(var item in gg)
                {
                    APIPatientAppointments patapp = new APIPatientAppointments();
                    patapp.PatientId = PatientId;
                    patapp.DoctorId = (int)item.DOCTORS_APPOINTMENT_DETAILS.DoctorId;
                    patapp.DoctorName = db.DOCTORS.Find(patapp.DoctorId).FullName;
                    patapp.AppointmentId = (int)item.AppointmentId;
                    patapp.AppointmentDate = item.AppointmentDate;
                    patapp.AppointmentStart = item.DOCTORS_APPOINTMENT_DETAILS.Starting;
                    patapp.AppointmentEnd = item.DOCTORS_APPOINTMENT_DETAILS.Ending;
                    patapp.BookingDate = item.BookingDate;
                    DateTime? newdate = patapp.AppointmentDate + patapp.AppointmentStart;
                    if (newdate < DateTime.Now)
                    {
                        patapp.AppoitmentState = "مضى";
                    }
                    else patapp.AppoitmentState = "انتظار";

                    ListApp.Add(patapp);
                }
            }
            IEnumerable<APIPatientAppointments> ff = ListApp;
            return ff;
        }

        // --> api/AppointmentsAPI/BookAppointment
        [HttpPost]
        public IHttpActionResult BookAppointment(PATIENTS_APPOINTMENTS pat)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            db.PATIENTS_APPOINTMENTS.Add(pat);
            db.SaveChanges();

            return Ok();
           
        }
    }
}
