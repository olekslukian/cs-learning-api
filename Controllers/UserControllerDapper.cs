using Microsoft.AspNetCore.Mvc;
using DotnetAPI.Models;
using DotnetAPI.Data;
using DotnetAPI.DTOs;
using Microsoft.Data.SqlClient;

namespace DotnetAPI.Controllers.UserController
{
    [ApiController]
    [Route("[controller]")]
    public class UserControllerDapper(IConfiguration config) : ControllerBase, IUserController
    {
        private readonly DataContextDapper _dapper = new(config);

        [HttpGet("GetUsers")]
        public IEnumerable<User> GetUsers()
        {
            string sql = @"
                SELECT [UserId],
                    [FirstName],
                    [LastName],
                    [Email],
                    [Gender],
                    [Active] 
                FROM TutorialAppSchema.Users;";

            IEnumerable<User> users = _dapper.LoadData<User>(sql);

            return users;
        }

        [HttpGet("GetSingleUser/{userId}")]
        public User GetSingleUser(int userId)
        {
            string sql = @$"
                SELECT [UserId],
                    [FirstName],
                    [LastName],
                    [Email],
                    [Gender],
                    [Active] 
                FROM TutorialAppSchema.Users WHERE [UserId] = @userId";

            SqlParameter[] parameters = [new("@userId", userId)];

            User user = _dapper.LoadDataSingle<User>(sql);

            return user;
        }

        [HttpPut("EditUser")]
        public IActionResult EditUser(User user)
        {
            string sql = @$"
                  UPDATE TutorialAppSchema.Users 
                    SET [FirstName] = '" + user.FirstName +
                        "', [LastName] = '" + user.LastName +
                        "', [Email] = '" + user.Email +
                        "', [Gender] = '" + user.Gender +
                        "', [Active] = '" + user.Active +
                    "' WHERE [UserId] = " + user.UserId;

            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }

            throw new Exception("Failed to update user");

        }

        [HttpPost("AddUser")]
        public IActionResult AddUser(UserToAddDTO user)
        {

            string sql = @"
            INSERT INTO TutorialAppSchema.Users(
                [FirstName],
                [LastName],
                [Email],
                [Gender],
                [Active]
            ) VALUES (
                '" + user.FirstName +
                "', '" + user.LastName +
                "', '" + user.Email +
                "', '" + user.Gender +
                "', '" + user.Active +
            "')";

            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }

            throw new Exception("Failed to add new user");
        }

        [HttpDelete("DeleteUser")]
        public IActionResult DeleteUser(int userId)
        {
            string sql = $"DELETE FROM TutorialAppSchema.Users WHERE [UserId] = {userId}";

            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }

            throw new Exception("Failed to delete user");
        }

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



