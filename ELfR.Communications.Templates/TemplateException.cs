namespace ELfR.Communications.Templates;

/// <summary>
/// An exception that is thrown if an error occurs during the processing of a communications template.
/// </summary>
public class TemplateException : Exception
{
    /// <summary>
    /// Initializes a new instance of <see cref="TemplateException" />.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The inner exception.</param>
    public TemplateException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
