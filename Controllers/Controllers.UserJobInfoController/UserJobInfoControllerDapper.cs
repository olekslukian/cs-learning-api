using DotnetAPI.Controllers;
using DotnetAPI.Data;
using DotnetAPI.DTOs;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers.UserJobInfoController
{

    [ApiController]
    [Route("[controller]")]
    public class UserJobInfoControllerDapper(IConfiguration config) : ControllerBase, IUserJobInfoController
    {

        private readonly DataContextDapper _dapper = new(config);


        [HttpGet("GetUserJobInfo/{userId}")]
        public UserJobInfo GetUserJobInfo(int userId)
        {
            string sql = @"
                SELECT [UserId],
                    [JobTitle],
                    [Department] FROM TutorialAppSchema.UserJobInfo
                WHERE [UserId] = " + userId;

            UserJobInfo result = _dapper.LoadDataSingle<UserJobInfo>(sql);

            return result;
        }

        [HttpPost("AddUserJobInfo")]
        public IActionResult AddUserJobInfo(UserJobInfo userJobInfo)
        {
            string sql = @"
            INSERT INTO TutorialAppSchema.UserJobInfo(
                [UserId],
                [JobTitle],
                [Department]
            ) VALUES (
                '" + userJobInfo.UserId +
                "', '" + userJobInfo.JobTitle +
                "', '" + userJobInfo.Department +
            "')";

            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }

            throw new Exception("Failed to add user job info");
        }

        [HttpPut("EditUserJobInfo")]
        public IActionResult EditUserJobInfo(UserJobInfo userJobInfo)
        {
            string sql = @$"
                  UPDATE TutorialAppSchema.UserJobInfo
                    SET [JobTitle] = '" + userJobInfo.JobTitle +
                     "', [Department] = '" + userJobInfo.Department +
                 "' WHERE [UserId] = " + userJobInfo.UserId;

            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }

            throw new Exception("Failed to update user job info");
        }

        [HttpDelete("DeleteUserJobInfo/{userId}")]
        public IActionResult DeleteUserJobInfo(int userId)
        {
            string sql = $"DELETE FROM TutorialAppSchema.UserJobInfo WHERE [UserId] = {userId}";

            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }

            throw new Exception("Failed to delete user job info");
        }

    }
}
