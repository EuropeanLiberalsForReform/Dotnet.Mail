namespace ELfR.Communications.Email
{
    /// <summary>
    /// A service that sends e-mails.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Sends an e-mail message.
        /// </summary>
        /// <param name="recipient">The e-mail recipient.</param>
        /// <param name="emailMessage">The e-mail message.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="EmailException">An <see cref="EmailException" /> is thrown if an error occurred during the submission of the e-mail.</exception>
        Task SendAsync(EmailIdentity recipient, EmailMessage emailMessage, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends an e-mail message to multiple recipients.
        /// </summary>
        /// <param name="recipients">The e-mail recipient.</param>
        /// <param name="emailMessage">The e-mail message.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="EmailException">An <see cref="EmailException" /> is thrown if an error occurred during the submission of the e-mails.</exception>
        Task SendBulkAsync(IReadOnlySet<EmailIdentity> recipients, EmailMessage emailMessage, CancellationToken cancellationToken = default);
    }
}