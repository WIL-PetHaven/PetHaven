using System;
using System.Web;
using PetHaven.DAL;
using System.Linq;
using System.Collections.Generic;

namespace PetHaven.Models
{
    public class Booking
    {
        private string BookingID { get; set; }
        private string AnimalName { get; set; }
        private string AnimalDescription { get; set; }

        private const string BookingSessionKey = "BookingID";
        private StoreContext db = new StoreContext();

        private string GetBookingID()
        {
            if (HttpContext.Current.Session[BookingSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name))
                {
                    HttpContext.Current.Session[BookingSessionKey] =
                 HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    Guid tempBookingID = Guid.NewGuid();
                    HttpContext.Current.Session[BookingSessionKey] = tempBookingID.ToString();
                }
            }
            return HttpContext.Current.Session[BookingSessionKey].ToString();
        }

        public static Booking GetBooking()
        {
            Booking booking = new Booking();
            booking.BookingID = booking.GetBookingID();
            return booking;
        }

        public void AddToBooking(int animalID, int quantity)
        {
            //var bookingLine = db.BookingLines.FirstOrDefault(b => b.BookingID == BookingID && b.AnimalID
            // == animalID);

            var bookingLine = db.BookingLines.FirstOrDefault(b => b.BookingID == BookingID && b.AnimalID
             == animalID && b.AnimalName == AnimalName && b.AnimalDescrtiption == AnimalDescription);

            if (bookingLine == null)
            {
                bookingLine = new BookingLine
                {
                    AnimalID = animalID,
                    BookingID = BookingID,
                    Quantity = quantity,
                    AnimalName = AnimalName,
                    AnimalDescrtiption = AnimalDescription,
                    DateCreated = DateTime.Now
                };
                db.BookingLines.Add(bookingLine);
            }
            else
            {
                bookingLine.Quantity += quantity;
            }
            db.SaveChanges();
        }

        public void RemoveLine(int animalID)
        {
            var bookingLine = db.BookingLines.FirstOrDefault(b => b.BookingID == BookingID && b.AnimalID
             == animalID);
            if (bookingLine != null)
            {
                db.BookingLines.Remove(bookingLine);
            }
            db.SaveChanges();
        }

        public void UpdateBooking(List<BookingLine> lines)
        {
            foreach (var line in lines)
            {
                var bookingLine = db.BookingLines.FirstOrDefault(b => b.BookingID == BookingID &&
                 b.AnimalID == line.AnimalID);
                if (bookingLine != null)
                {
                    if (line.Quantity == 0)
                    {
                        RemoveLine(line.AnimalID);
                    }
                    else
                    {
                        bookingLine.Quantity = line.Quantity;
                    }
                }
            }
            db.SaveChanges();
        }

        public void EmptyBooking()
        {
            var bookingLines = db.BookingLines.Where(b => b.BookingID == BookingID);
            foreach (var bookingLine in bookingLines)
            {
                db.BookingLines.Remove(bookingLine);
            }
            db.SaveChanges();
        }

        public List<BookingLine> GetBookingLines()
        {
            return db.BookingLines.Where(b => b.BookingID == BookingID).ToList();
        }

        public int GetNumberOfItems()
        {
            int numberOfItems = 0;
            if (GetBookingLines().Count > 0)
            {
                numberOfItems = db.BookingLines.Where(b => b.BookingID == BookingID).Sum(b => b.Quantity);
            }

            return numberOfItems;
        }

        public void MigrateBooking(string userName)
        {
            //find the current booking and store it in memory using ToList()
            var booking = db.BookingLines.Where(b => b.BookingID == BookingID).ToList();

            //find if the user already has a booking or not and store it in memory using ToList()
            var usersBooking = db.BookingLines.Where(b => b.BookingID == userName).ToList();

            //if the user has a booking then add the current items to it
            if (usersBooking != null)
            {
                //set the bookingID to the username
                string prevID = BookingID;
                BookingID = userName;
                //add the lines in anonymous booking to the user's booking
                foreach (var line in booking)
                {
                    AddToBooking(line.AnimalID, line.Quantity);
                }
                //delete the lines in the anonymous booking from the database
                BookingID = prevID;
                EmptyBooking();
            }
            else
            {
                //if the user does not have a booking then just migrate this one
                foreach (var bookingLine in booking)
                {
                    bookingLine.BookingID = userName;
                }

                db.SaveChanges();
            }
            HttpContext.Current.Session[BookingSessionKey] = userName;
        }

        //public string CreateBookedLines(int bookedID)
        //{
        //    string bookedTotal = "";

        //    var bookingLines = GetBookingLines();

        //    foreach (var item in bookingLines)
        //    {
        //        BookedLine bookedLine = new BookedLine
        //        {
        //            Animal = item.Animal,
        //            AnimalID = item.AnimalID,
        //            AnimalName = item.Animal.Name,
        //            Quantity = item.Quantity,

        //            BookedID = bookedID
        //        };

        //        db.BookedLines.Add(bookedLine);
        //    }

        //    db.SaveChanges();
        //    EmptyBooking();
        //    return bookedTotal;
        //}
    }
}