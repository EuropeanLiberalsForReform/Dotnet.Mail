namespace ELfR.Communications.Email.Tests.Smtp
{
    internal record SmtpTestBatch(IReadOnlySet<EmailIdentity> Recipients, EmailMessage EmailMessage);
}