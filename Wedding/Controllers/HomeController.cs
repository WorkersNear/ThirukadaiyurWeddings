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
            //SendEmailtoContacts();
            var fromAddress = new MailAddress("bala1991.phoenix@gmail.com", "New_Booking" + DateTime.Now);
            var toAddress = new MailAddress("abirami.arangements@gmail.com", "Abirami_Arrangements");
            const string fromPassword = "X3haquc6";
            const string subject = "New_Booking:";
            // const string body = "test";
            string body = "test";

            string data = (" MrName:" + onlineBooking.MrName +
            " MrsName:" + onlineBooking.MrsName +
            " MrRasiandNakshatra:" + onlineBooking.MrRasiandNakshatra +
            " MrsRasiandNakshatra :" + onlineBooking.MrsRasiandNakshatra +
            " MobileNumber: " + onlineBooking.MobileNumber +
            " AlterneteMobile: " + onlineBooking.AlterneteMobile +
            " FunctionDate :" + onlineBooking.FunctionDate +
            " Location :" + onlineBooking.Location +
            " TypeofAbishegam : " + onlineBooking.TypeofAbishegam +
            " AbishegamDays :" + onlineBooking.AbishegamDays +
            " Bhramin : " + onlineBooking.Bhramin +
            " Food :" + onlineBooking.Food +
            " Hotel :" + onlineBooking.Hotel +
            " Photography :" + onlineBooking.Photography +
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
                smtp.Send(message);
            }
            return View();
        }

        public ActionResult OnlineBookingconfirmation()
        {
            return View();
        }
        private void SendEmailtoContacts()
        {
            string subjectEmail = "Meeting has been rescheduled.";
            string bodyEmail = "Meeting is one hour later.";
            Outlook.Application app = new Outlook.Application();
            Outlook.MAPIFolder sentContacts = (Outlook.MAPIFolder)
                app.ActiveExplorer().Session.GetDefaultFolder
                (Outlook.OlDefaultFolders.olFolderContacts);
            foreach (Outlook.ContactItem contact in sentContacts.Items)
            {
                if (contact.Email1Address.Contains("abirami.arangements@gmail.com"))
                {
                    this.CreateEmailItem(subjectEmail, contact
                        .Email1Address, bodyEmail);
                }
            }
        }

        private void CreateEmailItem(string subjectEmail,
               string toEmail, string bodyEmail)
        {            

            Outlook.Application app = new Outlook.Application();
            Outlook.MailItem eMail = app.CreateItem(Outlook.OlItemType.olMailItem);
            eMail.Subject = subjectEmail;
            eMail.To = toEmail;
            eMail.Body = bodyEmail;
            eMail.Importance = Outlook.OlImportance.olImportanceLow;
            ((Outlook._MailItem)eMail).Send();
        }



    }
}