using eSya.Vendor.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.Vendor.IF
{
    public interface IApproveRepository
    {
        Task<List<DO_VendorRegistration>> GetVendorsForApprovals(string approved);
        Task<DO_ReturnParameter> ApproveVendor(DO_VendorApproval obj);
        Task<DO_ReturnParameter> RejectVendor(DO_VendorApproval obj);
    }
}
