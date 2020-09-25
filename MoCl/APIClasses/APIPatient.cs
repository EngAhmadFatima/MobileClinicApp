using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoCl.APIClasses
{
    public class APIPatient
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public Nullable<int> Gender { get; set; }
        public string MobaileNumber { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public string Password { get; set; }
    }
}