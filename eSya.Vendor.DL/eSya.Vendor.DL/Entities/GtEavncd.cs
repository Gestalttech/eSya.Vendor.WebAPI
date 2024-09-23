using System;
using System.Collections.Generic;

namespace eSya.Vendor.DL.Entities
{
    public partial class GtEavncd
    {
        public GtEavncd()
        {
            GtEavnbds = new HashSet<GtEavnbd>();
            GtEavnbls = new HashSet<GtEavnbl>();
            GtEavnsgs = new HashSet<GtEavnsg>();
            GtEavnsls = new HashSet<GtEavnsl>();
        }

        public int VendorId { get; set; }
        public string VendorName { get; set; } = null!;
        public int VendorClass { get; set; }
        public string CreditType { get; set; } = null!;
        public decimal CreditPeriod { get; set; }
        public int PreferredPaymentMode { get; set; }
        public bool ApprovalStatus { get; set; }
        public bool IsBlackListed { get; set; }
        public int ReasonForBlacklist { get; set; }
        public int SupplierScore { get; set; }
        public string? RejectionReason { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; } = null!;
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; } = null!;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedTerminal { get; set; }

        public virtual ICollection<GtEavnbd> GtEavnbds { get; set; }
        public virtual ICollection<GtEavnbl> GtEavnbls { get; set; }
        public virtual ICollection<GtEavnsg> GtEavnsgs { get; set; }
        public virtual ICollection<GtEavnsl> GtEavnsls { get; set; }
    }
}
