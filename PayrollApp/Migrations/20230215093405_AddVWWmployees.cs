using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PayrollApp.Migrations
{
    /// <inheritdoc />
    public partial class AddVWWmployees : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

           
            migrationBuilder.Sql(@"create or alter view VW_Employees as 
select e.Id, e.IDNo, e.FullName, e.DepartmentId , d.Name as DepartmentName, 
e.PositionId, p.Name as PositionName from Employees e join Positions p  on e.PositionId=p.Id
join Departments d on e.DepartmentId=d.Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("drop view VW_Employees");
        }
    }
}
