using ELfR.Communications.Email.Configuration;
using ELfR.Communications.Email.Tests.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Text.Json;

namespace ELfR.Communications.Email.Tests.Smtp
{
    [Collection(nameof(ConfigurationTestCollection))]
    public class SmtpEmailServiceTests
    {
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
                    .Where(rn => rn.EndsWith(".json"))
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
    }
}
