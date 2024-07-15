using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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

                byte[] passwordHash = GetPasswordHash(userForRegistration.Password, passwordSalt);

                string sqlAddAuth = @" 
                INSERT INTO TutorialAppSchema.Auth (
                    [Email], 
                    [PasswordHash], 
                    [PasswordSalt]
                    ) VALUES (
                        '" + userForRegistration.Email +
                        "', @PasswordHash, @PasswordSalt)";

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
                    string sqlAddUser = @"
                        INSERT INTO TutorialAppSchema.Users(
                            [FirstName],
                            [LastName],
                            [Email],
                            [Gender],
                            [Active]
                        ) VALUES (
                            '" + userForRegistration.FirstName +
                        "', '" + userForRegistration.LastName +
                        "', '" + userForRegistration.Email +
                        "', '" + userForRegistration.Gender +
                        "', 1)";

                    if (_dapper.ExecuteSql(sqlAddUser))
                    {
                        return Ok();
                    }

                    throw new Exception("Failed to add user");
                }

                throw new Exception("Failed to register user");
            }

            throw new Exception("User already exists");

        }

        [HttpPost("LogIn")]
        public IActionResult LogIn(UserForLoginDTO userForLogin)
        {
            string sqlForHashAndSalt = @"
            SELECT 
                [Email],
                [PasswordHash],
                [PasswordSalt] FROM TutorialAppSchema.Auth 
            WHERE Email = '" + userForLogin.Email + "'";

            UserForLoginConfirmationDTO userForConfirmation = _dapper.LoadDataSingle<UserForLoginConfirmationDTO>(sqlForHashAndSalt);

            byte[] passwordHash = GetPasswordHash(userForLogin.Password, userForConfirmation.PasswordSalt);

            for (int i = 0; i < passwordHash.Length; i++)
            {
                if (passwordHash[i] != userForConfirmation.PasswordHash[i])
                {
                    return StatusCode(401, "Invalid password");
                }
            }

            string userIdSql = @"
                        SELECT 
                            [UserId] 
                        FROM TutorialAppSchema.Users 
                        WHERE [Email] = + '" + userForLogin.Email + "'";

            int userId = _dapper.LoadDataSingle<int>(userIdSql);

            return Ok(new Dictionary<string, string> { { "token", CreateToken(userId) } });
        }

        private byte[] GetPasswordHash(string password, byte[] passwordSalt)
        {
            string passwordSaltPlusString = _config
                .GetSection("AppSettings:PasswordKey")
                .Value + Convert.ToBase64String(passwordSalt);

            return KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.ASCII.GetBytes(passwordSaltPlusString),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8
                );
        }

        private string CreateToken(int userId)
        {
            Claim[] claims = [
                new("userId", userId.ToString()),
                ];

            string tokenKeyString = _config.GetSection("AppSettings:TokenKey").Value ?? "";

            SymmetricSecurityKey tokenKey = new(Encoding.UTF8.GetBytes(tokenKeyString));

            SigningCredentials credentials = new(tokenKey, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor descriptor = new()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            JwtSecurityTokenHandler tokenHandler = new();

            SecurityToken token = tokenHandler.CreateToken(descriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}