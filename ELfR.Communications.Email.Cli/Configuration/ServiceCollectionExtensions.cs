using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ELfR.Communications.Email.Cli.Configuration;

/// <summary>
/// Extension methods for Command Line Interface (CLI) services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Command Line Interface (CLI) services.
    /// </summary>
    /// <param name="services">The services for dependency injection.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <returns>The services for dependency injection.</returns>
    public static IServiceCollection AddCli(this IServiceCollection services, IConfiguration configuration)
    {
        return services.Configure<CliOptions>(configuration);
    }
}