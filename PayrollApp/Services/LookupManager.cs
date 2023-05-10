using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PayrollApp.Data;

namespace PayrollApp.Services
{
    public class LookupManager : ILookupManager
    {
        private readonly PayrollDbContext _context;

        public LookupManager(PayrollDbContext context)
        {
            _context = context;
        }

        public async Task<SelectList> GetDepartmentsList()
        {
            
          
            return new SelectList(await _context.Departments.ToListAsync(),nameof(Department.Id),nameof(Department.Name));
        }

        public async Task< SelectList> GetPoitionList()
        {
            return new SelectList(await _context.Positions.ToListAsync(),nameof(Position.Id),nameof(Position.Name));

        }
    }
}
