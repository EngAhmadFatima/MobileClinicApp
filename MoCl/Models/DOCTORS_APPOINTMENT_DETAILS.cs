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
    
    public partial class DOCTORS_APPOINTMENT_DETAILS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DOCTORS_APPOINTMENT_DETAILS()
        {
            this.PATIENTS_APPOINTMENTS = new HashSet<PATIENTS_APPOINTMENTS>();
        }
    
        public int Id { get; set; }
        public Nullable<int> DoctorId { get; set; }
        public Nullable<int> DoctorAvilableId { get; set; }
        public Nullable<System.TimeSpan> Starting { get; set; }
        public Nullable<System.TimeSpan> Ending { get; set; }
    
        public virtual DOCTOR DOCTOR { get; set; }
        public virtual DOCTORS_AVAILABLE DOCTORS_AVAILABLE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PATIENTS_APPOINTMENTS> PATIENTS_APPOINTMENTS { get; set; }
    }
}
