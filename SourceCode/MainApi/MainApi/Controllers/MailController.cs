using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MainApi.Helpers;
using MainApi.Models;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using MainApi.DataLayer.Entities;
using Autofac;
using MainApi.DataLayer.Stores;
using MainApi.SharedLibs;
using MainApi.Resources;
using StackExchange.Redis;
using System.Collections.Generic;
using Square.Models;
using System.Configuration;
using Microsoft.AspNetCore.Http;
using Manager.WebApp.Services;
using Polly;
using MimeKit;
using MailKit.Net.Smtp;
using RestSharp;
using System.Text.Json;
using MailKit.Security;
using Manager.Business;
using System.IO;
using Microsoft.Extensions.Options;

namespace MainApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : BaseController
    {
        private readonly ILogger<MailController> _logger;
        private readonly IMailService mailService;
        public MailController(IMailService mailService)
        {
            this.mailService = mailService;
        }
        [HttpPost("send")]
        public async Task<IActionResult> SendMail([FromForm] MailRequest request)
        {
            try
            {               
                await mailService.SendEmailAsync(request);
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost("sendwithhtml")]
        public async Task<IActionResult> SendHTMLMail([FromForm] MailRequestWithHTML request)
        {
            try
            {
                await mailService.SendHTMLEmailAsync(request);
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

}
