using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using PayrollApp.Data;
using PayrollApp.Models;
using System.Net.Mime;

namespace PayrollApp.Controllers
{
    [Route("api/[controller]")]

    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ApiController]
    [Authorize]

    public class DepartmentApiController : ControllerBase
    {
        private readonly PayrollDbContext _context;
        private readonly IMapper _mapper;
        public DepartmentApiController( PayrollDbContext context, IMapper mapper)
        {
            _context= context;
            _mapper= mapper;
        }



        // Get : /api/DepartmentApi/

        [HttpGet]
        //[Route("")]
        //[Route("GetAll")]

        [MapToApiVersion("1.0")]
        // Spefic Type -> return type
        public async Task<List<DepartmentListInfo>> GetAll()
        {

            var  depts =await  _context.Departments.ToListAsync();

            return _mapper.Map<List<DepartmentListInfo>>(depts);

        }

        [HttpGet]
        [Route("")]
        [Route("GetAll")]

        [MapToApiVersion("2.0")]
        // Spefic Type -> return type
        public async Task<List<DepartmentListInfo>> GetAllV2()
        {

            var depts = await _context.Departments.OrderBy(d=>d.Name).ToListAsync();

            return _mapper.Map<List<DepartmentListInfo>>(depts);

        }


        [HttpPost]
        //IActionResult -> return type
        [DefaultStatusCode(200)]
        public async  Task<IActionResult> Create( DepartmentListInfo info)
        {

            if(ModelState.IsValid)
            {

                Department dept = _mapper.Map<Department>(info);
                _context.Departments.Add(dept);
                await _context.SaveChangesAsync();

                info.Id=dept.Id;

                var location = Url.Action(nameof(GetById), new { id = dept.Id }) ?? $"/{dept.Id}";
                return Ok(Results.Created(location, info));

               // return Created();

            }

            return BadRequest(ModelState);
        }


        [HttpGet("{id}")]
        // ActionResult<T> -> return type
        public async Task<ActionResult<DepartmentListInfo>> GetById(int id)
        {
            var  dept =await _context.Departments.FirstOrDefaultAsync(de => de.Id == id);

            if(dept != null)
            {
                return Ok(_mapper.Map<DepartmentListInfo>(dept));
            }


            return NotFound();
        }

        [HttpDelete("{id}")]
        
        public async Task<ActionResult> DeleteById(int id)
        {
            var  dept  =  new Department { Id = id};

            _context.Departments.Remove(dept);

            await _context.SaveChangesAsync();

            return Ok();
        }


        [HttpDelete("DeleteByName/{name}")]
        public async Task<ActionResult> DeleteByName(string name)
        {
            var dept =await _context.Departments.FirstOrDefaultAsync(d => d.Name == name);

            if(dept == null)
            {
                return NotFound();
            }

            _context.Departments.Remove(dept);

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{id}")] // Put: /DepartmentApi/2
        public async Task<ActionResult> Update(int id, DepartmentListInfo dept)
        {

            var oldDept = await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);

            if (oldDept == null)
                return NotFound();


             _mapper.Map(dept,oldDept);
            await _context.SaveChangesAsync();
            return Ok();


            //var  deptToUpdate=  _mapper.Map<Department>(dept);

            //_context.Departments.Update(deptToUpdate);

            //await _context.SaveChangesAsync();

            //return Ok();




        }


        [HttpGet("CustomGetById/{id}")]
      
        public async Task<APiResult<DepartmentListInfo>> CustomGetById(int id)
        {
            var dept = await _context.Departments.FirstOrDefaultAsync(de => de.Id == id);

            if (dept != null)
            {
                return new APiResult<DepartmentListInfo> 
                { Data= _mapper.Map<DepartmentListInfo>(dept), Success=true };
            }


            return new APiResult<DepartmentListInfo>() { Success = false, ErrorMsg = "Data not found!!" };
        }







    }
}
