using System;
using Newtonsoft.Json;

namespace VirtoCommerce.BuilderIO.Core.Converters;

public class UnixMillisecondsJsonConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        if (value is DateTime dateTime)
        {
            var unixTimeMilliseconds = new DateTimeOffset(dateTime).ToUnixTimeMilliseconds();
            writer.WriteValue(unixTimeMilliseconds);
        }
        else
        {
            throw new JsonSerializationException("Expected date object value.");
        }
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
        {
            return objectType == typeof(DateTime?) ? null : default(DateTime);
        }

        if (reader.TokenType == JsonToken.Integer || reader.TokenType == JsonToken.Float)
        {
            var unixTimeMilliseconds = Convert.ToInt64(reader.Value);
            return DateTimeOffset.FromUnixTimeMilliseconds(unixTimeMilliseconds).DateTime;
        }

        throw new JsonSerializationException($"Expected integer milliseconds value, got {reader.TokenType}.");
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(DateTime) || objectType == typeof(DateTime?);
    }
}
