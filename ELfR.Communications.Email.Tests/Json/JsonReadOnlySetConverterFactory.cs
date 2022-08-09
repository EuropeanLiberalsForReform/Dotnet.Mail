using System.Text.Json;
using System.Text.Json.Serialization;

namespace ELfR.Communications.Email.Tests.Json;

public class JsonReadOnlySetConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(IReadOnlySet<>);
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        if (this.CanConvert(typeToConvert))
        {
            var genericTypeParameter = typeToConvert.GetGenericArguments()[0];
            var readOnlySetType = typeof(IReadOnlySet<>).MakeGenericType(genericTypeParameter);
            var jsonReadOnlySetConverter = typeof(JsonReadOnlySetConverter<,>).MakeGenericType(readOnlySetType, genericTypeParameter);
            return (JsonConverter)Activator.CreateInstance(jsonReadOnlySetConverter)!;
        }
        return null;
    }
}
