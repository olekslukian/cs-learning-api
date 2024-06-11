using DotnetAPI.DTOs;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserSalaryController : ControllerBase, IUSerSalaryController
    {
        public IActionResult AddSalary(UserSalaryToAddDTO salary)
        {
            throw new NotImplementedException();
        }

        public IActionResult DeleteSalary(int userId)
        {
            throw new NotImplementedException();
        }

        public IActionResult EditSalary(UserSalary salary)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserSalary> GetSalaries()
        {
            throw new NotImplementedException();
        }

        public UserSalary GetSingleSalary(int userId)
        {
            throw new NotImplementedException();
        }
    }
}