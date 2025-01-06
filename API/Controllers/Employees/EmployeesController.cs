using API.Models;
using API.Models.DTOs;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Employees
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : Controller
    {
        private readonly EmployeesService _employeesService;

        public EmployeesController(EmployeesService employeesService)
        {
            _employeesService = employeesService;
        }

        // GET: api/Employee
        [HttpGet]
        public async Task<ActionResult<IEnumerable<REmployeeDTO>>> GetAllEmployees()
        {
            var employees = await _employeesService.GetAllEmployeesAsync();
            return Ok(employees);
        }

        // GET: api/Employee/5
        [HttpGet("{id}")]
        public async Task<ActionResult<REmployeeDTO>> GetEmployee(int id)
        {
            var employee = await _employeesService.GetEmployeeByIdAsync(id);
            if (employee == null)
                return NotFound();
            return Ok(employee);
        }

        // POST: api/Employee
        [HttpPost]
        public async Task<ActionResult<REmployeeDTO>> CreateEmployee(CUEmployeeDTO employee)
        {
            var createdEmployee = await _employeesService.CreateEmployeeAsync(employee);
            return CreatedAtAction(nameof(GetEmployee), new { id = createdEmployee.Id }, createdEmployee);
        }

        // PUT: api/Employee/5
        [HttpPut("{id}")]
        public async Task<ActionResult<REmployeeDTO>> UpdateEmployee(int id, Employee employee)
        {
            if (id != employee.Id)
                return BadRequest();

            var updatedEmployee = await _employeesService.UpdateEmployeeAsync(employee);
            if (updatedEmployee == null)
                return NotFound();

            return Ok(updatedEmployee);
        }

        // DELETE: api/Employee/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var success = await _employeesService.DeleteEmployeeAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
