using DotnetAPI.DTOs;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers.UserJobInfoController
{
    public interface IUserJobInfoController
    {

        UserJobInfo GetUserJobInfo(int userId);

        IActionResult EditUserJobInfo(UserJobInfo userJobInfo);

        IActionResult AddUserJobInfo(UserJobInfo userJobInfo);

        IActionResult DeleteUserJobInfo(int userId);
    }
}