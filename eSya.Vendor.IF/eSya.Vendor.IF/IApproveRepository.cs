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
        Task<List<DO_VendorRegistration>> GetVendorsForApprovals();
        Task<DO_ReturnParameter> ApproveVendor(DO_VendorApproval obj);
    }
}
