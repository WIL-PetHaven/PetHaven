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
        private const string Value = "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";

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AboutUs()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        // Method used to post the Contact page
        [HttpPost]
        public async Task<ActionResult> Contact(ContactForm model)
        {
            // If the model state is valid (all fields have passed validation), then send a message
            if (ModelState.IsValid)
            {
                await Execute(model);

                // Return confirmation page
                return View();
            }

            return View();
        } 

        static async Task Execute(ContactForm model)
        {

            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("ngo.pethaven@gmail.com", "PetHaven");

            var subject1 = "Message Recieved";
            var to1 = new EmailAddress(model.Email, model.Name);
            var plainTextContent1 = model.Message;
            var htmlContent1 = "<!DOCTYPE html> <html lang=\"en\"><head><meta charset=\"UTF-8\"> <meta name=\"viewport\" contentwidth=\"device-width\", initial-scale=\"1.0\", maximum-scale=\"1.0\", user-scalable=\"no\", height=\"device-height\"/> <meta http-equiv=\"X-UA-Compatible\" content=\"ie-edge\"/> </head> <body style=\"width: 100%;margin: 0px;padding: 0px;font-family: Arial, Helvetica, sans-serif;margin-top: 5vh;\"> <div class=\"container\" style=\"max-width: 800px;margin:auto;box-shadow: 0 6px 20px rgba(0,0,0,.4);height: auto;background-color: grey;\"> <div class=\"headingRow\" style=\"background-color: #39393A;width: 100%;padding: 15px 0;color: #ffff;display: grid;grid-template-columns: repeat(3, 1fr);\"> <div class=\"company\" style=\"text-align: left;font-size: 14px;padding-left: 20px;\"> </div> <div class=\"emailType\" style=\"text-align: center;\"> <h1 style =\"font-size: 20px;margin: 0;padding: 0;\">" +
                "Message Recieved" + 
                "</h1> </div> </div> <div class=\"emailContent\" style=\"padding: 5px 20px;background-color: #39393a;text-align: center;\"> <div class=\"messageDetails\" style=\"background-color: #F6F9FC;padding: 20px 20px;margin: 20px 0;\"> <p class=\"emailContent_title\" style=\"font-size: 20px;margin-bottom: 20px;\"> <b>" +
                "Message Details" +
                "</b> </p> <p>" +
                model.Message +
                "</p> </div> <div class=\"customerInfo\" style=\"background-color: #F6F9FC;padding: 20px 20px;margin: 20px 0;\"> <p class=\"emailContent_title\" style=\"font-size: 20px;margin-bottom: 20px;\"> <b>" +
                "Contact Information" +
                "</b> </p> <div class=\"info-container\" style=\"width: 400px;margin: 0 auto;overflow: hidden; padding-bottom: 20px;\"> <div class=\"custRow\" style=\"margin-bottom: 10px;display: grid;grid-template-columns: .5fr 1fr;grid-column-gap: 20px;\"> <p class=\"cust-item\" style=\"text-align: left;padding: 5px 0;\">Full Name:</p> <p class=\"cust-descr\" style=\"background-color: #ffff;padding: 5px 30px;text-align: left;\">" + model.Name +
                "</p> </div> <div class=\"custRow\" style=\"margin-bottom: 10px;display: grid;grid-template-columns: .5fr 1fr;grid-column-gap: 20px;\"> <p class=\"cust-item\" style=\"text-align: left; padding: 5px 0;\">Email:</p> <p class=\"cust-descr\" style=\"background-color: #ffff;padding: 5px 30px;text-align: left;\">" + model.Email +
                "</p> </div> <div class=\"custRow\" style=\"margin-bottom: 10px;display: grid;grid-template-columns: .5fr 1fr;grid-column-gap: 20px;\"> <p class=\"cust-item\" style=\"text-align: left;padding: 5px 0;\">Contact No:</p> <p class=\"cust-descr\" style=\"background-color: #ffff;padding: 5px 30px;text-align: left;\">" + model.Phone +
                "</p> </div> </div> </div> </div> </div> </body> </html>";
            var msg1 = MailHelper.CreateSingleEmail(from, to1, subject1, plainTextContent1, htmlContent1);
            var response1 = await client.SendEmailAsync(msg1);

            var subject2 = "Contact Form Message";
            var to2 = new EmailAddress("ngo.pethaven@gmail.com", "PetHaven");
            var plainTextContent2 = model.Message;
            var htmlContent2 = "<!DOCTYPE html> <html lang=\"en\"><head><meta charset=\"UTF-8\"> <meta name=\"viewport\" contentwidth=\"device-width\", initial-scale=\"1.0\", maximum-scale=\"1.0\", user-scalable=\"no\", height=\"device-height\"/> <meta http-equiv=\"X-UA-Compatible\" content=\"ie-edge\"/> </head> <body style=\"width: 100%;margin: 0px;padding: 0px;font-family: Arial, Helvetica, sans-serif;margin-top: 5vh;\"> <div class=\"container\" style=\"max-width: 800px;margin:auto;box-shadow: 0 6px 20px rgba(0,0,0,.4);height: auto;background-color: grey;\"> <div class=\"headingRow\" style=\"background-color: #39393A;width: 100%;padding: 15px 0;color: #ffff;display: grid;grid-template-columns: repeat(3, 1fr);\"> <div class=\"company\" style=\"text-align: left;font-size: 14px;padding-left: 20px;\"> </div> <div class=\"emailType\" style=\"text-align: center;\"> <h1 style =\"font-size: 20px;margin: 0;padding: 0;\">" +
                "Contact Form Message" +
                "</h1> </div> </div> <div class=\"emailContent\" style=\"padding: 5px 20px;background-color: #39393a;text-align: center;\"> <div class=\"messageDetails\" style=\"background-color: #F6F9FC;padding: 20px 20px;margin: 20px 0;\"> <p class=\"emailContent_title\" style=\"font-size: 20px;margin-bottom: 20px;\"> <b>" +
                "Message Details" +
                "</b> </p> <p>" +
                model.Message +
                "</p> </div> <div class=\"customerInfo\" style=\"background-color: #F6F9FC;padding: 20px 20px;margin: 20px 0;\"> <p class=\"emailContent_title\" style=\"font-size: 20px;margin-bottom: 20px;\"> <b>" +
                "Contact Information" +
                "</b> </p> <div class=\"info-container\" style=\"width: 400px;margin: 0 auto;overflow: hidden; padding-bottom: 20px;\"> <div class=\"custRow\" style=\"margin-bottom: 10px;display: grid;grid-template-columns: .5fr 1fr;grid-column-gap: 20px;\"> <p class=\"cust-item\" style=\"text-align: left;padding: 5px 0;\">Full Name:</p> <p class=\"cust-descr\" style=\"background-color: #ffff;padding: 5px 30px;text-align: left;\">" + model.Name +
                "</p> </div> <div class=\"custRow\" style=\"margin-bottom: 10px;display: grid;grid-template-columns: .5fr 1fr;grid-column-gap: 20px;\"> <p class=\"cust-item\" style=\"text-align: left; padding: 5px 0;\">Email:</p> <p class=\"cust-descr\" style=\"background-color: #ffff;padding: 5px 30px;text-align: left;\">" + model.Email +
                "</p> </div> <div class=\"custRow\" style=\"margin-bottom: 10px;display: grid;grid-template-columns: .5fr 1fr;grid-column-gap: 20px;\"> <p class=\"cust-item\" style=\"text-align: left;padding: 5px 0;\">Contact No:</p> <p class=\"cust-descr\" style=\"background-color: #ffff;padding: 5px 30px;text-align: left;\">" + model.Phone +
                "</p> </div> </div> </div> </div> </div> </body> </html>";
            var msg2 = MailHelper.CreateSingleEmail(from, to2, subject2, plainTextContent2, htmlContent2);
            var response2 = await client.SendEmailAsync(msg2);
        }
    }
}