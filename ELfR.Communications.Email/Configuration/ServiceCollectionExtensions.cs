using ELfR.Communications.Email.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Text.Json;

namespace ELfR.Communications.Email.Configuration
{
    /// <summary>
    /// Extension methods for dependency injection.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds e-mail services for dependency injection.
        /// </summary>
        /// <param name="services">The injectable services.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <returns>The injectable services.</returns>
        /// <exception cref="ArgumentOutOfRangeException">An invalid SMTP delivery method was specified.</exception>
        public static IServiceCollection AddEmail(this IServiceCollection services, IConfiguration configuration)
        {
            return
                services
                    .AddLogging()
                    .Configure<EmailSettings>(configuration)
                    .AddSingleton<IEmailService, SmtpEmailService>()
                    .AddScoped(sp =>
                    {
                        var emailSettings = sp.GetRequiredService<IOptions<EmailSettings>>().Value;
                        var smtpSettings = emailSettings.Smtp;
                        return smtpSettings.DeliveryMethod switch
                        {
                            SmtpDeliveryMethod.PickupDirectoryFromIis =>
                                new SmtpClient
                                {
                                    DeliveryMethod = smtpSettings.DeliveryMethod,
                                    PickupDirectoryLocation = smtpSettings.PickupDirectoryLocation
                                },
                            SmtpDeliveryMethod.Network =>
                                new SmtpClient(smtpSettings.Server)
                                {
                                    DeliveryMethod = smtpSettings.DeliveryMethod,
                                    Credentials = new NetworkCredential(smtpSettings.Username, smtpSettings.Password)
                                },
                            SmtpDeliveryMethod.SpecifiedPickupDirectory =>
                                new SmtpClient
                                {
                                    DeliveryMethod = smtpSettings.DeliveryMethod,
                                    PickupDirectoryLocation = smtpSettings.PickupDirectoryLocation
                                },
                            _ => throw new ArgumentOutOfRangeException()
                        };
                    });
        }
    }
}