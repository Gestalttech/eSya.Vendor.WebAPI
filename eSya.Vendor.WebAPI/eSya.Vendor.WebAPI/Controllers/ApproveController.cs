using eSya.Vendor.DL.Entities;
using eSya.Vendor.DL.Repository;
using eSya.Vendor.DO;
using eSya.Vendor.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eSya.Vendor.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApproveController : ControllerBase
    {
        private readonly IApproveRepository _ApproveRepository;
        public ApproveController(IApproveRepository ApproveRepository)
        {
            _ApproveRepository = ApproveRepository;
        }
        [HttpGet]
        public IActionResult GetVendorsForApprovals()
        {
            var vendors = _ApproveRepository.GetVendorsForApprovals();
            return Ok(vendors);
        }
        [HttpPost]
        public IActionResult ApproveVendor(DO_VendorApproval obj)
        {
            var vendors = _ApproveRepository.ApproveVendor(obj);
            return Ok(vendors);
        }
    }
}
