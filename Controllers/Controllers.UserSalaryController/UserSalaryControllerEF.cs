using DotnetAPI.Data;
using DotnetAPI.DTOs;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers.UserSalaryController

{
    [ApiController]
    [Route("[controller]")]
    public class UserSalaryControllerEF(IConfiguration config) : ControllerBase, IUSerSalaryController
    {
        private readonly DataContextEF _contextEF = new(config);

        [HttpGet("GetUserSalary/{userId}")]
        public UserSalary GetUserSalary(int userId)
        {
            UserSalary? userSalary = _contextEF.UserSalary
               .Where(u => u.UserId == userId)
               .ToList()
               .FirstOrDefault() ?? throw new Exception("Failed loading user salary");

            return userSalary;
        }

        [HttpPost("AddSalary")]
        public IActionResult AddSalary(UserSalary salary)
        {
            _contextEF.UserSalary.Add(salary);

            if (_contextEF.SaveChanges() > 0)
            {
                return Ok();
            }

            throw new Exception("Failed to Add User Salary");
        }

        [HttpPut("EditSalary")]
        public IActionResult EditSalary(UserSalary salary)
        {
            UserSalary? userSalaryDb = _contextEF.UserSalary
                .Where(u => u.UserId == salary.UserId)
                .ToList()
                .FirstOrDefault() ?? throw new Exception("Failed loading user salary");

            userSalaryDb.Salary = salary.Salary;

            _contextEF.UserSalary.Update(userSalaryDb);

            if (_contextEF.SaveChanges() > 0)
            {
                return Ok();
            }

            throw new Exception("Failed to Update User Salary");
        }

        [HttpDelete("DeleteSalary/{userId}")]
        public IActionResult DeleteSalary(int userId)
        {
            UserSalary? userSalaryDb = _contextEF.UserSalary
                 .Where(u => u.UserId == userId)
                 .ToList()
                 .FirstOrDefault() ?? throw new Exception("Failed loading user salary");

            _contextEF.UserSalary.Remove(userSalaryDb);

            if (_contextEF.SaveChanges() > 0)
            {
                return Ok();
            }

            throw new Exception("Failed to Delete User Salary");
        }
    }
}