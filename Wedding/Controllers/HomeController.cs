using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Wedding.Models;
using Outlook = Microsoft.Office.Interop.Outlook;


namespace Wedding.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult OnlineBooking()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult OnlineBooking(OnlineBooking onlineBooking)
        {

            var fromAddress = new MailAddress("bala1991.phoenix@gmail.com", "New_Booking" + DateTime.Now);
            var toAddress = new MailAddress("abirami.arangements@gmail.com", "Abirami_Arrangements");
            const string fromPassword = "X3haquc6";
            const string subject = "New_Booking:";

            string body = (" MrName:" + onlineBooking.MrName + "\n" +
            " MrsName:" + onlineBooking.MrsName + "\n" +
            " MrRasiandNakshatra:" + onlineBooking.MrRasiandNakshatra + "\n" +
            " MrsRasiandNakshatra :" + onlineBooking.MrsRasiandNakshatra + "\n" +
            " MobileNumber: " + onlineBooking.MobileNumber + "\n" +
            " AlterneteMobile: " + onlineBooking.AlterneteMobile + "\n" +
            " FunctionDate :" + onlineBooking.FunctionDate + "\n" +
            " Location :" + onlineBooking.Location + "\n" +
            " TypeofAbishegam : " + onlineBooking.TypeofAbishegam + "\n" +
            " AbishegamDays :" + onlineBooking.AbishegamDays + "\n" +
            " Bhramin : " + onlineBooking.Bhramin + "\n" +
            " Food :" + onlineBooking.Food + "\n" +
            " Hotel :" + onlineBooking.Hotel + "\n" +
            " Photography :" + onlineBooking.Photography + "\n" +
            " VideoCoverage: " + onlineBooking.VideoCoverage);

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
               // smtp.Send(message);
            }
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        public ActionResult OnlineBookingconfirmation()
        {
            return View();
        }

    }
}