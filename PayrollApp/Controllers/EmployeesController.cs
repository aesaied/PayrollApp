using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayrollApp.Data;
using PayrollApp.Models;
using PayrollApp.Services;

namespace PayrollApp.Controllers
{
    public class EmployeesController : Controller
    {

        private readonly PayrollDbContext _context;
        private readonly ILookupManager _lookupManager;
        private readonly ILogger<EmployeesController> _logger;

        private readonly IMapper _mapper;


        public EmployeesController(PayrollDbContext context, ILogger<EmployeesController> logger, ILookupManager lookupManager, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _lookupManager = lookupManager;
            _mapper = mapper;

        }
        public async Task<IActionResult> Index()
        {


            _logger.LogInformation("Index ");
            var  emps = await _context.VWEmployees.ToListAsync();


            return View(emps);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await  FillLookups();

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateOrEditEmployee e)
        {
            if(ModelState.IsValid)
            {

                var  emp  = _mapper.Map<Employee>(e);

                _context.Add(emp);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
                //Employee emp= new Employee() { IDNo=e.IDNo, BasicSalary=e.BasicSalary,  DepartmentId=e.DepartmentId, EmpNo=e.EmpNo, FirstName=e.FirstName, Gender=e.Gender,   };
            }


            await FillLookups();
            return View();
        }


        private async Task FillLookups()
        {
            ViewBag.Departments =await _lookupManager.GetDepartmentsList();
            ViewBag.Positions = await _lookupManager.GetPoitionList();

        }
    }
}
