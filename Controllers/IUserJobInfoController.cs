using DotnetAPI.DTOs;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers
{
    public interface IUserJobInfoController
    {
        IEnumerable<UserJobInfo> GetAllUserJobInfo();

        UserJobInfo GetSingleUserJobInfo(int userId);

        IActionResult EditUserJobInfo(UserJobInfo userJobInfo);

        IActionResult AddUserJobInfo(UserJobInfoToAddDTO userJobInfo);

        IActionResult DeleteUserJobInfo(int userId);
    }
}