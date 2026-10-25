using LoanManagement.Models;
using Microsoft.EntityFrameworkCore;
namespace LoanManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        public DbSet<Role> Roles {  get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Loan> Loans { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable(nameof(Role), "Loan");

                entity.HasKey(x => x.RoleId);

                entity.Property(x => x.RoleId).HasColumnName("role_id").ValueGeneratedOnAdd();

                entity.Property(x => x.RoleName).HasColumnName("role_name");
                    
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable(nameof(User), "Loan");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();

                entity.Property(x => x.Name).HasColumnName("name");
                entity.Property(x => x.Email).HasColumnName("email");
                entity.Property(x => x.Password).HasColumnName("password");
                entity.Property(x => x.PhoneNumber).HasColumnName("phone");
                entity.Property(x => x.RoleId).HasColumnName("role_id");
            });

            modelBuilder.Entity<Loan>(entity =>
            {
                entity.ToTable("LoanApplication", "Loan");

                entity.HasKey(x => x.LoanId);

                entity.Property(x => x.LoanId).HasColumnName("loan_id").ValueGeneratedOnAdd();

                entity.Property(x => x.LoanAmount).HasColumnName("loan_amount");
                entity.Property(x => x.TenorMonth).HasColumnName("tenor_month");
                entity.Property(x => x.InterestRate).HasColumnName("interest_rate");
                entity.Property(x => x.Status).HasConversion<string>().HasMaxLength(20).HasColumnName("loan_status");
                entity.Property(x => x.CustomerName).HasColumnName("customer_name");
                entity.Property(x => x.CustomerEmail).HasColumnName("customer_email");
                entity.Property(x => x.SalesOfficerId).HasColumnName("sales_officer_id");
            });
        }
    }
    
}
