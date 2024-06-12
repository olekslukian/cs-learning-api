using DotnetAPI.DTOs;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers.UserSalaryController
{
    public interface IUSerSalaryController
    {
        UserSalary GetUserSalary(int userId);
        IActionResult EditSalary(UserSalary salary);
        IActionResult AddSalary(UserSalary salary);
        IActionResult DeleteSalary(int userId);
    }
}