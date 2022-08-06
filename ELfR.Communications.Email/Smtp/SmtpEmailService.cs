using ELfR.Communications.Email.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Mail;

namespace ELfR.Communications.Email.Smtp
{
    /// <summary>
    /// An e-mail service that sends e-mails through SMTP.
    /// </summary>
    public partial class SmtpEmailService : IEmailService
    {
        private readonly ILogger<SmtpEmailService> logger;
        private readonly IOptions<EmailSettings> emailSettings;
        private readonly SmtpClient smtpClient;

        /// <summary>
        /// Initializes a new instance of <see cref="SmtpEmailService" />.
        /// </summary>
        /// <param name="logger">Logs messages for debugging and troubleshooting.</param>
        /// <param name="emailSettings">The e-mail settings.</param>
        /// <param name="smtpClient">The SMTP client.</param>
        public SmtpEmailService(ILogger<SmtpEmailService> logger, IOptions<EmailSettings> emailSettings, SmtpClient smtpClient)
        {
            this.logger = logger;
            this.emailSettings = emailSettings;
            this.smtpClient = smtpClient;
            LogInitialized(this.logger, null);
        }

        /// <inheritdoc />
        public async Task SendAsync(EmailIdentity recipient, EmailMessage emailMessage, CancellationToken cancellationToken = default)
        {
            LogSendStart(this.logger, null);
            try
            {
                var emailSettingsCurrent = this.emailSettings.Value;
                var (subject, body, isHtml) = emailMessage;
                var from = new MailAddress(emailSettingsCurrent.Sender.Address, emailSettingsCurrent.Sender.Name);
                var to = new MailAddress(recipient.Address, recipient.Name);
                var mailMessage = new MailMessage(from, to) { Body = body, Subject = subject, IsBodyHtml = isHtml };
                await this.smtpClient.SendMailAsync(mailMessage, cancellationToken);
            }
            catch (Exception ex)
            {
                LogSendError(this.logger, ex);
                throw new SmtpEmailException(ex);
            }
            LogSendFinish(this.logger, null);
        }

        /// <inheritdoc />
        public async Task SendBulkAsync(IReadOnlySet<EmailIdentity> recipients, EmailMessage emailMessage, CancellationToken cancellationToken = default)
        {
            LogSendBulkStart(this.logger, null);
            var emailSettingsCurrent = this.emailSettings.Value;
            var (subject, body, isHtml) = emailMessage;
            var from = new MailAddress(emailSettingsCurrent.Sender.Address, emailSettingsCurrent.Sender.Name);
            foreach (var recipient in recipients)
            {
                try
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var to = new MailAddress(recipient.Address, recipient.Name);
                    var mailMessage = new MailMessage(from, to) { Body = body, Subject = subject, IsBodyHtml = isHtml };
                    await this.smtpClient.SendMailAsync(mailMessage, cancellationToken);
                    await Task.Delay(emailSettingsCurrent.SendInterval, cancellationToken);
                }
                catch (Exception ex)
                {
                    LogSendBulkError(this.logger, ex);
                }
            }
            LogSendBulkFinish(this.logger, null);
        }
    }
}