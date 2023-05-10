using System.ComponentModel.DataAnnotations.Schema;

namespace PayrollApp.Data
{
    public class VWEmployee
    {
        public int Id { get; set; }
       
        public string FullName { get; set; }

        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public int PositionId { get; set; }

        public string PositionName { get; set; }
    }
}
