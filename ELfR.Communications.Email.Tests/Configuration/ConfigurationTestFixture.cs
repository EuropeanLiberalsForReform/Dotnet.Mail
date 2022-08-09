using ELfR.Communications.Email.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ELfR.Communications.Email.Tests.Configuration;

/// <summary>
/// A test fixture that enables configuration of tests.
/// </summary>
public class ConfigurationTestFixture
{
    /// <summary>
    /// Initializes a new instance of <see cref="ConfigurationTestFixture" />.
    /// </summary>
    public ConfigurationTestFixture()
    {
        var configuration =
            new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json", true)
                .Build();
        this.ServiceProvider =
            new ServiceCollection()
                .AddEmail(configuration.GetSection("Email"))
                .BuildServiceProvider();
    }

    internal IServiceProvider ServiceProvider { get; }
}
