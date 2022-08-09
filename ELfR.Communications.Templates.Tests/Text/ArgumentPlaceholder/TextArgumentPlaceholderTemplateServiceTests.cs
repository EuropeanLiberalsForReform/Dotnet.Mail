using ELfR.Communications.Templates.Text.ArgumentPlaceholder;
using Microsoft.Extensions.Logging.Abstractions;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ELfR.Communications.Templates.Tests.Text.ArgumentPlaceholder;

public class TextArgumentPlaceholderTemplateServiceTests
{
    private static readonly Assembly Assembly = typeof(TextArgumentPlaceholderTemplateServiceTests).Assembly;
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    private const string ResourcesNamespace = "ELfR.Communications.Templates.Tests.Text.ArgumentPlaceholder.Resources.";
    private static readonly Regex InputRegex = new(@$"^{ResourcesNamespace}Input_(?<Id>.+)\.json$");
    private static readonly Regex SubjectOutputRegex = new(@$"^{ResourcesNamespace}Output_Subject_(?<Id>.+)\.txt$");
    private static readonly Regex BodyOutputRegex = new(@$"^{ResourcesNamespace}Output_Body_(?<Id>.+)\.txt$");

    [Fact]
    public async Task HydrateTestAsync()
    {
        // Arrange
        var logger = new NullLogger<TextArgumentPlaceholderTemplateService>();
        var templateService = new TextArgumentPlaceholderTemplateService(logger);
        var cancellationToken = CancellationToken.None;
        var inputs =
            Assembly
                .GetManifestResourceNames()
                .Where(rn => InputRegex.IsMatch(rn))
                .Select(rn =>
                {
                    var key = InputRegex.Match(rn).Groups["Id"].Value;
                    using var resourceStream = Assembly.GetManifestResourceStream(rn)!;
                    using var streamReader = new StreamReader(resourceStream);
                    var value = streamReader.ReadToEnd();
                    var input = JsonSerializer.Deserialize<TemplateWithArguments>(value, JsonSerializerOptions)!;
                    return new KeyValuePair<string, TemplateWithArguments>(key, input);
                })
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        var subjectOutputs =
            Assembly
                .GetManifestResourceNames()
                .Where(rn => SubjectOutputRegex.IsMatch(rn))
                .Select(rn =>
                {
                    var key = SubjectOutputRegex.Match(rn).Groups["Id"].Value;
                    using var resourceStream = Assembly.GetManifestResourceStream(rn)!;
                    using var streamReader = new StreamReader(resourceStream);
                    var value = streamReader.ReadToEnd();
                    return new KeyValuePair<string, string>(key, value);
                })
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        var bodyOutputs =
            Assembly
                .GetManifestResourceNames()
                .Where(rn => BodyOutputRegex.IsMatch(rn))
                .Select(rn =>
                {
                    var key = BodyOutputRegex.Match(rn).Groups["Id"].Value;
                    using var resourceStream = Assembly.GetManifestResourceStream(rn)!;
                    using var streamReader = new StreamReader(resourceStream);
                    var value = streamReader.ReadToEnd();
                    return new KeyValuePair<string, string>(key, value);
                })
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        foreach (var (key, input) in inputs)
        {
            // Act
            var context = input.Arguments;
            using var subjectTemplateStream = new MemoryStream(Encoding.UTF8.GetBytes(input.Subject));
            using var subjectOutputStream = new MemoryStream();
            await templateService.HydrateAsync(subjectTemplateStream, subjectOutputStream, context, cancellationToken);
            using var bodyTemplateStream = new MemoryStream(Encoding.UTF8.GetBytes(input.Body));
            using var bodyOutputStream = new MemoryStream();
            await templateService.HydrateAsync(bodyTemplateStream, bodyOutputStream, context, cancellationToken);

            // Assert
            using var subjectStreamReader = new StreamReader(subjectOutputStream);
            var actualSubject = await subjectStreamReader.ReadToEndAsync();
            var expectedSubject = subjectOutputs[key];
            Assert.Equal(expectedSubject, actualSubject);
            using var bodyStreamReader = new StreamReader(bodyOutputStream);
            var actualBody = await bodyStreamReader.ReadToEndAsync();
            var expectedBody = bodyOutputs[key];
            Assert.Equal(expectedBody, actualBody);
        }
    }
}
