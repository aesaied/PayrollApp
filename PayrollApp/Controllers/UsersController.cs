using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayrollApp.Consts;
using PayrollApp.Data;
using PayrollApp.Models;

namespace PayrollApp.Controllers
{
    [Authorize(Roles = SystemRoles.Admins)]
    public class UsersController : Controller
    {

        private readonly UserManager<AppUser> _userManager;

        private readonly PayrollDbContext _payrollDbContext;
        public UsersController( UserManager<AppUser> userManager, PayrollDbContext payrollDbContext) { 
        
        _userManager= userManager;
            _payrollDbContext= payrollDbContext;
        }
        public async Task<IActionResult> Index()
        {

            var users =  await _userManager.Users.Select(u => 
            new UserListInfo{ UserName=u.UserName, Email=u.Email, Id=u.Id }).ToListAsync();
            return View(users);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateUserInfo input)
        {
            if(ModelState.IsValid)
            {

                var user = new AppUser() { UserName = input.UserName, Email = input.EmailAddress };
                await _userManager.CreateAsync(user,input.Password);

                await _userManager.AddToRoleAsync(user, SystemRoles.Users);
                return RedirectToAction(nameof(Index));
            }


            return View(input);
        }
    }
}
