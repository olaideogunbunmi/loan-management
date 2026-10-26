using LoanManagement.Data;
using LoanManagement.Dtos;
using LoanManagement.Interfaces;
using LoanManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LoanManagement.Business
{
    public class LoanService : ILoanService
    {
        private readonly ApplicationDbContext _context;

        public LoanService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<CreateResponseDto> CreateAsync(CreateLoanDto dto, string email)
        {
            var salesOfficerId = await _context.Users.Where(x => x.Email == email).Select(x => x.Id).FirstOrDefaultAsync();

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

            return new CreateResponseDto()
            {
                Success = true
            };
        }

        public async Task<ReviewResponseDto> ReviewAsync()
        {
            var loans = await _context.Loans.Where(x => x.Status == LoanStatus.Pending).ToListAsync();

            if (!loans.Any())
            {
                return new ReviewResponseDto()
                {
                    Success = false
                };
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

            return new ReviewResponseDto()
            {
                Success = true,
                RejectedLoanCount = rejectedCount,
                CapturedLoanCount = capturedCount,
            };           
        }

        public async Task<CaptureResponseDto> CaptureAsync()
        {
            var loans = await _context.Loans.Where(x => x.Status == LoanStatus.Captured).ToListAsync();

            if (!loans.Any())
            {
                return new CaptureResponseDto()
                {
                    Success = false
                };
            }

            foreach (var loan in loans)
            {
                loan.Status = LoanStatus.Approved;
            }

            await _context.SaveChangesAsync();

            return new CaptureResponseDto()
            {
                Success = true,
                ApprovedLoanCount = loans.Count
            };
        }

        public async Task<List<LoanDto>> GetLoansAsync()
        {
            var loans = await _context.Loans.AsNoTracking().Select(x => new LoanDto() 
            {
                LoanAmount =  x.LoanAmount,
                 TenorMonth = x.TenorMonth, 
                InterestRate =  x.InterestRate, 
                 Status = x.Status.ToString(), 
                CustomerName = x.CustomerName, 
                CustomerEmail = x.CustomerEmail 
            }).ToListAsync();

            return loans;
        }
    }
}
