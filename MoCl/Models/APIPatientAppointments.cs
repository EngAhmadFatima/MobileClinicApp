using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoCl.APIClasses
{
    public class APIPatientAppointments
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public int AppointmentId { get; set; }
        public System.Nullable<DateTime> AppointmentDate { get; set; }
        public System.Nullable<TimeSpan> AppointmentStart { get; set; }
        public System.Nullable<TimeSpan>  AppointmentEnd { get; set; }
        public System.Nullable<DateTime> BookingDate { get; set; }
        public string AppoitmentState { get; set; }
    }
}