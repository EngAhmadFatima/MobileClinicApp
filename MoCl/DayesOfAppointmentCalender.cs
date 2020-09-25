using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoCl
{
    public class DayesOfAppointmentCalender
    {
        public int DayNum { get; set; }
        public bool Available { get; set; }
        public int NumOfAppointments { get; set; }
        public int NumOfBokked { get; set; }
    }
}