using DotnetAPI.DTOs;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers
{
    public interface IUSerSalaryController
    {
        IEnumerable<UserSalary> GetSalaries();
        UserSalary GetSingleSalary(int userId);
        IActionResult EditSalary(UserSalary salary);
        IActionResult AddSalary(UserSalaryToAddDTO salary);
        IActionResult DeleteSalary(int userId);
    }
}