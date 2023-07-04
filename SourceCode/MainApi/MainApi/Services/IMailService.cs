using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using Serilog;
using System.IO;
using RestSharp;
using System.Reflection;
using MainApi.Models;
using MainApi.Settings;
using MainApi.SharedLibs;
using System.Collections.Generic;
using MainApi.DataLayer.Entities;
using MailKit;
using MimeKit;
using MailKit.Security;
using Manager.Business;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MainApi.Services;

namespace Manager.WebApp.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
        Task SendHTMLEmailAsync(MailRequestWithHTML request);
    }
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        private readonly ILogger<MailService> _logger;
        private readonly IViewRenderService _viewRenderService;
        public MailService(IOptions<MailSettings> mailSettings, ILogger<MailService> logger, IViewRenderService viewRender)
        {
            _mailSettings = mailSettings.Value;
            _logger = logger;
            _viewRenderService = viewRender;
        }
        public async Task SendHTMLEmailAsync(MailRequestWithHTML request)
        {
            try
            {
                var res = _viewRenderService.RenderToStringAsync("\\Views\\Templates\\SendEmailTemplate.cshtml", null);
                //string FilePath = Directory.GetCurrentDirectory() + "\\Views\\Templates\\SendEmailTemplate.cshtml";
                //StreamReader str = new StreamReader(FilePath);
                //string MailText = str.ReadToEnd();
                //str.Close();
                var mailText = res.Result;
                mailText = res.Result.Replace("[username]", request.UserName).Replace("[email]", request.ToEmail).Replace("[link]", request.Link).Replace("[ID]", request.IdUser);
                //MailText = MailText.Replace("[username]", request.UserName).Replace("[email]", request.ToEmail).Replace("[link]", request.Link).Replace("[ID]", request.IdUser);

                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
                email.To.Add(MailboxAddress.Parse(request.ToEmail));
                email.Subject = $"Welcome {request.UserName}";

                var builder = new BodyBuilder();
                builder.HtmlBody = mailText;
                email.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            if (mailRequest.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in mailRequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}