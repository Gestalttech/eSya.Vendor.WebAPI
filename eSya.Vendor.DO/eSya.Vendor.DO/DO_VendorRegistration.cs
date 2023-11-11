using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.Vendor.DO
{
    public class DO_VendorRegistration
    {
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
        public bool ActiveStatus { get; set; }
       // public int VendorCode { get; set; }
        public string FormID { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
        public List<DO_eSyaParameter> l_FormParameter { get; set; }
    }

    public class DO_VendorLocation
    {
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
        public string FormID { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
    }

    public class DO_BusinessKey
    {
        public string BusinessLocation { get; set; }
        public int BusinessKey { get; set; }
    }

    public class DO_VendorBusinessLink
    {
        public int VendorId { get; set; }
        public int BusinessKey { get; set; }
        public bool ActiveStatus { get; set; }
        public List<int> Businesslink { get; set; }
        public string FormID { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
    }

    public class DO_VendorStatutoryDetails
    {
        public int VendorId { get; set; }
        public int VendorLocationId { get; set; }
        public int StatutoryCode { get; set; }
        public string StatutoryDescription { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormID { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
    }

    public class DO_VendorBankdetails
    {
        public int VendorId { get; set; }
        public string BenificiaryBankAccountNo { get; set; }
        public string BenificiaryName { get; set; }
        public string BenificiaryBankName { get; set; }
        public string BankIfsccode { get; set; }
        public string BankSwiftcode { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormID { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
    }
    public class DO_eSyaParameter
    {
        public int ParameterID { get; set; }
        public bool ParmAction { get; set; }
        public decimal ParmValue { get; set; }
        public decimal ParmPerct { get; set; }
        public string? ParmDesc { get; set; }
        public bool ActiveStatus { get; set; }
        public int VendorId { get; set; }
    }

    public class DO_VendorSupplyGroup
    {
        public int VendorId { get; set; }
        public string FormID { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
        public List<DO_eSyaParameter> l_SupplyGroupParam { get; set; }

    }
}
