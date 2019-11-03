using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ESMS.Services
{
    public class EmailSender : IEmailSender
    {
        public IConfiguration configuration { get; set; }

        public EmailSender(IConfiguration configuration)
        {
           this.configuration = configuration;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(configuration.GetSection("AppSettings").GetSection("email").Value);
            mailMessage.To.Add(new MailAddress(email));
            mailMessage.Subject = subject;
            mailMessage.Body = htmlMessage;

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", int.Parse(configuration.GetSection("AppSettings").GetSection("smtpPort").Value));
            NetworkCredential credential = new NetworkCredential(configuration.GetSection("AppSettings").GetSection("email").Value,
                configuration.GetSection("AppSettings").GetSection("password").Value);
            smtpClient.Credentials = credential;
            smtpClient.EnableSsl = true;
            try
            {
                smtpClient.SendMailAsync(mailMessage);
                return Task.CompletedTask;
            } catch (Exception ex)
            {
                return Task.FromResult(0);
            }
        }
    }
}
