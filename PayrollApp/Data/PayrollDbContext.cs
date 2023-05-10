

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PayrollApp.Models;

namespace PayrollApp.Data
{
    public class PayrollDbContext : IdentityDbContext<AppUser,AppRole,string>
    {
        public PayrollDbContext(DbContextOptions<PayrollDbContext> options) : base(options) { }


        public DbSet<Position> Positions { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Allowance> Allowances { get; set; }

        public DbSet<Deduction> Deductions { get; set; }

        public DbSet<Payroll> Payrolls { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<EmployeeAllowance> EmployeeAllowances { get; set; }

        public DbSet<EmployeeDeduction> EmployeeDeductions { get; set; }

        public DbSet<EmployeePayroll> EmployeePayrolls { get; set; }

        public DbSet<EmployeePayrollAllowance> EmployeePayrollAllowances { get; set; }
        public DbSet<EmployeePayrollDeduction> EmployeePayrollDeductions { get; set; }

        public DbSet<VWEmployee> VWEmployees { get; set; }  

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // fluent api
            // unique key  for employee's empno and idno
            modelBuilder.Entity<Employee>().HasAlternateKey(e => e.IDNo);
            modelBuilder.Entity<Employee>().HasAlternateKey(e => e.EmpNo);

            modelBuilder.Entity<Employee>().Property(e => e.FullName)
                .HasComputedColumnSql(
                $"{nameof(Employee.FirstName)} + ' ' + {nameof(Employee.SecondName)} + ' ' + {nameof(Employee.ThirdName)} +' '+ {nameof(Employee.LastName)}");



            modelBuilder.Entity<EmployeeAllowance>().HasAlternateKey(ea => new { ea.AllowanceId, ea.EmployeeId });
            modelBuilder.Entity<EmployeeDeduction>().HasAlternateKey(ea => new { ea.DeductionId, ea.EmployeeId });

            modelBuilder.Entity<EmployeePayroll>().HasAlternateKey(ea => new { ea.PayrollId, ea.EmployeeId });

            modelBuilder.Entity<EmployeePayroll>().Property(ea => ea.NetAmount)
                .HasComputedColumnSql($"{nameof(EmployeePayroll.BasicSalary)} +  {nameof(EmployeePayroll.AllowanceAmount)} - {nameof(EmployeePayroll.DeductionAmount)}");

            modelBuilder.Entity<EmployeePayrollDeduction>().HasKey(e=> new {e.EmployeeDeductionId, e.EmployeePayrollId });
            modelBuilder.Entity<EmployeePayrollAllowance>().HasKey(e => new { e.EmployeeAllowanceId, e.EmployeePayrollId });



            modelBuilder.Entity<EmployeePayrollAllowance>().HasOne(e => e.EmployeeAllowance)
                .WithMany(a => a.PayrollAllowances).OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<EmployeeDeduction>().HasMany(e => e.PayrollDeductions)
                .WithOne(a => a.EmployeeDeduction).OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Position>().HasData(
                new Position() { Id = 1, Name = "Programmer" },

                new Position() { Id = 2, Name = "Software Engineer" });

            modelBuilder.Entity<Allowance>().HasData(new Allowance() { Id = 1, Description = "Job nature", DefaultAmount = 500 },
                 new Allowance() { Id = 2, Description = "Transportation", DefaultAmount = 100 }
                ) ;

            modelBuilder.Entity<Deduction>().HasData(new Deduction() { Id = 1, Description = "Health insurance", DefaultAmount = 75 },
               new Deduction() { Id = 2, Description = "Emp contribution", DefaultAmount = 300 }
              );


            modelBuilder.Entity<VWEmployee>().ToView("VW_Employees");

            
        }

       

      

    }
}
