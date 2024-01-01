using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IP_Project.Models;

namespace IP_Project.Controllers
{
    public class MercedesController : Controller
    {
        private List<Car> getCars(string carInitials)
        {
            var db = new MercedesEntities();
            var unsortedCars = (from car in db.Cars
                               where car.Model.StartsWith(carInitials)
                               select car)
                               .ToList();
            var cars = unsortedCars.OrderBy(c => c.Price).ToList();
            return cars;
        }
        
        public ActionResult C_class()
        {
            return View(getCars("C-"));
        }
        public ActionResult E_class()
        {
            return View(getCars("E-"));
        }

        public ActionResult S_class()
        {
            return View(getCars("S-"));
        }
        public ActionResult GLC_suv()
        {
            return View(getCars("GLC-"));
        }

        public ActionResult GLE_suv()
        {
            return View(getCars("GLE-"));
        }

        public ActionResult GLS_suv()
        {
            return View(getCars("GLS-"));
        }
        public ActionResult CLA_coupe()
        {
            return View(getCars("CLA-"));
        }
        public ActionResult GT4Door_coupe()
        {
            return View(getCars("GT-"));
        }
    }
}