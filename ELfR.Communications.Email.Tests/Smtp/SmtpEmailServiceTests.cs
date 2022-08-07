using ELfR.Communications.Email.Configuration;
using ELfR.Communications.Email.Tests.Configuration;
using ELfR.Communications.Email.Tests.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ELfR.Communications.Email.Tests.Smtp
{
    [Collection(nameof(ConfigurationTestCollection))]
    public class SmtpEmailServiceTests
    {
        private static readonly Regex SendRegex = new(@"Send_[0-9a-zA-Z]\.json$");
        private static readonly Regex SendBulkRegex = new(@"SendBulk_[0-9a-zA-Z]\.json$");
        private static readonly Assembly Assembly = typeof(SmtpEmailServiceTests).Assembly;
        private static readonly JsonSerializerOptions JsonSerializerOptions =
            new()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        private readonly ConfigurationTestFixture configurationTestFixture;

        public SmtpEmailServiceTests(ConfigurationTestFixture configurationTestFixture)
        {
            this.configurationTestFixture = configurationTestFixture;
        }

        [Fact]
        public async Task SendTestAsync()
        {
            // Arrange
            var emailSettings = this.configurationTestFixture.ServiceProvider.GetRequiredService<IOptions<EmailSettings>>().Value;
            var cancellationToken = CancellationToken.None;
            IEmailService smtpEmailService = this.configurationTestFixture.ServiceProvider.GetRequiredService<IEmailService>();
            var items =
                Assembly
                    .GetManifestResourceNames()
                    .Where(rn => SendRegex.IsMatch(rn))
                    .Select(rn =>
                    {
                        using var stream = Assembly.GetManifestResourceStream(rn)!;
                        return JsonSerializer.Deserialize<SmtpTestItem>(new StreamReader(stream).ReadToEnd(), JsonSerializerOptions)!;
                    })
                    .ToArray();

            // Act
            foreach (var item in items)
            {
                var (recipient, emailMessage) = item;
                await smtpEmailService.SendAsync(recipient, emailMessage, cancellationToken);
            }

            // Assert
            var pickupDirectoryLocation = new DirectoryInfo(emailSettings.Smtp.PickupDirectoryLocation!);
            Assert.True(pickupDirectoryLocation.GetFiles().Any());
        }

        [Fact]
        public async Task SendBulkTestAsync()
        {
            // Arrange
            var emailSettings = this.configurationTestFixture.ServiceProvider.GetRequiredService<IOptions<EmailSettings>>().Value;
            var cancellationToken = CancellationToken.None;
            IEmailService smtpEmailService = this.configurationTestFixture.ServiceProvider.GetRequiredService<IEmailService>();
            JsonSerializerOptions.Converters.Add(new JsonReadOnlySetConverterFactory());
            var items =
                Assembly
                    .GetManifestResourceNames()
                    .Where(rn => SendBulkRegex.IsMatch(rn))
                    .Select(rn =>
                    {
                        using var stream = Assembly.GetManifestResourceStream(rn)!;
                        return JsonSerializer.Deserialize<SmtpTestBatch>(new StreamReader(stream).ReadToEnd(), JsonSerializerOptions)!;
                    })
                    .ToArray();

            // Act
            foreach (var item in items)
            {
                var (recipients, emailMessage) = item;
                await smtpEmailService.SendBulkAsync(recipients, emailMessage, cancellationToken);
            }

            // Assert
            var pickupDirectoryLocation = new DirectoryInfo(emailSettings.Smtp.PickupDirectoryLocation!);
            Assert.True(pickupDirectoryLocation.GetFiles().Any());
        }
    }
}
