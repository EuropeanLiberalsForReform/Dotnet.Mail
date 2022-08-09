namespace ELfR.Communications.Email.Configuration;

/// <summary>
/// The e-mail settings.
/// </summary>
public class EmailSettings
{
    /// <summary>
    /// Gets or sets the host name of the server.
    /// </summary>
    public SmtpSettings Smtp { get; set; } = new SmtpSettings();

    /// <summary>
    /// Gets or sets the default sender.
    /// </summary>
    public EmailIdentity Sender { get; set; } = new("no-reply@elfr.eu", "European Liberals for Reform");

    /// <summary>
    /// Gets or sets the interval between sending e-mails in bulks.
    /// </summary>
    public TimeSpan SendInterval { get; set; } = TimeSpan.FromSeconds(30.0D);
}
