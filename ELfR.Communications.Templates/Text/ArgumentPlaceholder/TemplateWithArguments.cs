namespace ELfR.Communications.Templates.Text.ArgumentPlaceholder;

/// <summary>
/// A template with arguments.
/// </summary>
/// <param name="Arguments">The template's arguments.</param>
/// <param name="Subject">The template's subject.</param>
/// <param name="Body">The template's contents</param>
public record TemplateWithArguments(IReadOnlyDictionary<string, string> Arguments, string Subject, string Body);