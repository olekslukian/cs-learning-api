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
    public class UserControllerEF(IConfiguration config) : ControllerBase, IUserController
    {
        private readonly DataContextEF _contextEF = new(config);

        private readonly IMapper _mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<UserToAddDTO, User>();
        }));

        [HttpGet("GetUsers")]
        public IEnumerable<User> GetUsers()
        {
            /// Linter will suggest to use spread here, but original
            /// code looks like [_context.EF.Users.ToList<User>()]
            IEnumerable<User> users = [.. _contextEF.Users];

            return users;
        }

        [HttpGet("GetSingleUser/{userId}")]
        public User GetSingleUser(int userId)
        {
            User user = _contextEF.Users.Where(u => u.UserId == userId)
                .ToList()
                .FirstOrDefault() ?? throw new Exception("Failed to Get User");

            return user;
        }

        [HttpPut("EditUser")]
        public IActionResult EditUser(User user)
        {
            User userDb = _contextEF.Users.Where(u => u.UserId == user.UserId)
                            .ToList()
                            .FirstOrDefault() ?? throw new Exception("Failed to Get User");

            userDb.FirstName = user.FirstName.IsNullOrEmpty() ? userDb.FirstName : user.FirstName;
            userDb.LastName = user.LastName.IsNullOrEmpty() ? userDb.LastName : user.LastName;
            userDb.Email = user.Email.IsNullOrEmpty() ? userDb.Email : user.Email;
            userDb.Gender = user.Gender.IsNullOrEmpty() ? userDb.Gender : user.Gender;
            userDb.Active = user.Active;

            _contextEF.Users.Update(userDb);

            if (_contextEF.SaveChanges() > 0)
            {
                return Ok();
            }

            throw new Exception("Failed to Update User");

        }

        [HttpPost("AddUser")]
        public IActionResult AddUser(UserToAddDTO user)
        {
            User userDb = _mapper.Map<User>(user);

            _contextEF.Users.Add(userDb);

            if (_contextEF.SaveChanges() > 0)
            {
                return Ok();
            }

            throw new Exception("Failed to Add User");
        }

        [HttpDelete("DeleteUser/{userId}")]
        public IActionResult DeleteUser(int userId)
        {
            User userDb = _contextEF.Users.Where(u => u.UserId == userId)
                          .ToList()
                          .FirstOrDefault() ?? throw new Exception("Failed to Get User");

            _contextEF.Users.Remove(userDb);

            if (_contextEF.SaveChanges() > 0)
            {
                return Ok();
            }

            throw new Exception("Failed to Delete User");
        }

    }
}