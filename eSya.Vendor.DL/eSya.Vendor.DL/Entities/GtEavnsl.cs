using System;
using System.Collections.Generic;

namespace eSya.Vendor.DL.Entities
{
    public partial class GtEavnsl
    {
        public GtEavnsl()
        {
            GtEavnsds = new HashSet<GtEavnsd>();
        }

        public int VendorId { get; set; }
        public int VendorLocationId { get; set; }
        public string VendorLocation { get; set; } = null!;
        public bool IsLocationDefault { get; set; }
        public string VendorAddress { get; set; } = null!;
        public int StateCode { get; set; }
        public int Isdcode { get; set; }
        public string MobileNumber { get; set; } = null!;
        public string WhatsappNumber { get; set; } = null!;
        public string ContactPerson { get; set; } = null!;
        public string EMailId { get; set; } = null!;
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; } = null!;
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; } = null!;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedTerminal { get; set; }

        public virtual GtEavncd Vendor { get; set; } = null!;
        public virtual ICollection<GtEavnsd> GtEavnsds { get; set; }
    }
}
