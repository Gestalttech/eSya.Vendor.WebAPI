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
        public  async Task<IActionResult> GetVendorsForApprovals(string approved)
        {
            var vendors =await _ApproveRepository.GetVendorsForApprovals(approved);
            return Ok(vendors);
        }
        [HttpPost]
        public async Task<IActionResult> ApproveVendor(DO_VendorApproval obj)
        {
            var vendors =await _ApproveRepository.ApproveVendor(obj);
            return Ok(vendors);
        }
    }
}
