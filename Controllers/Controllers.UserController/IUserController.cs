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
    }
}