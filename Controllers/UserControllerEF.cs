using AutoMapper;
using DotnetAPI.Data;
using DotnetAPI.DTOs;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DotnetAPI.Controllers.UserController
{


    /// Entity Framework looks simplier, than Dapper. But under the hood it can be more complex.
    /// In this case it will execute on SQL server slower, which will require additional calibration.
    /// In more big projects suggested to use dapper, because it is more flexible and faster.
    [ApiController]
    [Route("[controller]")]
    public class UserControllerEF(IUserRepository userRepository) : ControllerBase, IUserController
    {

        private readonly IUserRepository _userRepository = userRepository;

        private readonly IMapper _mapper = new Mapper(new MapperConfiguration(cfg =>

       {
           cfg.CreateMap<UserToAddDTO, User>();
       }));

        [HttpGet("GetUsers")]
        public IEnumerable<User> GetUsers()
        {
            IEnumerable<User> users = _userRepository.GetUsers();

            return users;
        }

        [HttpGet("GetSingleUser/{userId}")]
        public User GetSingleUser(int userId)
        {
            User user = _userRepository.GetSingleUser(userId);

            return user;
        }

        [HttpPut("EditUser")]
        public IActionResult EditUser(User user)
        {
            User userDb = _userRepository.GetSingleUser(user.UserId);

            userDb.FirstName = user.FirstName.IsNullOrEmpty() ? userDb.FirstName : user.FirstName;
            userDb.LastName = user.LastName.IsNullOrEmpty() ? userDb.LastName : user.LastName;
            userDb.Email = user.Email.IsNullOrEmpty() ? userDb.Email : user.Email;
            userDb.Gender = user.Gender.IsNullOrEmpty() ? userDb.Gender : user.Gender;
            userDb.Active = user.Active;

            _userRepository.UpdateEntity<User>(userDb);

            if (_userRepository.SaveChanges())
            {
                return Ok();
            }

            throw new Exception("Failed to Edit User");

        }

        [HttpPost("AddUser")]
        public IActionResult AddUser(UserToAddDTO user)
        {
            User userDb = _mapper.Map<User>(user);

            _userRepository.AddEntity<User>(userDb);

            if (_userRepository.SaveChanges())
            {
                return Ok();
            }

            throw new Exception("Failed to Add User");
        }

        [HttpDelete("DeleteUser/{userId}")]
        public IActionResult DeleteUser(int userId)
        {
            User userDb = _userRepository.GetSingleUser(userId);

            _userRepository.RemoveEntity<User>(userDb);

            if (_userRepository.SaveChanges())
            {
                return Ok();
            }

            throw new Exception("Failed to Delete User");
        }

        [HttpGet("GetUserJobInfo/{userId}")]
        public UserJobInfo GetUserJobInfo(int userId)
        {
            UserJobInfo userJobInfo = _userRepository.GetUserJobInfo(userId);

            return userJobInfo;
        }

        [HttpPost("AddUserJobInfo")]
        public IActionResult AddUserJobInfo(UserJobInfo userJobInfo)
        {
            _userRepository.AddEntity<UserJobInfo>(userJobInfo);

            if (_userRepository.SaveChanges())
            {
                return Ok();
            }

            throw new Exception("Failed to Add User Job Info");
        }

        [HttpPut("EditUserJobInfo")]
        public IActionResult EditUserJobInfo(UserJobInfo userJobInfo)
        {
            UserJobInfo? userJobInfoDb = _userRepository.GetUserJobInfo(userJobInfo.UserId);

            userJobInfoDb.JobTitle = userJobInfo.JobTitle.IsNullOrEmpty()
                ? userJobInfoDb.JobTitle
                : userJobInfo.JobTitle;

            _userRepository.UpdateEntity<UserJobInfo>(userJobInfoDb);

            if (_userRepository.SaveChanges())
            {
                return Ok();
            }

            throw new Exception("Failed to Update User Job Info");
        }

        [HttpDelete("DeleteUserJobInfo/{userId}")]
        public IActionResult DeleteUserJobInfo(int userId)
        {
            UserJobInfo? userJobInfoDb = _userRepository.GetUserJobInfo(userId);

            _userRepository.RemoveEntity<UserJobInfo>(userJobInfoDb);

            if (_userRepository.SaveChanges())
            {
                return Ok();
            }

            throw new Exception("Failed to Delete User Job Info");
        }

        [HttpGet("GetUserSalary/{userId}")]
        public UserSalary GetUserSalary(int userId)
        {
            UserSalary? userSalary = _userRepository.GetUserSalary(userId);

            return userSalary;
        }

        [HttpPost("AddSalary")]
        public IActionResult AddSalary(UserSalary salary)
        {
            _userRepository.AddEntity<UserSalary>(salary);

            if (_userRepository.SaveChanges())
            {
                return Ok();
            }

            throw new Exception("Failed to Add User Salary");
        }

        [HttpPut("EditSalary")]
        public IActionResult EditSalary(UserSalary salary)
        {
            UserSalary? userSalaryDb = _userRepository.GetUserSalary(salary.UserId);

            userSalaryDb.Salary = salary.Salary;

            _userRepository.UpdateEntity<UserSalary>(userSalaryDb);

            if (_userRepository.SaveChanges())
            {
                return Ok();
            }

            throw new Exception("Failed to Update User Salary");
        }

        [HttpDelete("DeleteSalary/{userId}")]
        public IActionResult DeleteSalary(int userId)
        {
            UserSalary? userSalaryDb = _userRepository.GetUserSalary(userId);

            _userRepository.RemoveEntity<UserSalary>(userSalaryDb);

            if (_userRepository.SaveChanges())
            {
                return Ok();
            }

            throw new Exception("Failed to Delete User Salary");
        }

    }
}