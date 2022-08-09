using ELfR.Communications.Templates.Text.ArgumentPlaceholder;
using Microsoft.Extensions.DependencyInjection;

namespace ELfR.Communications.Templates.Configuration;

/// <summary>
/// Extension methods for configuring services for communications templates.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds services for communications templates.
    /// </summary>
    /// <param name="services">The services for dependency injection.</param>
    /// <returns>The services for dependency injection.</returns>
    public static IServiceCollection AddCommunicationsTemplates(this IServiceCollection services)
    {
        return
            services
                .AddLogging()
                .AddSingleton<ITemplateService<IReadOnlyDictionary<string, string>>, TextArgumentPlaceholderTemplateService>();
    }
}