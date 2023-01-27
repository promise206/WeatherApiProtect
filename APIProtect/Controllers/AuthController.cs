using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Weather.Core.DTOs;
using Weather.Core.Interfaces;

namespace APIProtect.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// Validates, Register and Creates Wallet for User
        /// </summary>
        /// <returns>Registration Status.</returns>
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationDTO model)
        {
            var result = await _authService.Register(model);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Authenticate a user that has access to our application
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            var response = await _authService.Login(model);
            return StatusCode(response.StatusCode, response);
        }
    }
}
