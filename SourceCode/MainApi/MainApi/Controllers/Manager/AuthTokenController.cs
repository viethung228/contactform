using MainApi.SharedLibs;
using MainApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;
using System.Reflection;
using JWT.Exceptions;
using MainApi.DataLayer.Entities;

namespace MainApi.Controllers
{
    [ApiController]
    [Route("api/manager/authtoken")]
    public class MAuthTokenController : ControllerBase
    {
        private readonly ILogger<MAuthTokenController> _logger;

        public MAuthTokenController(ILogger<MAuthTokenController> logger)
        {
            _logger = logger;
        }
      
        [HttpPost("renewal")]
        public ApiResponseModel ProvideToken(AuthUserModel user)
        {
            var returnModel = new ApiResponseModel();
            try
            {
                if(user != null)
                {
                    //Create claims details based on the user information
                    var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, AppConfiguration.GetAppsetting("Jwt:Subject")),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("StaffId", user.StaffId.ToString()),
                        new Claim("UserName", user.UserName)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppConfiguration.GetAppsetting("Jwt:Key")));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        AppConfiguration.GetAppsetting("Jwt:Issuer"),
                        AppConfiguration.GetAppsetting("Jwt:Audience"),
                        claims,
                        expires: DateTime.Now.AddMinutes(10080),
                        signingCredentials: signIn);

                    var result = new JwtSecurityTokenHandler().WriteToken(token);

                    returnModel.Data = result;

                    return returnModel;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return returnModel;
        }

        [HttpGet("checktoken")]
        public ActionResult CheckToken()
        {
            try
            {                
                var tokenHandler = new JwtSecurityTokenHandler();
                var secretKey = AppConfiguration.GetAppsetting("Jwt:Key");
                var key = Encoding.ASCII.GetBytes(secretKey);

                var token = HttpContext.Request.Headers["Authorization"].ToString();
                if (!string.IsNullOrEmpty(token))
                {
                    token = token.Replace("Bearer ", string.Empty);
                }

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "UserId").Value);
            }
            catch (TokenExpiredException)
            {
                _logger.LogError("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, "Token has expired");
            }
            catch (SignatureVerificationException)
            {
                _logger.LogError("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, "Token has invalid signature");
            }
            catch (Exception ex)
            {
                _logger.LogError("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return Content("Ok");
        }
    }
}
