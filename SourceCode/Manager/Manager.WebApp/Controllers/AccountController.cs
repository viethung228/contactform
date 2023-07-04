using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Manager.WebApp.Helpers;
using Manager.WebApp.Models;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using MainApi.DataLayer.Entities;
using Autofac;
using Manager.DataLayer.Stores;
using Manager.SharedLibs;
using Manager.WebApp.Resources;
using Manager.WebApp.Services;
using Manager.WebApp.Settings;
using System.Linq;

namespace Manager.WebApp.Controllers
{
    public class AccountController : BaseController
    {
        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;

        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl = "")
        {
            var myKey = ".AspNetCore.JWT";
            var authToken = CommonHelpers.GetCookie(myKey);

            if (User.Identity.IsAuthenticated && !string.IsNullOrEmpty(authToken))
                return Redirect("/");

            LoginViewModel model = new LoginViewModel();
            model.ReturnUrl = returnUrl;

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.ReturnUrl))
                    model.ReturnUrl = "/";

                if (ModelState.IsValid)
                {
                    //var users = new List<IdentityUser>();
                    //users.Add(new IdentityUser { Id = "10", UserName = "admin", FullName = "Vũ Lương Bằng", PasswordHash = "123456" });

                    //var user = users.Where(x => x.UserName == model.UserName && x.PasswordHash == model.Password).FirstOrDefault();

                    model.UserName = model.UserName.ToStringNormally();

                    var pwd = model.Password.ToStringNormally();
                    pwd = Utility.Md5HashingData(pwd);
                    var storeUser = Startup.IocContainer.Resolve<IStoreUser>();

                    var user = storeUser.Login(new IdentityUser { UserName = model.UserName, PasswordHash = pwd });
                    if (user != null)
                    {
                        if (user.LockoutEnabled)
                        {
                            this.AddNotification("Your account was locked !!!", NotificationType.ERROR);

                            return View(model);
                        }

                        //Authorization token renewal
                        var authModel = new AuthUserModel
                        {
                            Id = user.Id,
                            StaffId = user.StaffId,
                            UserName = user.UserName,
                            AgencyId = (user.ParentId == 0) ? user.StaffId : user.ParentId
                        };

                        var apiRs = AuthTokenServices.RenewalAsync(authModel).Result;

                        var authToken = apiRs.Data != null ? apiRs.Data.ToString() : string.Empty;

                        if (!string.IsNullOrEmpty(authToken))
                        {
                            var myKey = ".AspNetCore.JWT";
                            CommonHelpers.SetCookie(myKey, authToken, 1 * 60 * 24);

                            //var myKey = string.Format(EnumFormatInfoCacheKeys.JWTAuthToken, user.StaffId);
                            //var cacheProvider = Startup.IocContainer.Resolve<ICacheProvider>();

                            ////Storage to cache
                            //cacheProvider.Set(myKey, authToken, SystemSettings.DefaultCachingTimeInMinutes);
                        }
                        else
                        {
                            this.AddNotification("Could not provided the Authorization Token", NotificationType.ERROR);

                            return View(model);
                        }

                        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);

                        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                        identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                        identity.AddClaim(new Claim(ClaimTypes.GivenName, user.FullName));

                        var principal = new ClaimsPrincipal(identity);

                        var authProperties = new AuthenticationProperties
                        {
                            AllowRefresh = true,
                            ExpiresUtc = DateTimeOffset.Now.AddDays(1),
                            IsPersistent = true,
                        };

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(principal), authProperties);

                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        this.AddNotification("The username or password is incorrect", NotificationType.ERROR);

                        return View(model);
                    }
                }
            }
            catch (Exception ex)
            {
                this.AddNotification(ManagerResource.LB_SYSTEM_BUSY, NotificationType.ERROR);
                _logger.LogError("Could not login: " + ex.ToString());
            }

            return View(model);
        }

        public IActionResult FakeLogin()
        {
            //var claims = new[] { new Claim(ClaimTypes.Name, "MyUserNameOrID"), new Claim(ClaimTypes.Role, "SomeRoleName") };

            //var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            //HttpContext.SignInAsync(
            //    CookieAuthenticationDefaults.AuthenticationScheme,
            //    new ClaimsPrincipal(identity));

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);

            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "bangkhmt3@gmail.com"));
            identity.AddClaim(new Claim(ClaimTypes.Name, "Vũ Lương Bằng"));
            identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));

            var principal = new ClaimsPrincipal(identity);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.Now.AddDays(1),
                IsPersistent = true,
            };

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(principal), authProperties);


            return Redirect("~/Home/Index");

            //ViewBag.RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            //return View("Login");
        }

        public async Task<IActionResult> LogOut()
        {
            try
            {
                //var myKey = string.Format(EnumFormatInfoCacheKeys.JWTAuthToken, CommonHelpers.GetCurrentStaffId());

                //CachingHelpers.ClearCacheByKey(myKey);

                var myKey = ".AspNetCore.JWT";
                var authToken = CommonHelpers.GetCookie(myKey);

                if (Request.Cookies.Count > 0)
                {
                    var siteCookies = Request.Cookies.Where(c => c.Key.Contains(".AspNetCore.") || c.Key.Contains("Microsoft.Authentication"));
                    foreach (var cookie in siteCookies)
                    {
                        Response.Cookies.Delete(cookie.Key);
                    }
                }
            }
            catch
            {
            }

            //SignOutAsync is Extension method for SignOut    
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            //Redirect to home page    
            return LocalRedirect("/");
        }
    }
}
