using IP_Project.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace IP_Project.Controllers
{
    public class HomeController : Controller
    {
        MercedesEntities db = new MercedesEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Careers()
        {
            return View((from job in db.Careers select job).ToList());
        }

        public ActionResult Parts()
        {
            return View((from acc in db.Accessories select acc).ToList());
        }

        public ActionResult LoginSignup()
        {
            Session["isAdmin"] = false;
            Session["email"] = null;
            return View(new User());
        }

        public ActionResult BookTestDrive()
        {
            var locList = (from loc in db.Locations select loc).ToList();
            var carList = (from car in db.Cars select car).ToList();
            ViewBag.message = TempData["message"];
            return View(new TestDriveFormModel(locList, carList));
        }

        [HttpPost]
        public ActionResult Login(User userModel)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.FirstOrDefault(u => u.Email == userModel.Email && u.Password == userModel.Password);
                if (user != null)
                {
                    Session["isAdmin"] = user.Admin;
                    Session["email"] = user.Email;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password");
                }
            }

            return View("LoginSignup", userModel);
        }

        [HttpPost]
        public ActionResult Signup(User userModel)
        {
            if (ModelState.IsValid)
            {

                var existingUser = db.Users.FirstOrDefault(u => u.Email == userModel.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "User with this email already exists");
                    return View("LoginSignup", userModel);
                }

                db.Users.Add(userModel);
                db.SaveChanges();
                Session["isAdmin"] = false;
                Session["email"] = userModel.Email;
                return RedirectToAction("Index", "Home");
            }

            return View("LoginSignup", userModel);
        }

        [HttpPost]
        public ActionResult Insert_Drive (FormCollection fc)
        {
            var email = Session["email"].ToString();
            var driveTime = Convert.ToDateTime(fc["date"].ToString() + " " + fc["time"].ToString());

            Test_Drive drive = new Test_Drive
            {
                Email = email,
                Car_Chassis = Int32.Parse(fc["car"]),
                Location_ID = Int32.Parse(fc["location"]),
                Time = driveTime,
            };

            var userBookings = (from td in db.Test_Drive
                                where td.User.Email == drive.Email
                                select td).ToList();
            if (userBookings.Count > 0)
            {
                foreach (var item in userBookings)
                {
                    if (item.Time == drive.Time)
                    {
                        TempData["message"] = "You have already booked a test drive for this time.";
                        return RedirectToAction("BookTestDrive", "Home");
                    }
                    else if (item.Car_Chassis == drive.Car_Chassis)
                    {
                        TempData["message"] = "You have already booked a test drive for this car.";
                        return RedirectToAction("BookTestDrive", "Home");
                    }
                }
            }

            db.Test_Drive.Add(drive);
            db.SaveChanges();
            TempData["message"] = "Test drive has been booked successfully.";
            return RedirectToAction("BookTestDrive", "Home");
        }


        [HttpPost]
        public ActionResult Contact(FormCollection form)
        {
            string name = form["inputFirstName"];
            string surname = form["inputLastName"];
            string email = form["inputEmail"];
            string phoneNumber = form["inputPhone"];
            string address = form["inputAddress"];
            string address2 = form["inputAddress2"];
            string city = form["inputCity"];
            string dealer = form["inputDealer"];
            string inquiry = form["floatingTextarea2"];

            string emailBody = $"Name: {name} {surname}\nEmail: {email}\nPhone: {phoneNumber}\nAddress: {address}, {address2}, {city}\nDealer: {dealer}\nInquiry: {inquiry}";

            // Create MailMessage object
            MailMessage mail = new MailMessage();

            mail.From = new MailAddress(email);
            var adminEmails = (from user in db.Users
                               where user.Admin
                               select user)
                               .ToList();
            foreach (var aEmail in adminEmails)
            {
                mail.To.Add(aEmail.Email);
            }
            mail.Subject = "New Contact Form Submission";
            mail.Body = emailBody;


            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.UseDefaultCredentials = true;
            smtp.EnableSsl = true;

            smtp.Send(mail);
            mail.Dispose();
            smtp.Dispose();
            ViewBag.Message = "Thank you for contacting us!";
            return View();
        }
    }
}