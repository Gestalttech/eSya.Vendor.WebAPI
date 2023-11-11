using System;
using System.Collections.Generic;

namespace eSya.Vendor.DL.Entities
{
    public partial class GtEavnpn
    {
        public int VendorCode { get; set; }
        public int ItemCode { get; set; }
        public string PartNo { get; set; } = null!;
        public string PartDesc { get; set; } = null!;
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; } = null!;
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; } = null!;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedTerminal { get; set; }
    }
}
