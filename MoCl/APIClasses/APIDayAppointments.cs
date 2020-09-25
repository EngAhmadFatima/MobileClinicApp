using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoCl.APIClasses
{
    public class APIDayAppointments
    {
        public int Id { get; set; }
        public Nullable<System.TimeSpan> Starting { get; set; }
        public Nullable<System.TimeSpan> Ending { get; set; }
        public string State { get; set; }
    }
}