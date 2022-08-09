namespace ELfR.Communications.Templates;

/// <summary>
/// A template service for hydrating communications templates.
/// </summary>
public interface ITemplateService<TContext>
{
    /// <summary>
    /// Hydrates a template.
    /// </summary>
    /// <param name="templateStream">The stream that contains the template.</param>
    /// <param name="outputStream">The stream that will contain the hydrated template.</param>
    /// <param name="context">The templating context.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <exception cref="TemplateException">
    /// A <see cref="TemplateException" /> is thrown if an error occurred during the processing of the template.
    /// </exception>
    Task HydrateAsync(Stream templateStream, Stream outputStream, TContext context, CancellationToken cancellationToken = default);
}
