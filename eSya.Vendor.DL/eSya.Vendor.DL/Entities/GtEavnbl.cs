﻿using System;
using System.Collections.Generic;

namespace eSya.Vendor.DL.Entities
{
    public partial class GtEavnbl
    {
        public int VendorId { get; set; }
        public int BusinessKey { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; } = null!;
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; } = null!;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedTerminal { get; set; }

        public virtual GtEcbsln BusinessKeyNavigation { get; set; } = null!;
        public virtual GtEavncd Vendor { get; set; } = null!;
    }
}
