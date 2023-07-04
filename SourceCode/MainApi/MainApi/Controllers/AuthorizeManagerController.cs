using JWT.Exceptions;
using MainApi.SharedLibs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;
using System;
using System.Linq;
using Serilog;

namespace MainApi.Controllers
{
    public class AuthorizeManagerController : ControllerBase
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(AuthorizeManagerController));

        [NonAction]
        public int GetCurrentUserId()
        {
            var staffId = 0;
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

                if(jwtToken != null)
                {
                    staffId = int.Parse(jwtToken.Claims.First(x => x.Type == "StaffId").Value);
                }                
            }
            catch (TokenExpiredException)
            {
                _logger.Error("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, "Token has expired");
            }
            catch (SignatureVerificationException)
            {
                _logger.Error("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, "Token has invalid signature");
            }
            catch (Exception ex)
            {
                _logger.Error("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return staffId;
        }

        [NonAction]
        public int GetCurrentAgencyId()
        {
            var staffId = 0;
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

                if (jwtToken != null)
                {
                    staffId = int.Parse(jwtToken.Claims.First(x => x.Type == "StaffId").Value);
                }
            }
            catch (TokenExpiredException)
            {
                _logger.Error("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, "Token has expired");
            }
            catch (SignatureVerificationException)
            {
                _logger.Error("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, "Token has invalid signature");
            }
            catch (Exception ex)
            {
                _logger.Error("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return staffId;
        }

        [NonAction]
        public int GetCurrentAgentId()
        {
            var agentId = 0;
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

                if (jwtToken != null)
                {
                    agentId = int.Parse(jwtToken.Claims.First(x => x.Type == "AgentId").Value);
                }
            }
            catch (TokenExpiredException)
            {
                _logger.Error("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, "Token has expired");
            }
            catch (SignatureVerificationException)
            {
                _logger.Error("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, "Token has invalid signature");
            }
            catch (Exception ex)
            {
                _logger.Error("Api {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return agentId;
        }
    }
}
