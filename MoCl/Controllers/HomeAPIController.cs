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
    public class HomeAPIController : ApiController
    {
        MOCL_DBEntities db = new MOCL_DBEntities();

        // --> api/HomeAPI/GetDoctorsByName/Prefix
        [HttpGet]
        public IEnumerable<APIDoctor> GetDoctorsByName(string Prefix)
        {
            IEnumerable<APIDoctor> model = db.DOCTORS.Where(x => x.FullName.Contains(Prefix)).Select(r => new APIDoctor
            {
                Id = r.Id,
                FirstName = r.FirstName,
                LastName = r.LastName,
                FatherName = r.FatherName,
                FullName = r.FullName,
                specialization = r.SPECIALIZATION1.Specialization1,
                Specialty1 = r.SPECIALTY.Specialty1,
                Specialty2 = r.SPECIALTY.Specialty1,
                Specialty3 = r.SPECIALTY.Specialty1,
                Country = r.Country,
                City = r.CITy1.City_ar,
                Address = r.Address,
                Phone = r.Phone,
                Mobile = r.Mobile,
                UserName = r.UserName,
                Password = r.Password
            });
            return model;
        }

        // --> api/HomeAPI/GetDoctorsBy3par/Prefix
        [HttpGet]
        public IEnumerable<APIDoctor> GetDoctorsBy3par(string Prefix)
        {
            string[] dd = Prefix.Trim().Split('-').ToArray();
            int city = int.Parse(dd[0]);
            int specz = int.Parse(dd[1]);
            int spec = int.Parse(dd[2]);

            if (spec != 0)
            {
                IEnumerable<APIDoctor> model = db.DOCTORS.Where(a => a.City == city && (a.Specialty1 == spec || a.Specialty2 == spec || a.Specialty3 == spec)).Select(r => new APIDoctor
                {
                    Id = r.Id,
                    FirstName = r.FirstName,
                    LastName = r.LastName,
                    FatherName = r.FatherName,
                    FullName = r.FullName,
                    specialization = r.SPECIALIZATION1.Specialization1,
                    Specialty1 = r.SPECIALTY.Specialty1,
                    Specialty2 = r.SPECIALTY.Specialty1,
                    Specialty3 = r.SPECIALTY.Specialty1,
                    Country = r.Country,
                    City = r.CITy1.City_ar,
                    Address = r.Address,
                    Phone = r.Phone,
                    Mobile = r.Mobile,
                    UserName = r.UserName,
                    Password = r.Password
                });
                return model;
            }else
            {
                IEnumerable<APIDoctor> model = db.DOCTORS.Where(a => a.City == city && a.specialization == specz).Select(r => new APIDoctor
                {
                    Id = r.Id,
                    FirstName = r.FirstName,
                    LastName = r.LastName,
                    FatherName = r.FatherName,
                    FullName = r.FullName,
                    specialization = r.SPECIALIZATION1.Specialization1,
                    Specialty1 = r.SPECIALTY.Specialty1,
                    Specialty2 = r.SPECIALTY.Specialty1,
                    Specialty3 = r.SPECIALTY.Specialty1,
                    Country = r.Country,
                    City = r.CITy1.City_ar,
                    Address = r.Address,
                    Phone = r.Phone,
                    Mobile = r.Mobile,
                    UserName = r.UserName,
                    Password = r.Password
                });
                return model;
            }
        
        }

        // --> api/HomeAPI/GetSpecializations
        [HttpGet]
        public IEnumerable<APISpecialization> GetSpecializations()
        {
            IEnumerable<APISpecialization> model = db.SPECIALIZATIONS.OrderBy(x => x.Specialization1).Select(r => new APISpecialization
            {
                Id = r.Id,
                Specialization = r.Specialization1
            });
            return model;
        }

        // --> api/HomeAPI/GetSpecialtiesBySpz/Prefix
        [HttpGet]
        public IEnumerable<APISpecialty> GetSpecialtiesBySpz(string Prefix)
        {
            int pre = int.Parse(Prefix.Trim());
            IEnumerable<APISpecialty> model = db.SPECIALTies.Where(x => x.SpecializationId == pre).Select(r => new APISpecialty
            {
                Id = r.Id,
                Specialty = r.Specialty1
            });
            return model;
        }
    }
}
