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
            bookings.DeliveryAddress = user.Address;
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
                 o.DeliveryAddress.AddressLine1.Contains(bookingsSearch) ||
                 o.DeliveryAddress.AddressLine2.Contains(bookingsSearch) ||
                 o.DeliveryAddress.Town.Contains(bookingsSearch) ||
                 o.DeliveryAddress.County.Contains(bookingsSearch) ||
                 o.DeliveryAddress.Postcode.Contains(bookingsSearch) ||
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
        public ActionResult Create([Bind(Include = "UserID,DeliveryName,DeliveryAddress")] Bookings bookings)
        {
            if (ModelState.IsValid)
            {
                bookings.DateCreated = DateTime.Now;
                db.Bookings.Add(bookings);
                db.SaveChanges();

                //add the orderlines to the database after creating the order
                Booking booking = Booking.GetBooking();

                //bookings.TotalPrice = booking.CreateBookingsLines(bookings.BookingsID);
                
                db.SaveChanges();
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
    }
}
