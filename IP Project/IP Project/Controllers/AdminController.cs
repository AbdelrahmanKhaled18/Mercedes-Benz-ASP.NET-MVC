using IP_Project.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IP_Project.Controllers
{
    public class AdminController : Controller
    {
        MercedesEntities db = new MercedesEntities();
        public ActionResult AdminCareers()
        {
            var jobList = (from job in db.Careers select job).ToList();
            var locList = (from loc in db.Locations select loc).ToList();
            return View(new AdminCareersModel(jobList, locList));
        }
        public ActionResult AdminParts()
        {
            return View((from acc in db.Accessories select acc).ToList());
        }

        public ActionResult AdminModels()
        {
            return View((from car in db.Cars select car).ToList());
        }

        [HttpPost]
        public ActionResult Insert_Car(HttpPostedFileBase imageFile, FormCollection fc)
        {
            byte[] image;
            using (Stream inputStream = imageFile.InputStream)
            {
                MemoryStream memoryStream = inputStream as MemoryStream;
                if (memoryStream == null)
                {
                    memoryStream = new MemoryStream();
                    inputStream.CopyTo(memoryStream);
                }
                image = memoryStream.ToArray();
            }

            Car car = new Car
            {
                Model = fc["model"],
                Type = fc["carType"],
                Year = Int32.Parse(fc["year"]),
                Horse_Power = Int32.Parse(fc["horsePower"]),
                Engine_Capacity = Int32.Parse(fc["engineCapacity"]),
                Acceleration = Math.Round(Single.Parse(fc["acceleration"]), 1),
                Price = Int32.Parse(fc["price"]),
                Image = image
            };

            db.Cars.Add(car);
            db.SaveChanges();
            return RedirectToAction("AdminModels");
        }


        [HttpPost]
        public ActionResult Insert_Part(HttpPostedFileBase imageFile, FormCollection fc)
        {
            Console.WriteLine(imageFile.ToString());
            byte[] image;
            using (Stream inputStream = imageFile.InputStream)
            {
                MemoryStream memoryStream = inputStream as MemoryStream;
                if (memoryStream == null)
                {
                    memoryStream = new MemoryStream();
                    inputStream.CopyTo(memoryStream);
                }
                image = memoryStream.ToArray();
            }

            Accessory acc = new Accessory
            {
                Name = fc["name"],
                Car_Chassis = fc["chassis"] == "" ? (int?)null : Int32.Parse(fc["chassis"]),
                Type = fc["accType"],
                Price = Int32.Parse(fc["price"]),
                Availability = fc["availability"] != null,
                Image = image
            };

            db.Accessories.Add(acc);
            db.SaveChanges();
            return RedirectToAction("AdminParts");
        }


        [HttpPost]
        public ActionResult Insert_Career(FormCollection fc)
        {
            Career job = new Career
            {
                Job_Title= fc["title"],
                Availability = fc["availability"] != null,
                Department = fc["department"],
                Type = fc["jobType"],
                Location_ID = Int32.Parse(fc["location"])
            };
            db.Careers.Add(job);
            db.SaveChanges();
            return RedirectToAction("AdminCareers");
        }
    }
}