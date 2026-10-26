using LoanManagement.Dtos;
using LoanManagement.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LoanManagement.Controllers
{
    [Route("api/loans")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly ILoanService _loanService;
        public LoansController(ILoanService loanService)
        {
            _loanService = loanService;
        }


        [HttpPost, Authorize(Roles = "Sales, Admin")]
        public async Task<ActionResult> Create(CreateLoanDto dto)
        {
            var salesOfficerEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email);

            var result = await _loanService.CreateAsync(dto, salesOfficerEmail);

            return Ok(result.Success);
        }


        [HttpPost("review"), Authorize(Roles = "Risk, Admin")]
        public async Task<ActionResult> Review()
        {

            var result = await _loanService.ReviewAsync();

            if (!result.Success)
            {
                return Ok("No loans on pending queue....");
            }

            return Ok(new
            {
                RejectedCount = result.RejectedLoanCount,
                CapturedCount = result.CapturedLoanCount,
            });
        }


        [HttpPost("capture"), Authorize(Roles = "Operations, Admin")]
        public async Task<ActionResult> CaptureLoans()
        {
            var result = await _loanService.CaptureAsync();

            if (!result.Success)
            {
                return Ok("No loans on captured queue...");
            }

            return Ok(new
            {
                ApprovedCount = result.ApprovedLoanCount,
            });
        }


        [HttpGet, Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetLoans()
        {
            var result = await _loanService.GetLoansAsync();

            return Ok(result);
        }
    }
}
