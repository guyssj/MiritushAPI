
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Miritush.Services.Abstract;
using Miritush.DTO;
namespace Miritush.Services
{

    // Email Service Implementation
    public class MailService : IMailService
    {
        private readonly MailSettings _settings;
        private readonly ILogger<MailService> _logger;

        public MailService(IOptions<MailSettings> settings, ILogger<MailService> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task<bool> SendEmailAsync(MailRequest request)
        {
            try
            {
                var message = new MimeMessage();

                // Sender
                message.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));

                // Recipients
                foreach (var email in request.ToEmails)
                {
                    message.To.Add(MailboxAddress.Parse(email));
                }

                // CC
                if (request.CcEmails != null)
                {
                    foreach (var email in request.CcEmails)
                    {
                        message.Cc.Add(MailboxAddress.Parse(email));
                    }
                }

                // BCC
                if (request.BccEmails != null)
                {
                    foreach (var email in request.BccEmails)
                    {
                        message.Bcc.Add(MailboxAddress.Parse(email));
                    }
                }

                // Subject
                message.Subject = request.Subject;

                // Body
                var builder = new BodyBuilder();
                if (request.IsHtml)
                {
                    builder.HtmlBody = request.Body;
                }
                else
                {
                    builder.TextBody = request.Body;
                }

                // Attachments
                if (request.Attachments != null)
                {
                    foreach (var attachment in request.Attachments)
                    {
                        builder.Attachments.Add(
                            attachment.FileName,
                            attachment.Content,
                            ContentType.Parse(attachment.ContentType)
                        );
                    }
                }

                message.Body = builder.ToMessageBody();

                // Send email
                using var smtp = new SmtpClient();

                await smtp.ConnectAsync(
                    _settings.SmtpServer,
                    _settings.SmtpPort,
                    _settings.UseSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None
                );

                await smtp.AuthenticateAsync(_settings.Username, _settings.Password);
                await smtp.SendAsync(message);
                await smtp.DisconnectAsync(true);

                _logger.LogInformation($"Email sent successfully to {string.Join(", ", request.ToEmails)}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email");
                return false;
            }
        }

        public async Task<bool> SendSimpleEmailAsync(string toEmail, string subject, string body)
        {
            var request = new MailRequest
            {
                ToEmails = new List<string> { toEmail },
                Subject = subject,
                Body = body,
                IsHtml = true
            };

            return await SendEmailAsync(request);
        }
    }
}
