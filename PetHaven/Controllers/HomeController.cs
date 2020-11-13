using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using System.Web.Mvc;
using PetHaven.Models;
using System.Diagnostics;

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

        // Method used to post the Contact page
        [HttpPost]
        public async Task<ActionResult> Contact(ContactForm model)
        {
            await Execute(model);

            // Return confirmation page
            return View();
        } 

        static async Task Execute(ContactForm model)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("ngo.pethaven@gmail.com", "PetHaven");

            var subject1 = "Message Recieved";
            var to1 = new EmailAddress(model.Email, model.Name);
            var plainTextContent1 = model.Message;
            var htmlContent1 = model.Message;
            var msg1 = MailHelper.CreateSingleEmail(from, to1, subject1, plainTextContent1, htmlContent1);
            var response1 = await client.SendEmailAsync(msg1);

            var subject2 = "Contact Form Message";
            var to2 = new EmailAddress("ngo.pethaven@gmail.com", "PetHaven");
            var plainTextContent2 = model.Message;
            var htmlContent2 = model.Message;
            var msg2 = MailHelper.CreateSingleEmail(from, to2, subject2, plainTextContent2, htmlContent2);
            var response2 = await client.SendEmailAsync(msg2);
        }
    }
}