namespace ELfR.Communications.Email.Tests.Smtp
{
    internal record SmtpTestBatch(IReadOnlyList<SmtpTestItem> Items);
}