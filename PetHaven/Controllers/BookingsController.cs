using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using PetHaven.DAL;
using PetHaven.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace PetHaven.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private StoreContext db = new StoreContext();

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ??
             HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Bookings/Review
        public async Task<ActionResult> Review()
        {
            Booking booking = Booking.GetBooking();
            Bookings bookings = new Bookings();

            bookings.UserID = User.Identity.Name;
            ApplicationUser user = await UserManager.FindByNameAsync(bookings.UserID);
            bookings.DeliveryName = user.FirstName + " " + user.LastName;
            bookings.BookingsLines = new List<BookingsLine>();
            foreach (var bookingLine in booking.GetBookingLines())
            {
                BookingsLine line = new BookingsLine
                {
                    Animal = bookingLine.Animal,
                    AnimalID = bookingLine.AnimalID,
                    AnimalName = bookingLine.Animal.Name,
                    Quantity = bookingLine.Quantity,

                };
                bookings.BookingsLines.Add(line);
            }

            return View(bookings);
        }

        // GET: Bookings
        public ActionResult Index(string bookingsSearch, string startDate, string endDate, string bookingsSortOrder)
        {
            var bookings = db.Bookings.OrderBy(o => o.DateCreated).Include(o => o.BookingsLines);

            if (!User.IsInRole("Admin"))
            {
                bookings = bookings.Where(o => o.UserID == User.Identity.Name);
            }            
            if (!String.IsNullOrEmpty(bookingsSearch))
            {
                bookings = bookings.Where(o => o.BookingsID.ToString().Equals(bookingsSearch) ||
                 o.UserID.Contains(bookingsSearch) ||
                 o.DeliveryName.Contains(bookingsSearch) ||
                 o.AnimalName.Contains(bookingsSearch) ||
                 o.BookingsLines.Any(ol => ol.AnimalName.Contains(bookingsSearch)));
            }

            DateTime parsedStartDate;
            if (DateTime.TryParse(startDate, out parsedStartDate))
            {
                bookings = bookings.Where(o => o.DateCreated >= parsedStartDate);
            }

            DateTime parsedEndDate;
            if (DateTime.TryParse(endDate, out parsedEndDate))
            {
                bookings = bookings.Where(o => o.DateCreated <= parsedEndDate);
            }

            ViewBag.DateSort = String.IsNullOrEmpty(bookingsSortOrder) ? "date" : "";
            ViewBag.UserSort = bookingsSortOrder == "user" ? "user_desc" : "user";
            ViewBag.CurrentBookingsSearch = bookingsSortOrder;
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;

            switch (bookingsSortOrder)
            {
                case "user":
                    bookings = bookings.OrderBy(o => o.UserID);
                    break;
                case "user_desc":
                    bookings = bookings.OrderByDescending(o => o.UserID);
                    break;
                case "date":
                    bookings = bookings.OrderBy(o => o.DateCreated);
                    break;
                default:
                    bookings = bookings.OrderByDescending(o => o.DateCreated);
                    break;
            }

            return View(bookings);

        }

        // GET: Bookings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bookings bookings = db.Bookings.Include(o => o.BookingsLines).Where(o => o.BookingsID == id).SingleOrDefault();

            if (bookings == null)
            {
                return HttpNotFound();
            }

            if (bookings.UserID == User.Identity.Name || User.IsInRole("Admin"))
            {
                return View(bookings);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

        }

        // POST: Bookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "UserID, DeliveryName, AnimalName")] Bookings bookings, DateTime dateofbooking, BookingsLine bookingsLines)
        {
            BookingsLine bookingsLine = new BookingsLine();
            if (ModelState.IsValid)
            {
                bookings.DateCreated = DateTime.Now;
                bookings.DateOfBooking = dateofbooking;
                db.Bookings.Add(bookings);
                db.SaveChanges();

                //add the orderlines to the database after creating the order
                Booking booking = Booking.GetBooking();

                bookings.AnimalName = booking.CreateBookingsLines(bookings.BookingsID);
                
                db.SaveChanges();

                await Execute(bookings, bookingsLine);

                return RedirectToAction("Details", new { id = bookings.BookingsID });
            }
            return RedirectToAction("Review");
        }

        // GET: Bookings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bookings bookings = db.Bookings.Find(id);
            if (bookings == null)
            {
                return HttpNotFound();
            }
            return View(bookings);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookingsID,UserID,DeliveryName,DeliveryAddress,AnimalName,DateCreated")] Bookings bookings)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bookings).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bookings);
        }

        // GET: Bookings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bookings bookings = db.Bookings.Find(id);
            if (bookings == null)
            {
                return HttpNotFound();
            }
            return View(bookings);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bookings bookings = db.Bookings.Find(id);
            db.Bookings.Remove(bookings);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        static async Task Execute(Bookings model, BookingsLine bookingsLine)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("ngo.pethaven@gmail.com", "PetHaven");

            var subject1 = "Booking Confirmation";
            var to1 = new EmailAddress(model.UserID, model.UserID);
            var plainTextContent1 = model.UserID;
            var htmlContent1 = "<!DOCTYPE html> <html lang=\"en\"><head><meta charset=\"UTF-8\"> <meta name=\"viewport\" contentwidth=\"device-width\", initial-scale=\"1.0\", maximum-scale=\"1.0\", user-scalable=\"no\", height=\"device-height\"/> <meta http-equiv=\"X-UA-Compatible\" content=\"ie-edge\"/> </head> <body style=\"width: 100%;margin: 0px;padding: 0px;font-family: Arial, Helvetica, sans-serif;margin-top: 5vh;\"> <div class=\"container\" style=\"max-width: 800px;margin:auto;box-shadow: 0 6px 20px rgba(0,0,0,.4);height: auto;background-color: grey;\"> <div class=\"headingRow\" style=\"background-color: #39393A;width: 100%;padding: 15px 0;color: #ffff;display: grid;grid-template-columns: repeat(3, 1fr);\"> <div class=\"company\" style=\"text-align: left;font-size: 14px;padding-left: 20px;\"> </div> <div class=\"emailType\" style=\"text-align: center;\"> <h1 style =\"font-size: 20px;margin: 0;padding: 0;\">" +
                "Booking Confirmation" +
                "</h1> </div> </div> <div class=\"customerInfo\" style=\"background-color: #F6F9FC;padding: 20px 20px;margin: 20px 0;\"> <p class=\"emailContent_title\" style=\"font-size: 20px;margin-bottom: 20px;\"> <b>" +
                "Booking Information" +
                "</b> </p> <div class=\"info-container\" style=\"width: 400px;margin: 0 auto;overflow: hidden; padding-bottom: 20px;\"> <div class=\"custRow\" style=\"margin-bottom: 10px;display: grid;grid-template-columns: .5fr 1fr;grid-column-gap: 20px;\"> <p class=\"cust-item\" style=\"text-align: left;padding: 5px 0;\">Animal Name:</p> <p class=\"cust-descr\" style=\"background-color: #ffff;padding: 5px 30px;text-align: left;\">" + model.DeliveryName +
                "</p> </div> <div class=\"custRow\" style=\"margin-bottom: 10px;display: grid;grid-template-columns: .5fr 1fr;grid-column-gap: 20px;\"> <p class=\"cust-item\" style=\"text-align: left; padding: 5px 0;\">Animal Name:</p> <p class=\"cust-descr\" style=\"background-color: #ffff;padding: 5px 30px;text-align: left;\">" + bookingsLine.Animal.Name +
                "</p> </div> <div class=\"custRow\" style=\"margin-bottom: 10px;display: grid;grid-template-columns: .5fr 1fr;grid-column-gap: 20px;\"> <p class=\"cust-item\" style=\"text-align: left;padding: 5px 0;\">Date and TIme of Booking:</p> <p class=\"cust-descr\" style=\"background-color: #ffff;padding: 5px 30px;text-align: left;\">" + model.DateOfBooking +
                "</p> </div> </div> </div> </div> </div> </body> </html>";
            var msg1 = MailHelper.CreateSingleEmail(from, to1, subject1, plainTextContent1, htmlContent1);
            var response1 = await client.SendEmailAsync(msg1);

            var subject2 = "New Booking";
            var to2 = new EmailAddress("ngo.pethaven@gmail.com", "PetHaven");
            var plainTextContent2 = model.UserID;
            var htmlContent2 = "<!DOCTYPE html> <html lang=\"en\"><head><meta charset=\"UTF-8\"> <meta name=\"viewport\" contentwidth=\"device-width\", initial-scale=\"1.0\", maximum-scale=\"1.0\", user-scalable=\"no\", height=\"device-height\"/> <meta http-equiv=\"X-UA-Compatible\" content=\"ie-edge\"/> </head> <body style=\"width: 100%;margin: 0px;padding: 0px;font-family: Arial, Helvetica, sans-serif;margin-top: 5vh;\"> <div class=\"container\" style=\"max-width: 800px;margin:auto;box-shadow: 0 6px 20px rgba(0,0,0,.4);height: auto;background-color: grey;\"> <div class=\"headingRow\" style=\"background-color: #39393A;width: 100%;padding: 15px 0;color: #ffff;display: grid;grid-template-columns: repeat(3, 1fr);\"> <div class=\"company\" style=\"text-align: left;font-size: 14px;padding-left: 20px;\"> </div> <div class=\"emailType\" style=\"text-align: center;\"> <h1 style =\"font-size: 20px;margin: 0;padding: 0;\">" +
                "Booking Confirmation" +
                "</h1> </div> </div> <div class=\"customerInfo\" style=\"background-color: #F6F9FC;padding: 20px 20px;margin: 20px 0;\"> <p class=\"emailContent_title\" style=\"font-size: 20px;margin-bottom: 20px;\"> <b>" +
                "Booking Information" +
                "</b> </p> <div class=\"info-container\" style=\"width: 400px;margin: 0 auto;overflow: hidden; padding-bottom: 20px;\"> <div class=\"custRow\" style=\"margin-bottom: 10px;display: grid;grid-template-columns: .5fr 1fr;grid-column-gap: 20px;\"> <p class=\"cust-item\" style=\"text-align: left;padding: 5px 0;\">Animal Name:</p> <p class=\"cust-descr\" style=\"background-color: #ffff;padding: 5px 30px;text-align: left;\">" + model.DeliveryName +
                "</p> </div> <div class=\"custRow\" style=\"margin-bottom: 10px;display: grid;grid-template-columns: .5fr 1fr;grid-column-gap: 20px;\"> <p class=\"cust-item\" style=\"text-align: left; padding: 5px 0;\">Animal Name:</p> <p class=\"cust-descr\" style=\"background-color: #ffff;padding: 5px 30px;text-align: left;\">" + model.AnimalName +
                "</p> </div> <div class=\"custRow\" style=\"margin-bottom: 10px;display: grid;grid-template-columns: .5fr 1fr;grid-column-gap: 20px;\"> <p class=\"cust-item\" style=\"text-align: left;padding: 5px 0;\">Date of Booking:</p> <p class=\"cust-descr\" style=\"background-color: #ffff;padding: 5px 30px;text-align: left;\">" + model.DateOfBooking +
                "</p> </div> </div> </div> </div> </div> </body> </html>";
            var msg2 = MailHelper.CreateSingleEmail(from, to2, subject2, plainTextContent2, htmlContent2);
            var response2 = await client.SendEmailAsync(msg2);
        }
    }
}