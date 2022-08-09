namespace ELfR.Communications.Email;

/// <summary>
/// An e-mail message.
/// </summary>
/// <param name="Subject">The subject of the e-mail message.</param>
/// <param name="Body">The body of the e-mail message.</param>
/// <param name="IsHtml">Indicates whether the body is in HTML format.</param>
public record EmailMessage(string Subject, string Body, bool IsHtml);