using AutoMapper;
using PayrollApp.Data;
using PayrollApp.Models;

namespace PayrollApp
{
    public class CustomMapper: Profile
    {

        public CustomMapper() 
        {
        
            CreateMap<Employee,CreateOrEditEmployee>().ReverseMap();
            CreateMap<Department,DepartmentListInfo>().ReverseMap();
        
        }
    }
}
