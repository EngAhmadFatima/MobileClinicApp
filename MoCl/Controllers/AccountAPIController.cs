using MoCl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MoCl.APIClasses;

namespace MoCl.Controllers
{
    public class AccountAPIController : ApiController
    {
        MOCL_DBEntities db = new MOCL_DBEntities();

        // --> api/AccountAPI/DoctorLogin/Username/Password
        [HttpGet]
        public List<string> DoctorLogin(string prefix, string profix)
        {
            string UserName = prefix.Trim();
            string Password = profix.Trim();
            List<string> Doc = new List<string>();
            try
            {
                DOCTOR d = db.DOCTORS.Where(a=>a.UserName==UserName&&a.Password==Password).FirstOrDefault();
                Doc.Add(d.Id.ToString());
                Doc.Add(d.FullName);
            }
            catch{ Doc.Add("error"); }
            return Doc;
             
        }

        // --> api/AccountAPI/PatientLogin/Mobile/Id
        [HttpGet]
        public List<string> PatientLogin(string prefix, string profix)
        {
            string Mobile = prefix.Trim();
            int Id = int.Parse(profix.Trim());
            List<string> Doc = new List<string>();
            try
            {
                PATIENT d = db.PATIENTS.Where(a => a.MobaileNumber == Mobile && a.Id == Id).FirstOrDefault();
                Doc.Add(d.Id.ToString());
                Doc.Add(d.FirstName);
            }
            catch { Doc.Add("error"); }
            return Doc;

        }

        // --> api/AccountAPI/GetPatentsByIdOrMobile/IdorMobile
        [HttpGet]
        public IEnumerable<APIPatient> GetPatentsByIdOrMobile(string prefix)
        {
            int id = int.Parse(prefix.Trim());
            IEnumerable<APIPatient> model = db.PATIENTS.Where(x => x.Id==id || x.MobaileNumber==prefix.Trim()).Select(r => new APIPatient
            {
                Id = r.Id,
                FirstName = r.FirstName,
                LastName = r.LastName,
                FatherName = r.FatherName,
                MobaileNumber = r.MobaileNumber
            });
            return model;
        }

    }
}
