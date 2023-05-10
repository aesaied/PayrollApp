using Microsoft.AspNetCore.Mvc;
using PayrollApp.Models;

namespace PayrollApp.Controllers
{
    public class TestApi : Controller
    {
        public async Task<IActionResult> Index()
        {
            //  Call  Api  from  Server side 


            HttpClient client =new HttpClient();
            client.BaseAddress =new Uri("https://localhost:44330/api/");




          var authResult = await  client.PostAsJsonAsync("auth/login", new UserLoginDto() { UserName = "admin", Password = "123@Qwe" });
          
            var  strToken=string.Empty;
           if(authResult.IsSuccessStatusCode)
            {


                var token =await authResult.Content.ReadFromJsonAsync<JWTTokenInfo>();

                strToken = token.Token;
            }

       

        

             var  client2 =new HttpClient();

            client2.BaseAddress = new Uri("https://localhost:44330/api/");


            client2.DefaultRequestHeaders.Add("Authorization", $"Bearer {strToken}");

            //  http://localhost:5266/api/departmentapi

            var reponse = await client2.GetAsync("departmentapi");


            if (reponse.IsSuccessStatusCode)
            {

                var  content =await  reponse.Content.ReadAsStringAsync();

                var deptList = await reponse.Content.ReadFromJsonAsync<List<DepartmentListInfo>>();

                return View(deptList);
            }


            return NotFound();




          
        }
    }
}
