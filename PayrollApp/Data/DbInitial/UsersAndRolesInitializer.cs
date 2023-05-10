using Microsoft.AspNetCore.Identity;
using PayrollApp.Consts;

namespace PayrollApp.Data.DbInitial
{
    public class UsersAndRolesInitializer
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public UsersAndRolesInitializer(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task Initialize()
        {
            //_roleManager.role

            if (!await _roleManager.RoleExistsAsync(SystemRoles.Admins))
            {
                await _roleManager.CreateAsync(new AppRole() { Name=SystemRoles.Admins  });
            }

            if (!await _roleManager.RoleExistsAsync(SystemRoles.Users))
            {
                await _roleManager.CreateAsync(new AppRole() { Name = SystemRoles.Users });
            }


            // Admin User
            string admnUser = "Admin";
            if(await _userManager.FindByNameAsync(admnUser)==null)
            {
                var user = new AppUser() { UserName = admnUser, Email = "atallah.esaied@gmail.com" };
                await _userManager.CreateAsync(user);

            }

            var aUser = await _userManager.FindByNameAsync(admnUser);
            if(!await _userManager.HasPasswordAsync(aUser))
            {
                await _userManager.AddPasswordAsync(aUser, "123@Qwe");

            }

            if(!await _userManager.IsInRoleAsync(aUser, SystemRoles.Admins))
            {
                await _userManager.AddToRoleAsync(aUser, SystemRoles.Admins);

            }

        }


    }
}
