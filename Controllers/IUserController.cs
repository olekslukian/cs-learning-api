using DotnetAPI.DTOs;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers.UserController
{
    public interface IUserController
    {
        IEnumerable<User> GetUsers();
        User GetSingleUser(int userId);
        IActionResult EditUser(User user);
        IActionResult AddUser(UserToAddDTO user);
        IActionResult DeleteUser(int userId);
        UserJobInfo GetUserJobInfo(int userId);
        IActionResult AddUserJobInfo(UserJobInfo userJobInfo);
        IActionResult EditUserJobInfo(UserJobInfo userJobInfo);
        IActionResult DeleteUserJobInfo(int userId);
        UserSalary GetUserSalary(int userId);
        IActionResult AddSalary(UserSalary salary);
        IActionResult EditSalary(UserSalary salary);
        IActionResult DeleteSalary(int userId);
    }
}