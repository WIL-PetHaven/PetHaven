using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using System.Web.Mvc;
using PetHaven.Models;

namespace PetHaven.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Donations(string id)
        {
            ViewBag.Message = "Your application description page. You entered the ID " + id;

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        public ActionResult Email()
        {
            Execute();

            return View($"Email sent");
        }

        // Method used to post the Contact page
        [HttpPost]
        public async Task Email(ContactForm model)
        {
            // If the model state is valid (all fields have passed validation), then send a message to admin@testsetup.net
            if (ModelState.IsValid)
            {
                // Gets a new GUID for the contact form
                guid = Guid.NewGuid();
                // Sends the email with all required information
                await _emailSender.SendEmailAsync("admin@testsetup.net", "Reference Number: "
                                                                         + guid, "<h2>Email: " + model.Email + "</h2>"
                                                                                 + "<br>" + "<h2>Message</h2>" +
                                                                                 "<p>" + model.Message.Replace("\n", "<br>") + "</p>");
                await _emailSender.SendEmailAsync(model.Email, "Reference Number: "
                                                                         + guid, "<h2>Email: " + model.Email + "</h2>"
                                                                                 + "<br>" + "<h2>Message</h2>" +
                                                                                 "<p>" + model.Message.Replace("\n", "<br>") + "</p>");
                // Return confirmation page
                return RedirectToAction("MessageSubmitted");
            }
            // Returns the contact page
            return About();
        }

        static void Execute()
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("test@example.com", "Example User");
            var subject = "Sending with SendGrid is Fun";
            var to = new EmailAddress("test@example.com", "Example User");
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = client.SendEmailAsync(msg).Result;
        }
    }
}