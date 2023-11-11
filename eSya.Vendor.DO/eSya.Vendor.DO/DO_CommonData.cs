using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.Vendor.DO
{
    public class DO_ApplicationCodes
    {
        public int ApplicationCode { get; set; }
        public int CodeType { get; set; }
        public string CodeDesc { get; set; }
    }

    public class DO_BusinessLocation
    {
        public int BusinessKey { get; set; }
        public string LocationDescription { get; set; }
    }

    public class DO_CountryCodes
    {
        public int Isdcode { get; set; }
        public string CountryName { get; set; }
      
    }

    public class DO_States
    {
        public int StateCode { get; set; }
        public string StateDesc { get; set; }
    }
}
