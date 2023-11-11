﻿using System;
using System.Collections.Generic;

namespace eSya.Vendor.DL.Entities
{
    public partial class GtEavnsd
    {
        public int VendorId { get; set; }
        public int VendorLocationId { get; set; }
        public int StatutoryCode { get; set; }
        public string StatutoryDescription { get; set; } = null!;
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; } = null!;
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; } = null!;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedTerminal { get; set; }

        public virtual GtEavnsl Vendor { get; set; } = null!;
    }
}
