using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace ELfR.Communications.Templates.Text.ArgumentPlaceholder;

/// <summary>
/// A service that processes templates 
/// </summary>
public partial class TextArgumentPlaceholderTemplateService : ITemplateService<IReadOnlyDictionary<string, string>>
{
    private const string ArgumentGroupName = "Argument";
    private static readonly Regex PlaceholderRegex = new(@"\{\{(?<Argument>.+)\}\}");
    private readonly ILogger<TextArgumentPlaceholderTemplateService> logger;

    /// <summary>
    /// Initializes a new instance of <see cref="TextArgumentPlaceholderTemplateService" />.
    /// </summary>
    /// <param name="logger">Logs messages for debugging and troubleshooting.</param>
    public TextArgumentPlaceholderTemplateService(ILogger<TextArgumentPlaceholderTemplateService> logger)
    {
        this.logger = logger;
        LogInitialized(this.logger, null);
    }

    /// <inheritdoc />
    public async Task HydrateAsync(
        Stream templateStream,
        Stream outputStream,
        IReadOnlyDictionary<string, string> context,
        CancellationToken cancellationToken = default)
    {
        LogHydrateStarted(this.logger, null);
        try
        {
            using var textReader = new StreamReader(templateStream);
            using var textWriter = new StreamWriter(outputStream, leaveOpen: true);
            while ((await textReader.ReadLineAsync()) is { } line)
            {
                cancellationToken.ThrowIfCancellationRequested();

                string PlaceholderEvaluator(Match match)
                {
                    var argument = match.Groups[ArgumentGroupName].Value;
                    if (context.TryGetValue(argument, out var value))
                    {
                        return value;
                    }
                    return match.Value;
                }

                line = PlaceholderRegex.Replace(line, PlaceholderEvaluator);
                textWriter.WriteLine(line);
            }
            await textWriter.FlushAsync();
            outputStream.Seek(0L, SeekOrigin.Begin);
        }
        catch (Exception ex)
        {
            LogHydrateError(this.logger, ex);
            throw new TextArgumentPlaceholderTemplateException(ex);
        }
        LogHydrateFinished(this.logger, null);
    }
}