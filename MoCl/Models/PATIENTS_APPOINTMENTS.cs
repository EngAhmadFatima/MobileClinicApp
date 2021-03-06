//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MoCl.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PATIENTS_APPOINTMENTS
    {
        public int Id { get; set; }
        public Nullable<int> PatientId { get; set; }
        public Nullable<int> AppointmentId { get; set; }
        public Nullable<System.DateTime> AppointmentDate { get; set; }
        public Nullable<int> Status { get; set; }
        public string BookedBy { get; set; }
        public Nullable<System.DateTime> BookingDate { get; set; }
    
        public virtual APPOINTMENT_STATE APPOINTMENT_STATE { get; set; }
        public virtual DOCTORS_APPOINTMENT_DETAILS DOCTORS_APPOINTMENT_DETAILS { get; set; }
        public virtual PATIENT PATIENT { get; set; }
        public virtual PATIENTS_APPOINTMENTS PATIENTS_APPOINTMENTS1 { get; set; }
        public virtual PATIENTS_APPOINTMENTS PATIENTS_APPOINTMENTS2 { get; set; }
    }
}
