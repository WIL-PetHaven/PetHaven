using System.Web.Mvc;
using PetHaven.Models;
using PetHaven.ViewModels;

namespace PetHaven.Controllers
{
    public class BookingController : Controller
    {
        // GET: Booking
        public ActionResult Index()
        {
            Booking booking = Booking.GetBooking();
            BookingViewModel viewModel = new BookingViewModel
            {
                BookingLines = booking.GetBookingLines(),
            };
            return View(viewModel);
        }

        //
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToBooking(int id)
        {
            Booking booking = Booking.GetBooking();
            booking.AddToBooking(id);
            return RedirectToAction("Index");
        }

        //
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateBooking(BookingViewModel viewModel)
        {
            Booking booking = Booking.GetBooking();
            booking.UpdateBooking(viewModel.BookingLines);
            return RedirectToAction("Index");
        }

        //
        [HttpGet]
        public ActionResult RemoveLine(int id)
        {
            Booking booking = Booking.GetBooking();
            booking.RemoveLine(id);
            return RedirectToAction("Index");
        }

        //
        public PartialViewResult Summary()
        {
            Booking booking = Booking.GetBooking();
            BookingSummaryViewModel viewModel = new BookingSummaryViewModel
            {
                NumberOfItems = booking.GetNumberOfItems(),
            };
            return PartialView(viewModel);
        }
    }
}