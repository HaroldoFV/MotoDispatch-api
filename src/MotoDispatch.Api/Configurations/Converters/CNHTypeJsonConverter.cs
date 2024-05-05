using System.Text.Json;
using System.Text.Json.Serialization;
using MotoDispatch.Domain.Enum;

namespace MotoDispatch.Api.Configurations.Converters;

public class CNHTypeJsonConverter : JsonConverter<CNHType>
{
    public override CNHType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string value = reader.GetString() ?? string.Empty;

        if (Enum.TryParse<CNHType>(value, true, out CNHType result))
        {
            return result;
        }

        throw new JsonException($"Value {value} is not valid for CNHType.");
    }

    public override void Write(Utf8JsonWriter writer, CNHType value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}