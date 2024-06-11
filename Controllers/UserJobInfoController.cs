using DotnetAPI.Controllers;
using DotnetAPI.DTOs;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UserJobInfoController : ControllerBase, IUserJobInfoController
    {
        public IActionResult AddUserJobInfo(UserJobInfoToAddDTO userJobInfo)
        {
            throw new NotImplementedException();
        }

        public IActionResult DeleteUserJobInfo(int userId)
        {
            throw new NotImplementedException();
        }

        public IActionResult EditUserJobInfo(UserJobInfo userJobInfo)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserJobInfo> GetAllUserJobInfo()
        {
            throw new NotImplementedException();
        }

        public UserJobInfo GetSingleUserJobInfo(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
