using EmployeeAdminPortal.Data;
using EmployeeAdminPortal.Models;
using EmployeeAdminPortal.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAdminPortal.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class EmployesController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public EmployesController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        [HttpGet("GetAllEmployees")]
        public IActionResult GetAllEmployees()
        {
            var allEmployees = dbContext.Employees.ToList();

            return Ok(allEmployees);
        }

        [HttpGet("GetEmployeeById")]
        public IActionResult GetEmployeeById(Guid id) {
            var employee = dbContext.Employees.Find(id);

            if(employee is null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [HttpPost("AddEmployee")]
        public IActionResult AddEmployee(AddEmployeeDto addEmployeeDto)
        {
            var employee = new Employee() { 
                Name = addEmployeeDto.Name,
                Email = addEmployeeDto.Email,
                Phone = addEmployeeDto.Phone,
                Salary = addEmployeeDto.Salary
            };

            var i = employee.Id;
            Console.WriteLine(i);
            dbContext.Employees.Add(employee);
            dbContext.SaveChanges();

            return Ok(employee);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdateEmployee(Guid id, UpdateEmployeeDto updateEmployeeDto)
        {
            var employee = dbContext.Employees.Find(id);
            if (employee is null)
            {
                return NotFound();
            }

            if(!string.IsNullOrEmpty(updateEmployeeDto.Name))
            {
                employee.Name = updateEmployeeDto.Name;
            }

            if (!string.IsNullOrEmpty(updateEmployeeDto.Email))
            {
                employee.Email = updateEmployeeDto.Email;
            }

            if (!string.IsNullOrEmpty(updateEmployeeDto.Phone))
            {
                employee.Phone = updateEmployeeDto.Phone;
            }
            if (updateEmployeeDto.Salary.HasValue && updateEmployeeDto.Salary != employee.Salary)
            {
                employee.Salary = updateEmployeeDto.Salary.Value;
            }
            dbContext.Employees.Update(employee);
            dbContext.SaveChanges();

            return Ok(employee);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeleteEmployee(Guid id) {
            var employee = dbContext.Employees.Find(id);
            if (employee is null) { return NotFound(); }
            dbContext.Employees.Remove(employee);
            dbContext.SaveChanges();
            return Ok(employee);
        }
    }
}
