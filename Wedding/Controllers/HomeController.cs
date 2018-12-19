using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using Wedding.Models;
using System.Configuration;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;


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


            Session["Customer"] = onlineBooking.MrName;
            
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
            
            return RedirectToAction("OnlineBookingconfirmation");
        }

        [HttpPost]
        public ActionResult ContactUs(ContactUs contactus)
        {
            var fromAddress = new MailAddress("bala1991.phoenix@gmail.com", "ContactUS_Messege" + DateTime.Now);
            var toAddress = new MailAddress("abirami.arangements@gmail.com", "Abirami_Arrangements");
            const string fromPassword = "X3haquc6";
            const string subject = "ContactUS_Messege:";

            string body = (" Name: " + contactus.name + "\n" +
            " Location:" + contactus.loc + "\n" +
            " Contact :" + contactus.contact + "\n" +
            " Subject: " + contactus.subject + "\n" +
            " Message: " + contactus.message + "\n");          

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
            return RedirectToAction("OnlineBookingconfirmation");
        }

        public ActionResult OnlineBookingconfirmation()
        {
            return View();
        }

        public ActionResult Payment()
        {
            string customer = "Newcustomer"+DateTime.Now;
            if ((string)Session["Customer"] != null)
            {
                customer = Convert.ToString(Session["Customer"]);
            }
            

            FormtoRequest model = new FormtoRequest();
            model.appId = ConfigurationManager.AppSettings["AppId"];
            model.orderId = DateTime.Now.Ticks.ToString();
            model.orderNote = "Abirami Arrangements payment";
            model.orderCurrency = "INR";
            model.customerName = customer;
            model.customerEmail = "abirami.arrangements@gmail.com";
            model.customerPhone = "9585831457";
            model.orderAmount = "2000";
            model.notifyUrl = "http://www.thirukadaiyurweddings.com/Home/Index/";
            model.returnUrl = "http://www.thirukadaiyurweddings.com/Home/OnlineBookingconfirmation/";

            string secretKey = ConfigurationManager.AppSettings["SecretKey"];
            string signatureData = "";
            PropertyInfo[] keys = model.GetType().GetProperties();
            keys = keys.OrderBy(key => key.Name).ToArray();

            foreach (PropertyInfo key in keys)
            {
                signatureData += key.Name + key.GetValue(model);
            }
            var hmacsha256 = new HMACSHA256(StringEncode(secretKey));
            byte[] gensignature = hmacsha256.ComputeHash(StringEncode(signatureData));
            string signature = Convert.ToBase64String(gensignature);
            ViewData["signature"] = signature;
            string mode = ConfigurationManager.AppSettings["Mode"];
            if (mode == "PROD")
            {
                ViewData["url"] = "https://www.cashfree.com/checkout/post/submit";
            }
            else
            {
                ViewData["url"] = "https://test.cashfree.com/billpay/checkout/post/submit";
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult HandleRequest(FormtoRequest model)
        {
            string secretKey = ConfigurationManager.AppSettings["SecretKey"];
            string signatureData = "";
            PropertyInfo[] keys = model.GetType().GetProperties();
            keys = keys.OrderBy(key => key.Name).ToArray();

            foreach (PropertyInfo key in keys)
            {
                signatureData += key.Name + key.GetValue(model);
            }
            var hmacsha256 = new HMACSHA256(StringEncode(secretKey));
            byte[] gensignature = hmacsha256.ComputeHash(StringEncode(signatureData));
            string signature = Convert.ToBase64String(gensignature);
            ViewData["signature"] = signature;

            string mode = ConfigurationManager.AppSettings["Mode"];

            if (mode == "PROD")
            {
                ViewData["url"] = "https://www.cashfree.com/checkout/post/submit";
            }
            else
            {
                ViewData["url"] = "https://test.cashfree.com/billpay/checkout/post/submit";
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult HandleResponse(FormCollection form)
        {
            string secretKey = "ENTER_YOUR_SECRET_KEY";
            string orderId = Request.Form["orderId"];
            string orderAmount = Request.Form["orderAmount"];
            string referenceId = Request.Form["referenceId"];
            string txStatus = Request.Form["txStatus"];
            string paymentMode = Request.Form["paymentMode"];
            string txMsg = Request.Form["txMsg"];
            string txTime = Request.Form["txTime"];
            string signature = Request.Form["signature"];

            string signatureData = orderId + orderAmount + referenceId + txStatus + paymentMode + txMsg + txTime;

            var hmacsha256 = new HMACSHA256(StringEncode(secretKey));
            byte[] gensignature = hmacsha256.ComputeHash(StringEncode(signatureData));
            string computedsignature = Convert.ToBase64String(gensignature);
            if (signature == computedsignature)
            {
                ViewData["panel"] = "panel panel-success";
                ViewData["heading"] = "Signature Verification Successful";

            }
            else
            {
                ViewData["panel"] = "panel panel-danger";
                ViewData["heading"] = "Signature Verification Failed";

            }
            ViewData["orderId"] = orderId;
            ViewData["orderAmount"] = orderAmount;
            ViewData["referenceId"] = referenceId;
            ViewData["txStatus"] = txStatus;
            ViewData["txMsg"] = txMsg;
            ViewData["txTime"] = txTime;
            ViewData["paymentMode"] = paymentMode;
            return View();
        }

        private static byte[] StringEncode(string text)
        {
            var encoding = new UTF8Encoding();
            return encoding.GetBytes(text);
        }

        //public ActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}


        [HttpPost]
        public ActionResult Enquiry(Enquiry enq)
        {
            TwilioClient.Init(
                Environment.GetEnvironmentVariable("ACdfdb075353f1efca1f6e2fb7da8d49ab"), Environment.GetEnvironmentVariable("1a7a04c388cbc1ef65741f3d77161594")
                );

            MessageResource.Create(
                to: new PhoneNumber("9585831457"),
               //from: new PhoneNumber("+17196248486"),
                  from: new PhoneNumber("+15005550006"),
             
                body: "test" 
                );

            return RedirectToAction("OnlineBookingconfirmation");
        }

    }
}