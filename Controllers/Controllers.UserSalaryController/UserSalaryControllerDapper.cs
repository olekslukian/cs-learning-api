using DotnetAPI.Data;
using DotnetAPI.DTOs;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers.UserSalaryController
{
    [ApiController]
    [Route("[controller]")]
    public class UserSalaryControllerDapper(IConfiguration config) : ControllerBase, IUSerSalaryController
    {
        private readonly DataContextDapper _dapper = new(config);

        [HttpGet("GetUserSalary/{userId}")]
        public UserSalary GetUserSalary(int userId)
        {
            string sql = @"
                SELECT [UserId],
                    [Salary],
                    [AvgSalary] FROM TutorialAppSchema.UserSalary
                WHERE [UserId] = " + userId;

            UserSalary result = _dapper.LoadDataSingle<UserSalary>(sql);

            return result;
        }

        [HttpPost("AddSalary")]
        public IActionResult AddSalary(UserSalary salary)
        {
            string sql = @"
            INSERT INTO TutorialAppSchema.UserSalary(
                [UserId],
                [Salary]
            ) VALUES (
                '" + salary.UserId +
                "', '" + salary.Salary +
            "')";

            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }

            throw new Exception("Failed to add user salary");
        }

        [HttpPut("EditSalary")]
        public IActionResult EditSalary(UserSalary salary)
        {
            string sql = @$"
                  UPDATE TutorialAppSchema.UserSalary
                    SET [Salary] = '" + salary.Salary +
              "' WHERE [UserId] = " + salary.UserId;

            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }

            throw new Exception("Failed to update user salary");
        }

        [HttpDelete("DeleteSalary/{userId}")]
        public IActionResult DeleteSalary(int userId)
        {
            string sql = $"DELETE FROM TutorialAppSchema.UserSalary WHERE [UserId] = {userId}";

            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }

            throw new Exception("Failed to delete user salary");
        }
    }
}