using Lavcode.Asp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Lavcode.Asp.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthTokenService _authTokenService;
        private readonly IConfiguration _configuration;
        public AuthController(AuthTokenService authTokenService, IConfiguration configuration)
        {
            _authTokenService = authTokenService;
            _configuration = configuration;
        }

        /// <summary>
        /// Get login token
        /// </summary>
        /// <param name="password">Lavcode password</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetToken([FromQuery, Required] string password)
        {
            var secretKey = _configuration["SecretKey"];
            try
            {
                var base64 = Encoding.UTF8.GetString(Convert.FromBase64String(password));
                if (base64 != secretKey)
                {
                    return Unauthorized("密码错误");
                }
            }
            catch (FormatException)
            {
                return BadRequest("password must be base64 encoded");
            }

            var token = _authTokenService.CreateJwtToken();
            return this.Ok(new
            {
                token
            });
        }

        /// <summary>
        /// Verify token
        /// </summary>
        /// <returns></returns>
        [HttpHead]
        [Authorize]
        public IActionResult VerifyToken()
        {
            return this.NoContent();
        }
    }
}
