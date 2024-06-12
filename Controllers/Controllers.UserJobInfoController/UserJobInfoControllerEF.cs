using DotnetAPI.Data;
using DotnetAPI.DTOs;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DotnetAPI.Controllers.UserJobInfoController
{
    [ApiController]
    [Route("[controller]")]
    public class UserJobInfoControllerEF(IConfiguration config) : ControllerBase, IUserJobInfoController
    {
        private readonly DataContextEF _contextEF = new(config);

        [HttpGet("GetUserJobInfo/{userId}")]
        public UserJobInfo GetUserJobInfo(int userId)
        {
            UserJobInfo? userJobInfo = _contextEF.UserJobInfo
                .Where(u => u.UserId == userId)
                .ToList()
                .FirstOrDefault() ?? throw new Exception("Failed loading user job info");

            return userJobInfo;
        }

        [HttpPost("AddUserJobInfo")]
        public IActionResult AddUserJobInfo(UserJobInfo userJobInfo)
        {
            _contextEF.UserJobInfo.Add(userJobInfo);

            if (_contextEF.SaveChanges() > 0)
            {
                return Ok();
            }

            throw new Exception("Failed to Add User Job Info");
        }

        [HttpPut("EditUserJobInfo")]
        public IActionResult EditUserJobInfo(UserJobInfo userJobInfo)
        {
            UserJobInfo? userJobInfoDb = _contextEF.UserJobInfo
                .Where(u => u.UserId == userJobInfo.UserId)
                .ToList()
                .FirstOrDefault() ?? throw new Exception("Failed loading user job info");

            userJobInfoDb.JobTitle = userJobInfo.JobTitle.IsNullOrEmpty()
                ? userJobInfoDb.JobTitle
                : userJobInfo.JobTitle;

            _contextEF.UserJobInfo.Update(userJobInfoDb);

            if (_contextEF.SaveChanges() > 0)
            {
                return Ok();
            }

            throw new Exception("Failed to Update User Job Info");
        }

        [HttpDelete("DeleteUserJobInfo/{userId}")]
        public IActionResult DeleteUserJobInfo(int userId)
        {
            UserJobInfo? userJobInfoDb = _contextEF.UserJobInfo
                 .Where(u => u.UserId == userId)
                 .ToList()
                 .FirstOrDefault() ?? throw new Exception("Failed loading user job info");

            _contextEF.UserJobInfo.Remove(userJobInfoDb);

            if (_contextEF.SaveChanges() > 0)
            {
                return Ok();
            }

            throw new Exception("Failed to Delete User Job Info");
        }
    }
}