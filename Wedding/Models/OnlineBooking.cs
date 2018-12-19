using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wedding.Models
{
    public class OnlineBooking
    {
        public string MrName { get; set; }
        public string MrsName { get; set; }
        public string MrRasiandNakshatra { get; set; }
        public string MrsRasiandNakshatra { get; set; }
        public string MobileNumber { get; set; }
        public string AlterneteMobile { get; set; }
        public string FunctionDate { get; set; }
        public string TypeofAbishegam { get; set; }
        public string Location { get; set; }
        public string AbishegamDays { get; set; }
        public string Food { get; set; }
        public string Hotel { get; set; }
        public string Photography { get; set; }
        public string VideoCoverage { get; set; }
        public string Bhramin { get; set; }

    }

    public class ContactUs
    {
        public string name { get; set; }
        public string loc { get; set; }
        public string contact { get; set; }
        public string subject { get; set; }
        public string message { get; set; }
    }

    public class Enquiry
    {
        public string Name { get; set; }
        public string MobileNumber { get; set; }

    }

}