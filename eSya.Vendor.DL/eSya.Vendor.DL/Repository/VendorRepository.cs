using eSya.Vendor.DL.Entities;
using eSya.Vendor.DO;
using eSya.Vendor.IF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace eSya.Vendor.DL.Repository
{
    public class VendorRepository : IVendorRepository
    {
        private readonly IStringLocalizer<VendorRepository> _localizer;
        public VendorRepository(IStringLocalizer<VendorRepository> localizer)
        {
            _localizer = localizer;
        }

        #region Vendor Registration
        public List<DO_VendorRegistration> GetVendors(string Alphabet)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    List<GtEavncd> vendor_list = new List<GtEavncd>();

                    if (!string.IsNullOrEmpty(Alphabet))
                    {

                        vendor_list = db.GtEavncds.Where(x => x.VendorName.ToUpper().Trim().StartsWith(Alphabet.ToUpper().Trim())).ToList();
                    }
                    if (Alphabet == "All")
                    {
                        vendor_list = db.GtEavncds.ToList();
                    }
                    var result = vendor_list.
                     Select(x => new DO_VendorRegistration
                     {
                         VendorId = x.VendorId,
                         VendorName = x.VendorName,
                         VendorClass=x.VendorClass,
                         CreditType = x.CreditType,
                         CreditPeriod = x.CreditPeriod,
                         PreferredPaymentMode = x.PreferredPaymentMode,
                         ApprovalStatus = x.ApprovalStatus,
                         IsBlackListed = x.IsBlackListed,
                         ReasonForBlacklist = x.ReasonForBlacklist,
                         SupplierScore = x.SupplierScore,
                         ActiveStatus = x.ActiveStatus,
                     }).OrderBy(x => x.VendorName).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<List<DO_eSyaParameter>> GetVendorParameterList(int vendorID)
        {
            using (var db = new eSyaEnterprise())
            {
                try
                {
                    var ds = db.GtEavnpas
                        .Where(s => s.VendorId == vendorID)
                        .Select(p => new DO_eSyaParameter
                        {
                            ParameterID = p.ParameterId,
                            ParmAction = p.ActiveStatus
                        }).ToListAsync();
                    return await ds;


                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public async Task<DO_ReturnParameter> InsertOrUpdateVendor(DO_VendorRegistration obj)
        {
            try
            {
                if (obj.VendorId != 0)
                {
                    return await UpdateVendor(obj);
                }
                else
                {
                    return await InsertVendor(obj);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DO_ReturnParameter> InsertVendor(DO_VendorRegistration obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        GtEavncd is_vendorExists = db.GtEavncds.FirstOrDefault(c => c.VendorName.ToUpper().Replace(" ", "") == obj.VendorName.ToUpper().Replace(" ", ""));
                        if (is_vendorExists == null)
                        {
                            int maxval = db.GtEavncds.Select(c => c.VendorId).DefaultIfEmpty().Max();
                            int v_code = maxval + 1;
                            var obj_vendor = new GtEavncd
                            {
                                VendorId = v_code,
                                VendorName = obj.VendorName,
                                VendorClass=obj.VendorClass,
                                CreditType = obj.CreditType,
                                PreferredPaymentMode = obj.PreferredPaymentMode,
                                CreditPeriod =Convert.ToInt32( obj.CreditPeriod),
                                ApprovalStatus = obj.ApprovalStatus,
                                IsBlackListed = obj.IsBlackListed,
                                ReasonForBlacklist = obj.ReasonForBlacklist,
                                SupplierScore = obj.SupplierScore,
                                ActiveStatus = obj.ActiveStatus,
                                FormId = obj.FormID,
                                CreatedBy = obj.UserID,
                                CreatedOn = DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtEavncds.Add(obj_vendor);
                            List<GtEavnpa> venparam = db.GtEavnpas.Where(p => p.VendorId == obj_vendor.VendorId).ToList();
                            if (obj.l_FormParameter != null)
                            {
                                if (venparam.Count > 0)
                                {
                                    foreach (var p in venparam)
                                    {
                                        db.GtEavnpas.Remove(p);
                                        db.SaveChanges();
                                    }

                                }
                                foreach (var param in obj.l_FormParameter)
                                {
                                    GtEavnpa objparam = new GtEavnpa
                                    {
                                        VendorId = v_code,
                                        ParameterId = param.ParameterID,
                                        ActiveStatus = param.ParmAction,
                                        ParmPerc=param.ParmPerct,
                                        ParmValue=param.ParmValue,
                                        ParmDesc="-",
                                        ParmAction=param.ParmAction,
                                        FormId = obj.FormID,
                                        CreatedBy = obj.UserID,
                                        CreatedOn = System.DateTime.Now,
                                        CreatedTerminal = obj.TerminalID,

                                    };
                                    db.GtEavnpas.Add(objparam);
                                    await db.SaveChangesAsync();

                                }

                                dbContext.Commit();
                                return new DO_ReturnParameter() { Status = true, StatusCode = "S0001",VendorId= v_code, Message = string.Format(_localizer[name: "S0001"]) };
                            }

                            else
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0172", Message = string.Format(_localizer[name: "W0172"]) };
                            }


                        }
                        else
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0173", Message = string.Format(_localizer[name: "W0173"]) };
                        }
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public async Task<DO_ReturnParameter> UpdateVendor(DO_VendorRegistration obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {

                    try
                    {
                        GtEavncd is_vendorExists = db.GtEavncds.FirstOrDefault(c => c.VendorId != obj.VendorId && c.VendorName.ToUpper().Replace(" ", "") == obj.VendorName.ToUpper().Replace(" ", ""));

                        var _objven = db.GtEavncds.FirstOrDefault(x => x.VendorId == obj.VendorId);
                        if (_objven != null)
                        {
                            if (is_vendorExists == null)
                            {
                                _objven.VendorName = obj.VendorName;
                                _objven.VendorClass = obj.VendorClass;
                                _objven.CreditType = obj.CreditType;
                                _objven.CreditPeriod =Convert.ToInt32(obj.CreditPeriod);
                                _objven.PreferredPaymentMode = obj.PreferredPaymentMode;
                                _objven.ApprovalStatus = obj.ApprovalStatus;
                                _objven.IsBlackListed = obj.IsBlackListed;
                                _objven.ReasonForBlacklist = obj.ReasonForBlacklist;
                                _objven.SupplierScore = obj.SupplierScore;
                                _objven.ActiveStatus = obj.ActiveStatus;
                                _objven.ModifiedBy = obj.UserID;
                                _objven.ModifiedOn = DateTime.Now;
                                _objven.ModifiedTerminal = obj.TerminalID;
                                await db.SaveChangesAsync();
                                List<GtEavnpa> vendorparam = db.GtEavnpas.Where(p => p.VendorId == obj.VendorId).ToList();
                                if (obj.l_FormParameter != null)
                                {
                                    if (vendorparam.Count > 0)
                                    {
                                        foreach (var p in vendorparam)
                                        {
                                            db.GtEavnpas.Remove(p);
                                            db.SaveChanges();
                                        }

                                    }
                                    foreach (var param in obj.l_FormParameter)
                                    {
                                        GtEavnpa objparam = new GtEavnpa
                                        {
                                            VendorId = obj.VendorId,
                                            ParameterId = param.ParameterID,
                                            ActiveStatus = param.ParmAction,
                                            ParmPerc = param.ParmPerct,
                                            ParmValue = param.ParmValue,
                                            ParmDesc = "-",
                                            ParmAction = param.ParmAction,
                                            FormId = obj.FormID,
                                            CreatedBy = obj.UserID,
                                            CreatedOn = System.DateTime.Now,
                                            CreatedTerminal = obj.TerminalID,

                                        };
                                        db.GtEavnpas.Add(objparam);
                                        await db.SaveChangesAsync();

                                    }

                                    dbContext.Commit();
                                    return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", VendorId = obj.VendorId, Message = string.Format(_localizer[name: "S0002"]) };

                                }

                                else
                                {
                                    return new DO_ReturnParameter() { Status = false, StatusCode = "W0174", Message = string.Format(_localizer[name: "W0174"]) };
                                }
                            }
                            else
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0173", Message = string.Format(_localizer[name: "W0173"]) };
                            }
                        }
                        else
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0175", Message = string.Format(_localizer[name: "W0175"]) };

                        }

                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public async Task<DO_ReturnParameter> ActiveOrDeActiveVendor(bool status, int vendorID)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEavncd vendor = db.GtEavncds.Where(x => x.VendorId == vendorID).FirstOrDefault();
                        if (vendor == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0175", Message = string.Format(_localizer[name: "W0175"]) };
                        }

                        vendor.ActiveStatus = status;
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        if (status == true)
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0003", Message = string.Format(_localizer[name: "S0003"]) };
                        else
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0004", Message = string.Format(_localizer[name: "S0004"]) };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));

                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }
        #endregion Vendor Registration

        #region Vendor Location
        public async Task<List<DO_VendorLocation>> GetVendorLocationsByVendorcode(int vendorID)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var vendor_loc = db.GtEavnsls.Where(v => v.VendorId == vendorID).
                      Select(x => new DO_VendorLocation
                      {
                          VendorId = x.VendorId,
                          VendorLocationId = x.VendorLocationId,
                          VendorLocation = x.VendorLocation,
                          IsLocationDefault = x.IsLocationDefault,
                          VendorAddress = x.VendorAddress,
                          StateCode=x.StateCode,
                          Isdcode = x.Isdcode,
                          ContactPerson = x.ContactPerson,
                          MobileNumber = x.MobileNumber,
                          WIsdcode=x.Wisdcode,
                          WhatsappNumber = x.WhatsappNumber,
                          EMailId = x.EMailId,
                          ActiveStatus = x.ActiveStatus
                      }).OrderBy(x => x.VendorLocationId).ToListAsync();
                    return await vendor_loc;

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertOrUpdateVendorLocation(DO_VendorLocation objloc)
        {
            try
            {
                if (objloc.VendorLocationId != 0)
                {
                    return await UpdateVendorLocation(objloc);
                }
                else
                {
                    return await InsertVendorLocation(objloc);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DO_ReturnParameter> InsertVendorLocation(DO_VendorLocation objloc)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var is_locExists = db.GtEavnsls.Where(x => x.VendorId == objloc.VendorId &&
                            x.VendorAddress.ToUpper().Replace(" ", "") == objloc.VendorAddress.ToUpper().Replace(" ", "") && x.MobileNumber == objloc.MobileNumber &&
                            x.WhatsappNumber == objloc.WhatsappNumber && x.EMailId.ToUpper().Replace(" ", "") == objloc.EMailId.ToUpper().Replace(" ", "") && x.ActiveStatus == true).FirstOrDefault();

                        GtEavnsl is_locdefaultExists = db.GtEavnsls.FirstOrDefault(c => c.IsLocationDefault == true && c.VendorId == objloc.VendorId && c.ActiveStatus == true);

                        if (is_locExists == null)
                        {

                            if (objloc.IsLocationDefault == true && is_locdefaultExists != null)
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0176", Message = string.Format(_localizer[name: "W0176"]) };
                            }
                            else
                            {
                                int maxval = db.GtEavnsls.Where(x => x.VendorId == objloc.VendorId).Select(c => c.VendorLocationId).DefaultIfEmpty().Max();
                                int locId = maxval + 1;
                                var ven_loc = new GtEavnsl
                                {
                                    VendorId = objloc.VendorId,
                                    VendorLocationId = locId,
                                    VendorLocation = objloc.VendorLocation,
                                    IsLocationDefault = objloc.IsLocationDefault,
                                    VendorAddress = objloc.VendorAddress,
                                    Isdcode = objloc.Isdcode,
                                    StateCode= objloc.StateCode,
                                    ContactPerson = objloc.ContactPerson,
                                    MobileNumber = objloc.MobileNumber,
                                    Wisdcode=objloc.WIsdcode,
                                    WhatsappNumber = objloc.WhatsappNumber,
                                    EMailId = objloc.EMailId,
                                    ActiveStatus = objloc.ActiveStatus,
                                    FormId = objloc.FormID,
                                    CreatedBy = objloc.UserID,
                                    CreatedOn = DateTime.Now,
                                    CreatedTerminal = objloc.TerminalID
                                };
                                db.GtEavnsls.Add(ven_loc);
                                await db.SaveChangesAsync();
                            }
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                        }
                        else
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0177",  Message = string.Format(_localizer[name: "W0177"]) };
                        }

                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public async Task<DO_ReturnParameter> UpdateVendorLocation(DO_VendorLocation objloc)

        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {

                    try

                    {
                        var is_locExists = db.GtEavnsls.Where(x => x.VendorLocationId != objloc.VendorLocationId && x.VendorAddress.ToUpper().Replace(" ", "") == objloc.VendorAddress.ToUpper().Replace(" ", "") &&
                           x.MobileNumber == objloc.MobileNumber && x.WhatsappNumber == objloc.WhatsappNumber
                          && x.EMailId.ToUpper().Replace(" ", "") == objloc.EMailId.ToUpper().Replace(" ", "") && x.ActiveStatus == true).FirstOrDefault();

                        var ven_loc = db.GtEavnsls.Where(x => x.VendorId == objloc.VendorId && x.VendorLocationId == objloc.VendorLocationId).FirstOrDefault();

                        GtEavnsl is_defaultlocExists = db.GtEavnsls.FirstOrDefault(c => c.IsLocationDefault == true && c.VendorId == objloc.VendorId && c.VendorLocationId != objloc.VendorLocationId && c.ActiveStatus == true);


                        if (ven_loc != null)
                        {
                            if (is_locExists == null)
                            {
                                if (objloc.IsLocationDefault && is_defaultlocExists != null)
                                {
                                    return new DO_ReturnParameter() { Status = false, StatusCode = "W0176", Message = string.Format(_localizer[name: "W0176"]) };
                                }
                                else
                                {
                                    ven_loc.VendorLocation = objloc.VendorLocation;
                                    ven_loc.IsLocationDefault = objloc.IsLocationDefault;
                                    ven_loc.VendorAddress = objloc.VendorAddress;
                                    ven_loc.StateCode = objloc.StateCode;
                                    ven_loc.Isdcode = objloc.Isdcode;
                                    ven_loc.ContactPerson = objloc.ContactPerson;
                                    ven_loc.MobileNumber = objloc.MobileNumber;
                                    ven_loc.Wisdcode = objloc.WIsdcode;
                                    ven_loc.WhatsappNumber = objloc.WhatsappNumber;
                                    ven_loc.EMailId = objloc.EMailId;
                                    ven_loc.ActiveStatus =objloc.ActiveStatus;
                                    ven_loc.ModifiedBy = objloc.UserID;
                                    ven_loc.ModifiedOn = DateTime.Now;
                                    ven_loc.ModifiedTerminal = objloc.TerminalID;
                                    await db.SaveChangesAsync();
                                    dbContext.Commit();
                                    return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };
                                }

                            }
                            else
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0177", Message = string.Format(_localizer[name: "W0177"]) };
                            }
                        }
                        else
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0175", Message = string.Format(_localizer[name: "W0175"]) };
                        }
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        #endregion Vendor Location

        #region Vendor Business Link
        public async Task<List<DO_BusinessKey>> GetBusinessLocation()
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var result = db.GtEcbslns.Where(x=>x.ActiveStatus)
                           
                            .Select(c => new DO_BusinessKey
                            {
                                BusinessLocation = c.BusinessName+ "-" + c.LocationDescription,
                                BusinessKey = c.BusinessKey

                            }).ToListAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_VendorBusinessLink>> GetBusinessKeysByVendorcode(int vendorID)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {

                    var result = db.GtEavnbls.Where(x => x.VendorId == vendorID).
                      Select(x => new DO_VendorBusinessLink
                      {
                          VendorId = x.VendorId,
                          BusinessKey = x.BusinessKey,
                          ActiveStatus = x.ActiveStatus
                      }).ToListAsync();
                    return await result;

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<DO_ReturnParameter> InsertBusinesskeyforVendor(DO_VendorBusinessLink objkey)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        List<GtEavnbl> ven_links = db.GtEavnbls.Where(c => c.VendorId == objkey.VendorId).ToList();
                        if (objkey.Businesslink != null)
                        {
                            if (ven_links.Count > 0)
                            {
                                foreach (var obj in ven_links)
                                {
                                    db.GtEavnbls.Remove(obj);
                                    db.SaveChanges();
                                }

                            }
                            foreach (var key in objkey.Businesslink)
                            {
                                GtEavnbl objkeys = new GtEavnbl
                                {
                                    VendorId = objkey.VendorId,
                                    BusinessKey = key,
                                    ActiveStatus = true,
                                    FormId = objkey.FormID,
                                    CreatedBy = objkey.UserID,
                                    CreatedOn = DateTime.Now,
                                    CreatedTerminal = objkey.TerminalID
                                };
                                db.GtEavnbls.Add(objkeys);
                                await db.SaveChangesAsync();

                            }

                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                        }

                        else
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Business Location Linked Failed." };
                        }
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        #endregion Vendor Business Link

        #region Vendor Statutory Details need to remove

        public async Task<List<DO_VendorStatutoryDetails>> GetStatutorydetailsbyVendorcodeAndLocationId(int vendorID, int locationId)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {

                    var result = db.GtEavnsds.Where(x => x.VendorId == vendorID && x.VendorLocationId == locationId).
                      Select(x => new DO_VendorStatutoryDetails
                      {
                          VendorId = x.VendorId,
                          VendorLocationId = x.VendorLocationId,
                          StatutoryCode = x.StatutoryCode,
                          StatutoryDescription = x.StatutoryDescription,
                          ActiveStatus = x.ActiveStatus
                      }).ToListAsync();

                    return await result;

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
       
        public async Task<DO_ReturnParameter> InsertOrUpdateStatutorydetails(DO_VendorStatutoryDetails objsat)
        {
            try
            {
                if (objsat.StatutoryCode != 0)
                {
                    return await UpdateStatutorydetails(objsat);
                }
                else
                {
                    return await InsertStatutorydetails(objsat);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DO_ReturnParameter> InsertStatutorydetails(DO_VendorStatutoryDetails objsat)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (objsat != null)
                        {
                            int maxval = db.GtEavnsds.Where(x => x.VendorId == objsat.VendorId && x.VendorLocationId == objsat.VendorLocationId).Select(c => c.StatutoryCode).DefaultIfEmpty().Max();
                            int statutorycode = maxval + 1;
                            GtEavnsd objsta_details = new GtEavnsd
                            {
                                VendorId = objsat.VendorId,
                                VendorLocationId = objsat.VendorLocationId,
                                StatutoryCode = statutorycode,
                                StatutoryDescription = objsat.StatutoryDescription,
                                ActiveStatus = objsat.ActiveStatus,
                                FormId = objsat.FormID,
                                CreatedBy = objsat.UserID,
                                CreatedOn = DateTime.Now,
                                CreatedTerminal = objsat.TerminalID
                            };

                            db.GtEavnsds.Add(objsta_details);
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };

                        }
                        else
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0178", Message = string.Format(_localizer[name: "W0178"]) };
                        }
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public async Task<DO_ReturnParameter> UpdateStatutorydetails(DO_VendorStatutoryDetails objsat)

        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {

                    try

                    {
                        var objstatutory = db.GtEavnsds.Where(x => x.VendorId == objsat.VendorId && x.VendorLocationId == objsat.VendorLocationId
                         && x.StatutoryCode == objsat.StatutoryCode).FirstOrDefault();
                        if (objstatutory != null)
                        {

                            objstatutory.StatutoryDescription = objsat.StatutoryDescription;
                            objstatutory.ActiveStatus = objsat.ActiveStatus;
                            objstatutory.ModifiedBy = objsat.UserID;
                            objstatutory.ModifiedOn = DateTime.Now;
                            objstatutory.ModifiedTerminal = objsat.TerminalID;
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };

                        }
                        else
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0178", Message = string.Format(_localizer[name: "W0178"]) };
                        }
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }


        #endregion

        #region Vendor Statutory Details
        public async Task<List<DO_VendorLocation>> GetVendorAddressLocationsByVendorID(int vendorID)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var vendor_loc =await db.GtEavnsls.Where(v => v.VendorId == vendorID).
                      Select(x => new DO_VendorLocation
                      {
                          VendorId = x.VendorId,
                          VendorLocationId = x.VendorLocationId,
                          VendorLocation = x.VendorAddress + "-" + x.VendorLocation,
                      }).OrderBy(x => x.VendorLocation).ToListAsync();
                    return vendor_loc;

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<List<DO_CountryISDCodes>> GetISDCodesbyVendorId(int vendorID)
        {
            using (var db = new eSyaEnterprise())
            {
                try
                {
                    var dc_ms = db.GtEavnbls
                        .Join(db.GtEcbslns,
                        d => new { d.BusinessKey },
                        a => new { a.BusinessKey },
                        (d, a) => new { d, a })
                        .Join(db.GtEccncds,
                        dd => new { dd.a.Isdcode },
                        aa => new { aa.Isdcode },
                        (dd, aa) => new { dd, aa })
                        .Where(w => w.dd.d.VendorId == vendorID && w.dd.d.ActiveStatus == true && w.aa.ActiveStatus == true
                            && w.dd.a.ActiveStatus == true)
                        .AsNoTracking()
                        .Select(x => new DO_CountryISDCodes
                        {
                            Isdcode = x.dd.a.Isdcode,
                            CountryFlag = x.aa.CountryFlag,
                            CountryCode = x.aa.CountryCode,
                            CountryName = x.aa.CountryName,
                            MobileNumberPattern = x.aa.MobileNumberPattern,

                        }).ToList();
                    if (dc_ms.Count > 0)
                    {
                        var res = dc_ms.GroupBy(x => x.Isdcode).Select(y => y.First()).Distinct();
                        return res.ToList();
                    }
                    else
                    {
                        return dc_ms.ToList();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }

        public async Task<List<DO_VendorStatutoryDetails>> GetVendorStatutoryDetails(int vendorID, int isdCode, int locationId)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {

                    return await db.GtEccnsds.Where(x => x.Isdcode == isdCode && x.ActiveStatus).
                        Join(db.GtEcsupas.Where(x => x.Isdcode == isdCode && x.Action && x.ParameterId == 3),
                        x => new { x.StatutoryCode },
                        y => new { y.StatutoryCode },
                        (x, y) => new { x, y })
                       .GroupJoin(db.GtEavnsds.Where(x => x.VendorId == vendorID && x.VendorLocationId == locationId),
                       m => m.x.StatutoryCode,
                       l => l.StatutoryCode,
                       (m, l) => new
                       { m, l }).SelectMany(z => z.l.DefaultIfEmpty(),
                       (a, b) => new DO_VendorStatutoryDetails
                       {
                           Isdcode = a.m.x.Isdcode,
                           VendorId = vendorID,
                           VendorLocationId = locationId,
                           StatutoryCode = a.m.x.StatutoryCode,
                           StatutoryShortCode = a.m.x.StatShortCode,
                           StatutoryDescription = a.m.x.StatutoryDescription,
                           StatutoryValue = b != null ? b.StatutoryDescription : "",
                           ActiveStatus = b != null ? b.ActiveStatus : false
                       }).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DO_ReturnParameter> InsertOrUpdateVendorStatutoryDetails(List<DO_VendorStatutoryDetails> obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var is_StatutoryDetailEnter = obj.Where(w => !String.IsNullOrEmpty(w.StatutoryValue)).Count();
                        if (is_StatutoryDetailEnter <= 0)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0183", Message = string.Format(_localizer[name: "W0183"]) };
                        }

                        foreach (var sd in obj.Where(w => !String.IsNullOrEmpty(w.StatutoryValue)))
                        {
                            GtEavnsd cs_sd = db.GtEavnsds.Where(x => x.VendorId == sd.VendorId && x.StatutoryCode == sd.StatutoryCode && x.VendorLocationId == sd.VendorLocationId).FirstOrDefault();
                            if (cs_sd == null)
                            {
                                var o_cssd = new GtEavnsd
                                {
                                    VendorId = sd.VendorId,
                                    VendorLocationId = sd.VendorLocationId,
                                    StatutoryCode = sd.StatutoryCode,
                                    StatutoryDescription = sd.StatutoryValue,
                                    ActiveStatus = sd.ActiveStatus,
                                    FormId = sd.FormID,
                                    CreatedBy = sd.UserID,
                                    CreatedOn = System.DateTime.Now,
                                    CreatedTerminal = sd.TerminalID
                                };
                                db.GtEavnsds.Add(o_cssd);
                            }
                            else
                            {
                                cs_sd.StatutoryDescription = sd.StatutoryValue;
                                cs_sd.ActiveStatus = sd.ActiveStatus;
                                cs_sd.ModifiedBy = sd.UserID;
                                cs_sd.ModifiedOn = System.DateTime.Now;
                                cs_sd.ModifiedTerminal = sd.TerminalID;
                            }
                            await db.SaveChangesAsync();
                        }

                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }
       
        //public async Task<List<DO_CountryISDCodes>> GetISDCodesbyBusinessKey(int businessKey)
        //{
        //    using (var db = new eSyaEnterprise())
        //    {
        //        try
        //        {
        //            var do_cl = db.GtEcbslns
        //                .Join(db.GtEccncds.Where(w => w.ActiveStatus),
        //                lc => new { lc.Isdcode },
        //                o => new { o.Isdcode },
        //                (lc, o) => new { lc, o })
        //                .Where(w => w.lc.BusinessKey == businessKey && w.lc.ActiveStatus)
        //               .AsNoTracking()
        //               .Select(r => new DO_CountryISDCodes
        //               {
        //                   Isdcode = r.lc.Isdcode,
        //                   CountryName = r.o.CountryName,
        //                   CountryFlag = r.o.CountryFlag,
        //                   MobileNumberPattern = r.o.MobileNumberPattern,
        //                   CountryCode = r.o.CountryCode,
        //               }).ToListAsync();

        //            return await do_cl;
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //}

        #endregion

        #region Vendor Bank Details

        public async Task<List<DO_VendorBankdetails>> GetVendorBankdetailsByVendorcode(int vendorID)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var result = db.GtEavnbds.Where(x => x.VendorId == vendorID).
                      Select(b => new DO_VendorBankdetails
                      {
                          VendorId = b.VendorId,
                          BenificiaryBankAccountNo = b.BenificiaryBankAccountNo,
                          BenificiaryName = b.BenificiaryName,
                          BenificiaryBankName = b.BenificiaryBankName,
                          BankIfsccode = b.BankIfsccode,
                          BankSwiftcode = b.BankSwiftcode,
                          ActiveStatus = b.ActiveStatus
                      }).ToListAsync();
                    return await result;

                }

            }

            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<DO_ReturnParameter> InsertVendorBankdetails(DO_VendorBankdetails objbank)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (objbank != null)
                        {
                            var is_AcnoExists = db.GtEavnbds.Where(x => x.VendorId == objbank.VendorId &&
                              x.BenificiaryBankAccountNo.ToUpper().Replace(" ", "") == objbank.BenificiaryBankAccountNo.ToUpper().Replace(" ", "") && x.ActiveStatus == true).FirstOrDefault();

                            if (is_AcnoExists == null)
                            {
                                var objbdetails = new GtEavnbd()
                                {
                                    VendorId = objbank.VendorId,
                                    BenificiaryBankAccountNo = objbank.BenificiaryBankAccountNo,
                                    BenificiaryName = objbank.BenificiaryName,
                                    BenificiaryBankName = objbank.BenificiaryBankName,
                                    BankIfsccode = objbank.BankIfsccode,
                                    BankSwiftcode = objbank.BankSwiftcode,
                                    ActiveStatus = objbank.ActiveStatus,
                                    FormId = objbank.FormID,
                                    CreatedBy = objbank.UserID,
                                    CreatedOn = DateTime.Now,
                                    CreatedTerminal = objbank.TerminalID
                                };
                                db.GtEavnbds.Add(objbdetails);
                                await db.SaveChangesAsync();
                                dbContext.Commit();
                                return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };

                            }
                            else
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0179", Message = string.Format(_localizer[name: "W0179"]) };
                            }

                        }
                        else
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0180", Message = string.Format(_localizer[name: "W0180"]) };
                        }

                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public async Task<DO_ReturnParameter> UpdateVendorBankdetails(DO_VendorBankdetails objbank)

        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {

                    try

                    {

                        var objbdetails = db.GtEavnbds.Where(x => x.VendorId == objbank.VendorId && x.BenificiaryBankAccountNo.ToUpper().Replace(" ", "") == objbank.BenificiaryBankAccountNo.ToUpper().Replace(" ", "")).FirstOrDefault();
                        if (objbdetails != null)
                        {
                            objbdetails.BenificiaryName = objbank.BenificiaryName;
                            objbdetails.BenificiaryBankName = objbank.BenificiaryBankName;
                            objbdetails.BankIfsccode = objbank.BankIfsccode;
                            objbdetails.BankSwiftcode = objbank.BankSwiftcode;
                            objbdetails.ActiveStatus = objbank.ActiveStatus;
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };
                        }
                        else
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0181", Message = string.Format(_localizer[name: "W0181"]) };
                        }
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        #endregion Vendor Bank Details

        #region Vendor Supply Group
        public async Task<List<DO_Parameters>> GetVendorSuuplyGroupSubledgerType(string subledgertype)
        {
            using (var db = new eSyaEnterprise())
            {
                try
                {

                    var ds = db.GtEcsulgs.Where(x => x.SubledgerType == subledgertype && x.ActiveStatus).
                      Select(x => new DO_Parameters
                      {
                          ParameterId = x.SubledgerGroup,
                          ParameterDesc=x.SubledgerDesc,
                          ActiveStatus = x.ActiveStatus
                      }).ToListAsync();
                    return await ds;


                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        public async Task<List<DO_eSyaParameter>> GetVendorSuuplyGroupParameterList(int vendorID)
        {
            using (var db = new eSyaEnterprise())
            {
                try
                {
                    
                        var result = db.GtEavnsgs.Where(x => x.VendorId == vendorID).
                          Select(x => new DO_eSyaParameter
                          {
                              VendorId=x.VendorId,
                              ParameterID = x.SupplyGroupCode,
                              ActiveStatus = x.ActiveStatus
                          }).ToListAsync();
                        return await result;

                    
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public async Task<DO_ReturnParameter> InsertSuuplyGroupforVendor(DO_VendorSupplyGroup objsupply)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        List<GtEavnsg> sgroup = db.GtEavnsgs.Where(c => c.VendorId == objsupply.VendorId).ToList();

                        if (objsupply.l_SupplyGroupParam != null)
                        {
                            if (sgroup.Count > 0)
                            {
                                foreach (var obj in sgroup)
                                {
                                    db.GtEavnsgs.Remove(obj);
                                    db.SaveChanges();
                                }

                            }
                            foreach (var s in objsupply.l_SupplyGroupParam)
                            {
                                GtEavnsg objkeys = new GtEavnsg
                                {
                                    VendorId = objsupply.VendorId,
                                    SupplyGroupCode = s.ParameterID,
                                    ActiveStatus = s.ActiveStatus,
                                    FormId = objsupply.FormID,
                                    CreatedBy = objsupply.UserID,
                                    CreatedOn = DateTime.Now,
                                    CreatedTerminal = objsupply.TerminalID
                                };
                                db.GtEavnsgs.Add(objkeys);
                                await db.SaveChangesAsync();

                            }

                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };
                        }

                        else
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0182", Message = string.Format(_localizer[name: "W0182"]) };
                        }
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        #endregion Vendor Supply Group

    }
}
