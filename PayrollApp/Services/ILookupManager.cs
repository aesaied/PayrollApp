using Microsoft.AspNetCore.Mvc.Rendering;

namespace PayrollApp.Services
{
    public interface ILookupManager
    {

        Task<SelectList> GetDepartmentsList();

        Task<SelectList> GetPoitionList();

    }
}
