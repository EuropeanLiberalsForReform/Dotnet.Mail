namespace ELfR.Communications.Email.Cli.Configuration;

public class CliOptions
{
    /// <summary>
    /// Gets or sets the path to a file that contains a recipients list.
    /// </summary>
    public string? RecipientsListPath { get; set; }

    /// <summary>
    /// Gets or sets the path to a file that contains a template and arguments.
    /// </summary>
    public string? TemplateWithArgumentsPath { get; set; }
}