using System.Net.Mail;

namespace ELfR.Communications.Email.Configuration;

/// <summary>
/// The settings for SMTP clients.
/// </summary>
public class SmtpSettings
{
    /// <summary>
    /// Gets or sets the SMTP delivery method.
    /// </summary>
    public SmtpDeliveryMethod DeliveryMethod { get; set; } = SmtpDeliveryMethod.Network;

    /// <summary>
    /// Gets or sets the pickup directory location.
    /// </summary>
    public string? PickupDirectoryLocation { get; set; }

    /// <summary>
    /// Gets or sets the host name of the SMTP server.
    /// </summary>
    public string? Server { get; set; }

    /// <summary>
    /// Gets or sets the username of the SMTP account.
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Gets or sets the password of the SMTP account.
    /// </summary>
    public string? Password { get; set; }
}