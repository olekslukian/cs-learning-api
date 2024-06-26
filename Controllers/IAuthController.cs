using DotnetAPI.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers
{
    public interface IAuthController
    {
        public IActionResult Register(UserForRegistrationDTO user);
        public IActionResult LogIn(UserForLoginDTO user);
    }
}