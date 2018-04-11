using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingSystem.Data;
using TrainingSystem.Models;
using TrainingSystem.ViewModels;

namespace TrainingSystem.Controllers
{
    [Produces("application/json")]
    [Route("api/Employees")]
    public class EmployeesController : Controller
    {
        private readonly TrainingSystemContext _context;
        private readonly UserManager<AppUser> _userManager;

        public EmployeesController(TrainingSystemContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Employees
        [HttpGet]
        public IEnumerable<Employee> GetEmployees()
        {
            return _context.Employee.Include(e => e.AppUser);
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee([FromRoute] int id)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var employee = await _context.Employee
                .Include(e => e.AppUser)
                .SingleOrDefaultAsync(e => e.EmployeeId == id);

            if (employee == null) {
                return NotFound();
            }

            return Ok(employee);
        }

        // POST: api/Employees
        [HttpPost]
        public async Task<IActionResult> PostEmployee([FromBody] RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var appUser = await _userManager.FindByEmailAsync(registerViewModel.Email);

            if (appUser != null) {
                ModelState.AddModelError("email", "User already exists");
                return BadRequest(ModelState);
            }

            appUser = new AppUser {
                UserName = registerViewModel.Email,
                FirstName = registerViewModel.FirstName,
                LastName = registerViewModel.LastName,
                Email = registerViewModel.Email
            };

            var result = await _userManager.CreateAsync(appUser, registerViewModel.Password);

            if (!result.Succeeded) {
                foreach(var error in result.Errors) {
                    ModelState.AddModelError(error.Code, error.Description);
                    return BadRequest(ModelState);
                }
            }

            await _userManager.AddToRoleAsync(appUser, "Employee");

            var employee = new Employee {
                Occupation = registerViewModel.Occupation,
                AppUser = appUser
            };

            _context.Employee.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployee", new { id = employee.EmployeeId }, employee);
        }
        
        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.EmployeeId == id);
        }
    }
}