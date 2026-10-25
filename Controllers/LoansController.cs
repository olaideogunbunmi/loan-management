using LoanManagement.Data;
using LoanManagement.Dtos;
using LoanManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LoanManagement.Controllers
{
    [Route("api/loans")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public LoansController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpPost, Authorize(Roles = "Sales, Admin")]
        public async Task<ActionResult> Create(CreateLoanDto dto)
        {
            var salesOfficerEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email);

            var salesOfficerId = await _context.Users.Where(x => x.Email == salesOfficerEmail).Select(x => x.Id).FirstOrDefaultAsync();

            var loan = new Loan()
            {
                LoanAmount = dto.Amount,
                TenorMonth = dto.TenorMonth,
                InterestRate = dto.InterestRate,
                CustomerName = dto.CustomerName,
                CustomerEmail = dto.CustomerEmail,
                SalesOfficerId = salesOfficerId,
            };

            loan.Status = LoanStatus.Pending;

            await _context.Loans.AddAsync(loan);
            await _context.SaveChangesAsync();

            return Created();
        }

        [HttpPost("review"), Authorize(Roles = "Risk, Admin")]
        public async Task<ActionResult> ReviewLoans()
        {
            var loans = await _context.Loans.Where(x => x.Status == LoanStatus.Pending).ToListAsync();

            if (!loans.Any())
            {
                return NotFound("No loans on pending queue...");
            }

            decimal minimumAmount = 10000M;
            byte minimumTenor = 3;


            int rejectedCount = 0;
            int capturedCount = 0;

            foreach (var loan in loans)
            {
                if (loan.LoanAmount < minimumAmount || loan.TenorMonth < minimumTenor)
                {
                    loan.Status = LoanStatus.Rejected;
                    rejectedCount++;
                }
                else
                {
                    loan.Status = LoanStatus.Captured;
                    capturedCount++;
                }
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                RejectedLoanCount = rejectedCount,
                CapturedLoanCount = capturedCount,
            });
        }

        [HttpPost("capture"), Authorize(Roles = "Operations, Admin")]
        public async Task<ActionResult> CaptureLoans()
        {
            var loans = await _context.Loans.Where(x => x.Status == LoanStatus.Captured).ToListAsync();

            if (!loans.Any())
            {
                return NotFound("No loans on captured queue...");
            }

            foreach (var loan in loans)
            {
                loan.Status = LoanStatus.Approved;
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                ApprovedLoanCOunt = loans.Count
            });
        }

        [HttpGet, Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<string>>> GetLoans()
        {
            var loans = await _context.Loans.AsNoTracking().Select(x => new { x.LoanAmount, x.TenorMonth, x.InterestRate, x.Status, x.CustomerName, x.CustomerEmail }).ToListAsync();

            return Ok(loans);
        }
    }
}
