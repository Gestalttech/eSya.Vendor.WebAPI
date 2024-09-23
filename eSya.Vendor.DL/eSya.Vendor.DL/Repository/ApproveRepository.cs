using eSya.Vendor.DL.Entities;
using eSya.Vendor.DO;
using eSya.Vendor.IF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.Vendor.DL.Repository
{
    public class ApproveRepository: IApproveRepository
    {
        private readonly IStringLocalizer<ApproveRepository> _localizer;
        public ApproveRepository(IStringLocalizer<ApproveRepository> localizer)
        {
            _localizer = localizer;
        }
        public async Task<List<DO_VendorRegistration>> GetVendorsForApprovals(string approved)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    if (approved == "Approved")
                    {
                        var result = await db.GtEavncds.Where(x => x.ApprovalStatus && x.IsBlackListed == false && x.ActiveStatus && x.RejectionReason==null)
                    .Select(x => new DO_VendorRegistration
                    {
                        VendorId = x.VendorId,
                        VendorName = x.VendorName,
                        VendorClass = x.VendorClass,
                        CreditType = x.CreditType,
                        CreditPeriod = x.CreditPeriod,
                        PreferredPaymentMode = x.PreferredPaymentMode,
                        ApprovalStatus = x.ApprovalStatus,
                        IsBlackListed = x.IsBlackListed,
                        ReasonForBlacklist = x.ReasonForBlacklist,
                        SupplierScore = x.SupplierScore,
                        ActiveStatus = x.ActiveStatus,
                    }).OrderBy(x => x.VendorName).ToListAsync();
                        return result;
                    }
                    else if(approved == "Rejected")
                    {
                        var result = await db.GtEavncds.Where(x => !x.ApprovalStatus && x.IsBlackListed == false && x.ActiveStatus && x.RejectionReason != null)
               .Select(x => new DO_VendorRegistration
               {
                   VendorId = x.VendorId,
                   VendorName = x.VendorName,
                   VendorClass = x.VendorClass,
                   CreditType = x.CreditType,
                   CreditPeriod = x.CreditPeriod,
                   PreferredPaymentMode = x.PreferredPaymentMode,
                   ApprovalStatus = x.ApprovalStatus,
                   IsBlackListed = x.IsBlackListed,
                   ReasonForBlacklist = x.ReasonForBlacklist,
                   SupplierScore = x.SupplierScore,
                   ActiveStatus = x.ActiveStatus,
               }).OrderBy(x => x.VendorName).ToListAsync();
                        return result;
                    }
                    else
                    {
                        var result = await db.GtEavncds.Where(x =>! x.ApprovalStatus && x.IsBlackListed == false && x.ActiveStatus && x.RejectionReason == null)
                 .Select(x => new DO_VendorRegistration
                 {
                     VendorId = x.VendorId,
                     VendorName = x.VendorName,
                     VendorClass = x.VendorClass,
                     CreditType = x.CreditType,
                     CreditPeriod = x.CreditPeriod,
                     PreferredPaymentMode = x.PreferredPaymentMode,
                     ApprovalStatus = x.ApprovalStatus,
                     IsBlackListed = x.IsBlackListed,
                     ReasonForBlacklist = x.ReasonForBlacklist,
                     SupplierScore = x.SupplierScore,
                     ActiveStatus = x.ActiveStatus,
                 }).OrderBy(x => x.VendorName).ToListAsync();
                        return result;
                    }
                }
                
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<DO_ReturnParameter> ApproveVendor(DO_VendorApproval obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEavncd vendor = db.GtEavncds.Where(x => x.VendorId == obj.VendorId).FirstOrDefault();
                        if (vendor == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0175", Message = string.Format(_localizer[name: "W0175"]) };
                        }

                        vendor.ApprovalStatus = true;
                        vendor.RejectionReason = null;
                        vendor.ModifiedOn = DateTime.Now;
                        vendor.ModifiedBy =obj.UserID;
                        vendor.ModifiedTerminal = obj.TerminalID;
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                       return new DO_ReturnParameter() { Status = true, StatusCode = "S0006", Message = string.Format(_localizer[name: "S0006"]) };
                       
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
        public async Task<DO_ReturnParameter> RejectVendor(DO_VendorApproval obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEavncd vendor = db.GtEavncds.Where(x => x.VendorId == obj.VendorId).FirstOrDefault();
                        if (vendor == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0175", Message = string.Format(_localizer[name: "W0175"]) };
                        }

                        vendor.ApprovalStatus = false;
                        vendor.RejectionReason = obj.RejectionReason;
                        vendor.ModifiedOn = DateTime.Now;
                        vendor.ModifiedBy = obj.UserID;
                        vendor.ModifiedTerminal = obj.TerminalID;
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0006", Message = string.Format(_localizer[name: "S0006"]) };

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
    }
}
