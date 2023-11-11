using System;
using System.Collections.Generic;

namespace eSya.Vendor.DL.Entities
{
    public partial class GtEavnbd
    {
        public int VendorId { get; set; }
        public string BenificiaryBankAccountNo { get; set; } = null!;
        public string BenificiaryName { get; set; } = null!;
        public string BenificiaryBankName { get; set; } = null!;
        public string BankIfsccode { get; set; } = null!;
        public string BankSwiftcode { get; set; } = null!;
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; } = null!;
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; } = null!;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedTerminal { get; set; }

        public virtual GtEavncd Vendor { get; set; } = null!;
    }
}
