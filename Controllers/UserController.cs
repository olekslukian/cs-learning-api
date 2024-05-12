using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(IConfiguration config) : ControllerBase
    {
        private readonly DataContextDapper _dapper = new(config);

        [HttpGet("TestConnection")]
        public DateTime TestConnection()
        {
            return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
        }

        [HttpGet("GetUsers/{testValue}")]
        public string[] GetUsers(string? testValue)
        {
            string[] responseArray = ["Test1", "Test2", "Test3"];

            if (testValue != null)
            {
                return [.. responseArray, testValue];
            }

            return responseArray;
        }
    }
}



