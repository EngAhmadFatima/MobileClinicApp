using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoCl.APIClasses
{
    public class APIDoctor
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public string FullName { get; set; }
        public string specialization { get; set; }
        public string Specialty1 { get; set; }
        public string Specialty2 { get; set; }
        public string Specialty3 { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}