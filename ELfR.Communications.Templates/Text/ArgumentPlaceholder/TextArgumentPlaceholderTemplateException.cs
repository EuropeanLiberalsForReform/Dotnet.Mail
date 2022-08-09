using ELfR.Communications.Templates.Text.ArgumentPlaceholder.Resources;

namespace ELfR.Communications.Templates.Text.ArgumentPlaceholder;

/// <summary>
/// An exception that is thrown if an error occurred during the processing of a communications template that uses text with argument placeholders.
/// </summary>
public class TextArgumentPlaceholderTemplateException : TemplateException
{
    /// <summary>
    /// Initializes a new instance of <see cref="TextArgumentPlaceholderTemplateException" />.
    /// </summary>
    /// <param name="innerException">The inner exception.</param>
    public TextArgumentPlaceholderTemplateException(Exception innerException)
        : base(ExceptionMessages.UnexpectedError, innerException)
    {
    }
}