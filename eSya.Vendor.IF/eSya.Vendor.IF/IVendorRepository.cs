using eSya.Vendor.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.Vendor.IF
{
    public interface IVendorRepository
    {

        #region Vendor Registration

        List<DO_VendorRegistration> GetVendors(string Alphabet);

        Task<DO_ReturnParameter> InsertOrUpdateVendor(DO_VendorRegistration vendor);

        Task<List<DO_eSyaParameter>> GetVendorParameterList(int vendorID);

        Task<DO_ReturnParameter> ActiveOrDeActiveVendor(bool status, int vendorID);
        #endregion Vendor Registration

        #region Vendor Location

        Task<List<DO_VendorLocation>> GetVendorLocationsByVendorcode(int vendorID);

        Task<DO_ReturnParameter> InsertOrUpdateVendorLocation(DO_VendorLocation objloc);

        #endregion Vendor Location

        #region Vendor Business Link
        Task<List<DO_BusinessKey>> GetBusinessLocation();
        Task<List<DO_VendorBusinessLink>> GetBusinessKeysByVendorcode(int vendorID);

        Task<DO_ReturnParameter> InsertBusinesskeyforVendor(DO_VendorBusinessLink objkey);

        #endregion Vendor Business Link

        #region Vendor Statutory Details

        Task<List<DO_VendorStatutoryDetails>> GetStatutorydetailsbyVendorcodeAndLocationId(int vendorID, int locationId);

        Task<DO_ReturnParameter> InsertOrUpdateStatutorydetails(DO_VendorStatutoryDetails objsat);

        #endregion Vendor Statutory Details

        #region Vendor Bank Details

        Task<List<DO_VendorBankdetails>> GetVendorBankdetailsByVendorcode(int vendorID);

        Task<DO_ReturnParameter> InsertVendorBankdetails(DO_VendorBankdetails objbank);

        Task<DO_ReturnParameter> UpdateVendorBankdetails(DO_VendorBankdetails objbank);

        #endregion Vendor Bank Details

        #region Vendor Supply Group
        Task<List<DO_Parameters>> GetVendorSuuplyGroupSubledgerType(string subledgertype);
        Task<List<DO_eSyaParameter>> GetVendorSuuplyGroupParameterList(int vendorID);
        Task<DO_ReturnParameter> InsertSuuplyGroupforVendor(DO_VendorSupplyGroup objsupply);
        #endregion Vendor Supply Group
    }
}
