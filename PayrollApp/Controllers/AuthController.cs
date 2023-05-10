using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PayrollApp.Data;
using PayrollApp.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PayrollApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto input)
        {
            
            var user = await _userManager.FindByNameAsync(input.UserName);

            if(user == null)
            {
                return Unauthorized();
            }


            var isValidPass = await _userManager.CheckPasswordAsync(user, input.Password);

            if (!isValidPass)
            {
                return Unauthorized();
            }


            //  if valid return  token

            var jwtConfig = _configuration.GetSection("JWT");
            var key = Encoding.UTF8.GetBytes(jwtConfig["secret"]);
            var secret = new SymmetricSecurityKey(key);

            var signInCredintial=new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);



            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName)
        };
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }



            //var jwtSettings = _configuration.GetSection("JwtConfig");
            var tokenOptions = new JwtSecurityToken
            (
            issuer: jwtConfig["validIssuer"],
            audience: jwtConfig["validAudience"],
            claims: claims,
            expires: DateTime.Now.AddDays(Convert.ToDouble(jwtConfig["expiresIn"])),
            signingCredentials: signInCredintial
      
            );


          

           var  tokenStr=   new JwtSecurityTokenHandler().WriteToken(tokenOptions);
          
            return Ok(new { Token = tokenStr });




        }
    }
}
