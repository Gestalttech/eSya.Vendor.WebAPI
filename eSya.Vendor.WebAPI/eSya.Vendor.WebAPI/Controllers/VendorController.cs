using eSya.Vendor.DL.Repository;
using eSya.Vendor.DO;
using eSya.Vendor.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.Vendor.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VendorController : ControllerBase
    {
        private readonly IVendorRepository _VendorRepository;
        private readonly ICommonDataRepository _CommonDataRepository;
        public VendorController(IVendorRepository vendorRepository, ICommonDataRepository commonDataRepository)
        {
            _VendorRepository = vendorRepository;
            _CommonDataRepository = commonDataRepository;
        }

        #region Vendor Registration
        /// <summary>
        /// Getting  Vendors.
        /// UI Reffered - Vendor Grid
        /// </summary>
        [HttpGet]
        public IActionResult GetVendors(string Alphabet)
        {
            var vendors = _VendorRepository.GetVendors(Alphabet);
            return Ok(vendors);
        }

        /// <summary>
        /// Getting  Vendors SupplyGroup by Vendor Code.
        /// UI Reffered -Vendor Registration Grid
        /// params-Vendorcode
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetVendorParameterList(int vendorID)
        {
            var vendorparam = await _VendorRepository.GetVendorParameterList(vendorID);
            return Ok(vendorparam);
        }

        /// <summary>
        /// Insert Or Update Vendors.
        /// UI Reffered -Vendor
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateVendor(DO_VendorRegistration obj)
        {
            var msg = await _VendorRepository.InsertOrUpdateVendor(obj);
            return Ok(msg);

        }

        /// <summary>
        /// Active/De Active Vendors.
        /// UI Reffered -Vendor
        /// Paramsstatus-vendorcode
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ActiveOrDeActiveVendor(bool status, int vendorID)
        {
            var msg = await _VendorRepository.ActiveOrDeActiveVendor(status, vendorID);
            return Ok(msg);

        }
        #endregion Vendor Registration

        #region Vendor Location
        /// <summary>
        /// Getting  Vendors Location.
        /// UI Reffered -Vendor Location Grid and Vendor Statutory Details Grid
        /// params-Vendorcode
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetVendorLocationsByVendorcode(int vendorID)
        {
            var vlocations = await _VendorRepository.GetVendorLocationsByVendorcode(vendorID);
            return Ok(vlocations);
        }
        /// <summary>
        /// Insert Or Update Vendor Location.
        /// UI Reffered -Vendor
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateVendorLocation(DO_VendorLocation vendorloc)
        {
            var msg = await _VendorRepository.InsertOrUpdateVendorLocation(vendorloc);
            return Ok(msg);

        }
        #endregion Vendor Location

        #region Vendor Business Link

        /// <summary>
        /// Getting  Business Locations.
        /// UI Reffered -Vendor Business Link
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetBusinessLocation()
        {
            var blocations = await _VendorRepository.GetBusinessLocation();
            return Ok(blocations);
        }

        /// <summary>
        /// Getting  Business Keys by Vendor code.
        /// UI Reffered -Vendor Business Link
        /// UI-Params-vendorcode
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetBusinessKeysByVendorcode(int vendorID)
        {
            var bkeys = await _VendorRepository.GetBusinessKeysByVendorcode(vendorID);
            return Ok(bkeys);
        }

        /// <summary>
        /// Link Business Key for Vendor.
        /// UI Reffered -Vendor Business Link
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertBusinesskeyforVendor(DO_VendorBusinessLink objkey)
        {
            var msg = await _VendorRepository.InsertBusinesskeyforVendor(objkey);
            return Ok(msg);

        }
        #endregion Vendor Business Link

        #region Vendor Statutory Details need to remove
        /// <summary>
        /// Getting  Statutory details.
        /// UI Reffered -Vendor Statutory Details Grid
        /// params-Vendorcode and LocationId
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetStatutorydetailsbyVendorcodeAndLocationId(int vendorID, int locationId)
        {
            var statdetails = await _VendorRepository.GetStatutorydetailsbyVendorcodeAndLocationId(vendorID, locationId);
            return Ok(statdetails);
        }
        /// <summary>
        /// Insert Or Update Vendor Statutory Details.
        /// UI Reffered -Vendor Statutory Details
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateStatutorydetails(DO_VendorStatutoryDetails objsat)
        {
            var msg = await _VendorRepository.InsertOrUpdateStatutorydetails(objsat);
            return Ok(msg);

        }
        #endregion Vendor Statutory Details

        #region Vendor Statutory Details
        [HttpGet]
        public async Task<IActionResult> GetVendorAddressLocationsByVendorID(int vendorID)
        {
            var statdetails =await _VendorRepository.GetVendorAddressLocationsByVendorID(vendorID);
            return Ok(statdetails);
        }
        [HttpGet]
        public async Task<IActionResult> GetISDCodesbyVendorId(int vendorID)
        {
            var statdetails =await _VendorRepository.GetISDCodesbyVendorId(vendorID);
            return Ok(statdetails);
        }
        [HttpGet]
        public async Task<IActionResult> GetVendorStatutoryDetails(int vendorID, int isdCode, int locationId)
        {
            var statdetails = await _VendorRepository.GetVendorStatutoryDetails(vendorID, isdCode, locationId);
            return Ok(statdetails);
        }

        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateVendorStatutoryDetails(List<DO_VendorStatutoryDetails> obj)
        {
            var msg = await _VendorRepository.InsertOrUpdateVendorStatutoryDetails(obj);
            return Ok(msg);

        }
        //[HttpGet]
        //public async Task<IActionResult> GetISDCodesbyBusinessKey(int businessKey)
        //{
        //    var statdetails = await _VendorRepository.GetISDCodesbyBusinessKey(businessKey);
        //    return Ok(statdetails);
        //}

        #endregion

        #region Vendor Bank Details
        /// <summary>
        /// Getting  Bank Details by Vendor code for Grid.
        /// UI Reffered -Vendor Bank Details Grid
        /// params-Vendorcode
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetVendorBankdetailsByVendorcode(int vendorID)
        {
            var bankdetails = await _VendorRepository.GetVendorBankdetailsByVendorcode(vendorID);
            return Ok(bankdetails);
        }
        /// <summary>
        /// Insert Vendor Bank Details.
        /// UI Reffered -Vendor Bank Details
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertVendorBankdetails(DO_VendorBankdetails objbank)
        {
            var msg = await _VendorRepository.InsertVendorBankdetails(objbank);
            return Ok(msg);

        }

        /// <summary>
        /// Update Vendor Bank Details.
        /// UI Reffered -Vendor Bank Details
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateVendorBankdetails(DO_VendorBankdetails objbank)
        {
            var msg = await _VendorRepository.UpdateVendorBankdetails(objbank);
            return Ok(msg);

        }
        #endregion Vendor Bank Details

        #region Vendor Supply Group
        /// <summary>
        /// Getting  Vendor Suuply Group from Subledger type.
        /// UI Reffered - Vendor Suuply Group Grid
        /// params-vendorID 
        /// </summary>     
        [HttpGet]
        public async Task<IActionResult> GetVendorSuuplyGroupSubledgerType(string subledgertype)
        {
            var supply = await _VendorRepository.GetVendorSuuplyGroupSubledgerType(subledgertype);
            return Ok(supply);
        }
        /// <summary>
        /// Getting  Vendor Suuply Group.
        /// UI Reffered - Vendor Suuply Group Grid
        /// params-vendorID 
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetVendorSuuplyGroupParameterList(int vendorID)
        {
            var supply = await _VendorRepository.GetVendorSuuplyGroupParameterList(vendorID);
            return Ok(supply);
        }
        /// <summary>
        /// Insert Or Update Vendor Supply Group
        /// UI Reffered -Vendor Supply Group
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertSuuplyGroupforVendor(DO_VendorSupplyGroup objsupply)
        {
            var msg = await _VendorRepository.InsertSuuplyGroupforVendor(objsupply);
            return Ok(msg);

        }
        #endregion Vendor Supply Group

        #region Getting Common Methods
        /// <summary>
        /// Getting  ISD Codes.
        /// UI Reffered -Vendor Locations
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetISDCodes()
        {
            var isdcodes = await _CommonDataRepository.GetISDCodes();
            return Ok(isdcodes);
        }
        /// <summary>
        /// Getting  States by ISD Codes.
        /// UI Reffered -Vendor 
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetStatesbyISDCode(int isdCode)
        {
            var isdcodes = await _CommonDataRepository.GetStatesbyISDCode(isdCode);
            return Ok(isdcodes);
        }
        /// <summary>
        /// Getting  Vendor Class by code types.
        /// UI Reffered -Vendor 
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetApplicationCodesByCodeType(int codeType)
        {
            var isdcodes = await _CommonDataRepository.GetApplicationCodesByCodeType(codeType);
            return Ok(isdcodes);
        }
        
        #endregion
    }
}
