using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoCl.APIClasses
{
    public class APISpecialty
    {
        public int Id { get; set; }
        public Nullable<int> SpecializationId { get; set; }
        public string Specialty { get; set; }
    }
}