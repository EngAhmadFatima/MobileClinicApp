using MoCl.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MoCl
{
    public class Func
    {
       static MOCL_DBEntities db = new MOCL_DBEntities();
        public static string Booked(int id, DateTime date, int PatId, int? DocSess)
        {
           // MOCL_DBEntities db = new MOCL_DBEntities();
            List<PATIENTS_APPOINTMENTS> dd = db.PATIENTS_APPOINTMENTS.ToList();
            int counter = 0;
       foreach(var item in dd)
            {
                if(item.AppointmentId == id && DateTime.Parse(item.AppointmentDate.ToString()).Date == date.Date)
                {
                    counter++;
                }
            }
            if (counter > 0) { return "محجوز"; }
            else
            {
                if(DocSess != null)
                {
                    if (date.Date < DateTime.Now.Date)
                    {
                        return "لم يتم الحجز";
                    }
                    else
                    {
                        return "<a href='DoctorBookAppointment?AppointmentId=" + id + "&AppointmentDate=" + date.ToShortDateString() + "&DoctorId=" + DocSess + "'><button class='btn btn-success'>متوفر</button></a>";
                    }
                }
               else return "<a href='BookAppointment?AppointmentId=" + id + "&AppointmentDate=" + date.ToShortDateString() + "'><button class='btn btn-success'>متوفر</button></a>";
            }

        }

        public static DateTime NewDateTime(DateTime Date, TimeSpan? Time)
        {
            //int Year = Date.Year;
            //int Month = Date.Month;
            //int Day = Date.Day;

            //int Houer = TimeSpan.Parse(Time.ToString()).Hours;
            //int Minuts = TimeSpan.Parse(Time.ToString()).Minutes;

            //DateTime D = new DateTime(Year, Month, Day, Houer, Minuts, 0);
            DateTime D = Date + ((TimeSpan)Time+TimeSpan.Parse("12:00:00"));
            return D;
        }

        public static string IfExpird(DateTime Date, int? DocSess)
        {
            if (DocSess != null)
            {
                return "";
            }
            else
            {
                if (Date < DateTime.Now)
                {
                    return "hidden='hiddin'";
                }
                else return "";
            }
        }

        public static string TheDayName(int SetYear, int SetMonth, int SetDay)
        {
            DateTime date = new DateTime(SetYear, SetMonth, SetDay);
            string Result = "<button id='Calender-Heder' class='btn btn-success disabled'>" + date.DayOfWeek + "</button>";
            return date.DayOfWeek.ToString();
        }

        public static string TheDay(int SetYear, int SetMonth, int SetDay , int DoctorId)
        {
            DateTime SetDate;
            try { SetDate = new DateTime(SetYear, SetMonth, SetDay); } catch { SetDate =  DateTime.Now; }
            string Result = "";
            List<string> Dayes = new List<string>();

            foreach (var item in db.DOCTORS_AVAILABLE)
            {
                if (item.DoctorId == DoctorId)
                {
                    Dayes.Add(item.Day.ToString().Trim());
                }
            }
            if (SetDay > DateTime.DaysInMonth(SetYear, SetMonth))
            { Result = "<button hidden='hidden'></button>"; }
            else
            {
                if (SetDate < DateTime.Today)
                { Result = "<button class='btn btn-default disabled'>" + SetDay + "</button>"; }
                else if (SetDate == DateTime.Today)
                { Result = "<button id ='Calender-item' class='btn btn-default' style='border-color:red; border-style:solid; color:red' >" + SetDay + "</button>"; }
                else if (Dayes.Contains(SetDate.DayOfWeek.ToString().Trim()))
                { Result = "<a href='DoctorDayAppointments?Day=" + SetDate.DayOfWeek.ToString().Trim()+ "&SelectedDate="+ SetDate.ToShortDateString()+ "'><button id = 'Calender-item' class='btn btn-warning' >" + SetDay+ " <br /> " + TheDayName(SetYear, SetMonth, SetDay)+ "</button></a>"; }
                else
                { Result = "<button id = 'Calender-item' class='btn btn-default' >" + SetDay + "</button>"; }
            }
            return Result;
        }

        public static string TheDayButton(int SetYear, int SetMonth, int SetDay, int DoctorId, bool DocLog)
        {
            DateTime SetDate;
            try { SetDate = new DateTime(SetYear, SetMonth, SetDay); } catch { SetDate = DateTime.Now; }
            string Result = "";
            List<string> Dayes = new List<string>();

            foreach (var item in db.DOCTORS_AVAILABLE.Where(a=>a.DoctorId == DoctorId))
            {
                try
                {
                    Dayes.Add(item.Day.ToString().Trim());
                }
                catch { }
                //if (item.DoctorId == DoctorId)
                //{
                   
                //}
            }

            
            if (SetDay > DateTime.DaysInMonth(SetYear, SetMonth))
            { Result = "<button hidden='hidden'></button>"; }
            else
            {
                if (SetDate < DateTime.Today)
                { Result = !DocLog? "id='Btn-Disabled'": "id='Btn-Old'"; }
                else if (SetDate == DateTime.Today)
                { Result = "id='Btn-Today'"; }
                else if (Dayes.Contains(SetDate.DayOfWeek.ToString().Trim()))
                { Result = "id=''"; }
                else
                { Result = ""; }
            }
            return Result;
        }

        public static void DoctorAppointments(int DoctorId, string Day)
        {
            foreach (var item in db.DOCTORS_AVAILABLE)
            {
                if (item.DoctorId == DoctorId && item.Day == Day)
                {

                }
            }
        }

        public static string DifirinceDates(DateTime Date1, DateTime Date2)
        {
            int Year1 = Date1.Year;
            int Month1 = Date1.Month;
            int Day1 = Date1.Day;
            int Hour1 = Date1.Hour;
            int Minute1 = Date1.Minute;

            int Year2 = Date2.Year;
            int Month2 = Date2.Month;
            int Day2 = Date2.Day;
            int Hour2 = Date2.Hour;
            int Minute2 = Date2.Minute;

            if (Date1 > Date2)
            {

                int Year = Year1 - Year2;
                if (Year<0) { Year = 0; }
                int Month = Month1 - Month2;
                if (Month < 0) { Month = 0; }
                int Day = Day1 - Day2;
                int Hour = Hour1 - Hour2;
                if (Hour < 0) { Hour = 0; }
                int Minute = Minute1 - Minute2;
                if (Minute < 0) { Minute = 0; }
                if (Day < 0) { Day = 24 - Day; }

                //  return Year + " Years - " + Month + " Months - " + Day + " Dayes - " + Hour + " Hours - " + Minute + " Minuts.";
                return " "+Date1.Subtract(Date2).Days.ToString("#")+" أيام - "+ Date1.Subtract(Date2).Hours.ToString("#")+" ساعات - "+ Date1.Subtract(Date2).Minutes.ToString("#")+" دقائق.";
            }
            else return "";
        }

        public static string ArabicDayName(string DayName)
        {
            string day = "";
            switch (DayName)
            {
                case "Sunday": day = "الأحد"; break;
                case "Monday": day = "الإثنين"; break;
                case "Tuesday": day = "الثلاثاء"; break;
                case "Wednesday": day = "الأربعاء"; break;
                case "Thursday": day = "الخميس"; break;
                case "Friday": day = "الجمعة"; break;
                case "Saturday": day = "السبت"; break;
            }
            return day;
        }

        public static string Appointmits(int SetYear, int SetMonth, int SetDay, int DoctorId, bool DocLog)
        {
            DateTime SetDate;
            string Result = "";
            try { SetDate = new DateTime(SetYear, SetMonth, SetDay); } catch { SetDate = DateTime.Now; }

            // Doctor_Avilable (DocId,Day) => Id
            // Doctro_Appointments_Daetaile(DocAvailableId) => Id + Count1();
            // Patients_Appointments(AppointmentId) => Count2();

            List<int> AppID = new List<int>();
            var DocAv = db.DOCTORS_AVAILABLE.Where(da => da.DoctorId == DoctorId && da.Day.Trim() == SetDate.DayOfWeek.ToString().Trim()).ToList();
            foreach (var item in DocAv)
            {

                var DocApp = db.DOCTORS_APPOINTMENT_DETAILS.Where(da => da.DoctorAvilableId == item.Id).ToList();
                foreach (var item2 in DocApp)
                {
                    AppID.Add(item2.Id);

                }
            }
            int DocAppCount = AppID.Count();
            int PatAppCount = 0;
            List<int> PatAppID = new List<int>();
            foreach (int id in AppID)
            {
                foreach (var item in db.PATIENTS_APPOINTMENTS)
                {
                    if (item.AppointmentId == id && DateTime.Parse(item.AppointmentDate.ToString()).ToShortDateString() == SetDate.ToShortDateString())
                    {
                        PatAppCount++;
                        //  Result = Result + item.Id.ToString() + "/";
                    }
                }
            }
            if (!DocLog && SetDate >= DateTime.Today) {
                if (DocAppCount > 0 && DocAppCount == PatAppCount) { Result = "<label class='label label-danger'>المواعيد ممتلئة</label>"; }
                else if (DocAppCount == 0) { Result = "<label class='label label-default'>لا يوجد مواعيد</label>"; }
                else if (DocAppCount > 0 && PatAppCount == 0) { Result = "<label class='label label-success'>كل المواعيد متاحة</label>"; }
                else if (DocAppCount > 0 && DocAppCount > PatAppCount) { Result = "<label class='label label-warning'>(" + (DocAppCount - PatAppCount) + ") موعد متاح </label>"; }
            }
            else if(DocLog)
            {
                if (SetDate <= DateTime.Today)
                {
                    if (DocAppCount > 0 && DocAppCount == PatAppCount) { Result = "<label class='label label-danger'>المواعيد ممتلئة</label>"; }
                    else if (DocAppCount == 0) { Result = "<label class='label label-default'>لا يوجد مواعيد</label>"; }
                    else if (DocAppCount > 0 && PatAppCount == 0) { Result = "<label class='label label-success'>لم يتم حجز مواعيد</label>"; }
                    else if (DocAppCount > 0 && DocAppCount > PatAppCount) { Result = "<label class='label label-warning'>(" + (DocAppCount - PatAppCount) + ") موعد كان متاح </label>"; }
                }
                else
                {
                    if (DocAppCount > 0 && DocAppCount == PatAppCount) { Result = "<label class='label label-danger'>المواعيد ممتلئة</label>"; }
                    else if (DocAppCount == 0) { Result = "<label class='label label-default'>لا يوجد مواعيد</label>"; }
                    else if (DocAppCount > 0 && PatAppCount == 0) { Result = "<label class='label label-success'>كل المواعيد متاحة</label>"; }
                    else if (DocAppCount > 0 && DocAppCount > PatAppCount) { Result = "<label class='label label-warning'>(" + (DocAppCount - PatAppCount) + ") موعد متاح </label>"; }
                }

            }
            
            return Result;
        }

        public static TimeSpan dd(TimeSpan Start, TimeSpan End, TimeSpan Duration)
        {
            TimeSpan diff = End - Start;
            return diff;
        }
        public static string ArabicMonthName(int Number)
        {
            string Month = "";
            switch (Number)
            {
                case 1: Month = "كانون الثاني"; break;
                case 2: Month = "شباط"; break;
                case 3: Month = "آذار"; break;
                case 4: Month = "نيسان"; break;
                case 5: Month = "أيار"; break;
                case 6: Month = "حزيران"; break;
                case 7: Month = "تموز"; break;
                case 8: Month = "آب"; break;
                case 9: Month = "أيلول"; break;
                case 10: Month = "تشرين الأول"; break;
                case 11: Month = "تشرين الثاني"; break;
                case 12: Month = "كانون الأول"; break;
            }
            return Month;
        }

        public static string AppointmentStatus(int PatAppId)
        {
          // int stat = (int)db.PATIENTS_APPOINTMENTS.Find(PatAppId).Status;
           int AppId = (int)db.PATIENTS_APPOINTMENTS.Find(PatAppId).AppointmentId;
            DateTime AppDate = (DateTime)db.PATIENTS_APPOINTMENTS.Find(PatAppId).AppointmentDate;
            TimeSpan AppTime = (TimeSpan)db.DOCTORS_APPOINTMENT_DETAILS.Find(AppId).Starting;
            DateTime AppDateTime = NewDateTime(AppDate, AppTime);
           
            if(AppDateTime < DateTime.Now)
            {
                PATIENTS_APPOINTMENTS PatApp = new PATIENTS_APPOINTMENTS();
                PatApp = db.PATIENTS_APPOINTMENTS.Find(PatAppId);
                PatApp.Status = 2;
                db.Entry(PatApp).State= EntityState.Modified; ;
                db.SaveChanges();
            }

            return db.PATIENTS_APPOINTMENTS.Find(PatAppId).APPOINTMENT_STATE.State_ar;

        }
    }
}