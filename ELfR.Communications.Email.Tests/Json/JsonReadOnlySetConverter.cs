using System.Text.Json;
using System.Text.Json.Serialization;

namespace ELfR.Communications.Email.Tests.Json
{
    public class JsonReadOnlySetConverter<TSet, TItem> : JsonConverter<TSet>
        where TSet : IReadOnlySet<TItem>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(IReadOnlySet<>);
        }

        public override TSet? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (this.CanConvert(typeToConvert))
            {
                var set = new HashSet<TItem>();
                if (reader.TokenType != JsonTokenType.StartArray)
                {
                    throw new JsonException("Expected an array.");
                }
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndArray)
                    {
                        break;
                    }
                    var item = JsonSerializer.Deserialize<TItem>(ref reader, options)!;
                    set.Add(item);
                }
                return (TSet)(IReadOnlySet<TItem>)set;
            }
            return default;
        }

        public override void Write(Utf8JsonWriter writer, TSet value, JsonSerializerOptions options)
        {
            if (value is not null && this.CanConvert(value.GetType()))
            {
                writer.WriteStartArray();
                foreach (var item in value)
                {
                    JsonSerializer.Serialize(writer, item, options);
                }
                writer.WriteEndArray();
            }
        }
    }
}
