using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayrollApp.Consts;
using PayrollApp.Data;
using PayrollApp.Models;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace PayrollApp.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {

        private readonly PayrollDbContext _dbContext;
        private readonly ILogger<DepartmentController> _logger;
        public DepartmentController(PayrollDbContext payrollDbContext, ILogger<DepartmentController> logger )
        {
            _dbContext = payrollDbContext;

            _logger = logger;

        }

       

        public async Task<IActionResult> Index( string? keyword,string type)
        {
            _logger.LogInformation($" Index?keyword={keyword} , User={User.Identity.Name}");
           
          

            if (type == "reset")
            {
                keyword= null;
            }

            // select  *  from Deparment
            var lst = _dbContext.Departments.AsQueryable();

            if(!string.IsNullOrEmpty(keyword) )
            {

                // + where name like '%keyword%'
                lst = lst.Where(d => d.Name.Contains(keyword) );
                //lst = lst.Where(d => (d.Name.Contains(keyword)   || d.Id==10) && d.Id>9);

            }

            // select  Id , Name from  Departments where name like '%keyword%'
            var result =await lst.Select(d=> new DepartmentListInfo() 
            { Id=d.Id, Name=d.Name, EmployeeCount=d.Employees.Count() }).ToListAsync();



            //  to pass  keyword to the view 
            ViewBag.Keyword= keyword ?? "";


            return View(result);
        }



    
        [HttpGet]
        [Authorize(Roles = SystemRoles.Admins)]
        public IActionResult Create ()
        {

            _logger.LogInformation($" Create, User={User.Identity.Name}");

            return View();
        }


        [HttpPost]
        [Authorize(Roles = SystemRoles.Admins)]

        public async Task<IActionResult> Create(DepartmentListInfo info)
        {

            if (ModelState.IsValid)
            {
                if (IsValidToAddDeparment(info))
                {

                    Department dept = new Department() { Name = info.Name };

                    _dbContext.Departments.Add(dept);
                    await _dbContext.SaveChangesAsync();
                    TempData["Msg"] = $"Department '{info.Name}' added successfuly";

                    _logger.LogInformation($" Create success (Name={info.Name}), User={User.Identity.Name}");

                    return RedirectToAction(nameof(Index));

                }
                else
                {
                   
                    _logger.LogCritical($" Create Error (Name={info.Name}), User={User.Identity.Name}");

                    ModelState.AddModelError("", $"Department '{info.Name}'  already used by  another department!");
                }


            }

            return View(info);
        }


        [HttpGet]
        [Authorize(Roles = SystemRoles.Admins)]
        public async Task<IActionResult> Edit(int id)
        {

            var dept = await _dbContext.Departments.Where(d => d.Id == id)
                            .Select(d => new DepartmentListInfo() { Id = d.Id, Name = d.Name })
                            .FirstOrDefaultAsync();


            // 
            if (dept == null)
            {
                TempData["Msg"] = $"Department Id='{id}' not found!";

                return RedirectToAction(nameof(Index));
              //  return NotFound();

            }

            return View("EditDept",dept);
        }

        [HttpPost]
        [Authorize(Roles = SystemRoles.Admins)]
        public async Task<IActionResult> Edit(DepartmentListInfo info)
        {


            if (ModelState.IsValid)
            {
                if (IsValidToAddDeparment(info))
                {

                    // Department dept = new Department() {Id=info.Id, Name = info.Name };

                    var deptToUpdate =await _dbContext.Departments
                        .Where(de => de.Id == info.Id).FirstOrDefaultAsync();
                    if (deptToUpdate == null)
                    {
                        return NotFound();
                    }

                    deptToUpdate.Name = info.Name; //  Status changed to  updated
                   // _dbContext.Departments.Update(dept);
                    await _dbContext.SaveChangesAsync(); // Apply  changed to database
                    TempData["Msg"] = $"Department '{info.Name}' updated successfuly";
                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    ModelState.AddModelError("", $"Department '{info.Name}'  already used by  another department!");
                }


            }
            return View("EditDept", info);
        }


        [HttpGet]
        [Authorize(Roles = SystemRoles.Admins)]
        public async Task<IActionResult> Delete(int id)
        {

            var dept = await _dbContext.Departments.Where(d => d.Id == id)
                            .Select(d => new DepartmentListInfo() { Id = d.Id, Name = d.Name })
                            .FirstOrDefaultAsync();


            // 
            if (dept == null)
            {
                TempData["Msg"] = $"Department Id='{id}' not found!";

                return RedirectToAction(nameof(Index));
                //  return NotFound();

            }


            //  select  count(*) from  employees where departmentId= x;
            var deptEmployeesCount =await _dbContext.Employees
                .Where(e => e.DepartmentId == dept.Id).CountAsync();


            if(deptEmployeesCount> 0)
            {
                TempData["Msg"] = $"Department {dept.Name} cann't be deleted! hint: {deptEmployeesCount} employee(s)";
                return RedirectToAction(nameof(Index));
            }
            return View(dept);
        }


        [HttpPost]
        [Authorize(Roles = SystemRoles.Admins)]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var dept = await _dbContext.Departments.Where(d => d.Id == id).FirstOrDefaultAsync();
                       // 
            if (dept == null)
            {
                TempData["Msg"] = $"Department Id='{id}' not found!";

                return RedirectToAction(nameof(Index));
                //  return NotFound();

            }
            //  select  count(*) from  employees where departmentId= x;
            var deptEmployeesCount = await _dbContext.Employees
                .Where(e => e.DepartmentId == dept.Id).CountAsync();


            if (deptEmployeesCount > 0)
            {
                TempData["Msg"] = $"Unable to  delete Department {dept.Name}! hint: {deptEmployeesCount} employee(s)";
                return RedirectToAction(nameof(Index));
            }

            _dbContext.Departments.Remove(dept);
            await _dbContext.SaveChangesAsync();
            TempData["Msg"] = $"Department '{dept.Name}' deleted successfully!";

            return RedirectToAction(nameof(Index));


        }



        private bool IsValidToAddDeparment(DepartmentListInfo info)
        {

            //

            var  deptExists = _dbContext.Departments.Any(d=>d.Name==info.Name && d.Id!=info.Id);

            return !deptExists;
        }

    }
}
