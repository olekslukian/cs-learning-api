using System.Data;
using System.Security.Cryptography;
using System.Text;
using DotnetAPI.Data;
using DotnetAPI.DTOs;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace DotnetAPI.Controllers
{
    public class AuthController(IConfiguration config) : ControllerBase, IAuthController
    {
        private readonly IConfiguration _config = config;
        private readonly DataContextDapper _dapper = new(config);

        [HttpPost("Register")]
        public IActionResult Register(UserForRegistrationDTO userForRegistration)
        {
            if (userForRegistration.Password != userForRegistration.PasswordConfirmation)
            {
                throw new Exception("Passwords do not match");
            }

            string sqlCheckUserExists = @"
                SELECT 
                    [Email]
                FROM TutorialAppSchema.Auth 
                    WHERE Email = '" + userForRegistration.Email + "'";

            IEnumerable<string> existingUsers = _dapper.LoadData<string>(sqlCheckUserExists);

            if (existingUsers.IsNullOrEmpty())
            {
                byte[] passwordSalt = new byte[128 / 8];

                using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                {
                    rng.GetNonZeroBytes(passwordSalt);
                }

                string passwordSaltPlusString = _config
                    .GetSection("AppSettings:PasswordKey")
                    .Value + Convert.ToBase64String(passwordSalt);

                byte[] passwordHash = KeyDerivation.Pbkdf2(
                    password: userForRegistration.Password,
                    salt: Encoding.ASCII.GetBytes(passwordSaltPlusString),
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8
                    );

                string sqlAddAuth = @" 
                INSERT INTO TutorialAppSchema.Auth (
                    [Email], 
                    [PasswordHash], 
                    [PasswordSalt]
                    ) VALUES (
                        '" + userForRegistration.Email +
                        "', @PaswordHash, @PasswordSalt)";

                List<SqlParameter> parameters = [];

                SqlParameter passwordHashParameter = new("@PasswordHash", SqlDbType.VarBinary)
                {
                    Value = passwordHash
                };

                SqlParameter passwordSaltParameter = new("@PasswordSalt", SqlDbType.VarBinary)
                {
                    Value = passwordSalt
                };

                parameters.Add(passwordHashParameter);
                parameters.Add(passwordSaltParameter);

                if (_dapper.ExecuteSqlWithParameters(sqlAddAuth, parameters))
                {
                    return Ok();
                }

                throw new Exception("Failed to register user");
            }

            throw new Exception("User already exists");

        }

        [HttpPost("LogIn")]
        public IActionResult LogIn(UserForLoginDTO userForLogin)
        {
            return Ok();
        }
    }
}