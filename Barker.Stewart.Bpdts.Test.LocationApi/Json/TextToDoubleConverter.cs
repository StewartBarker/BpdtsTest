namespace Barker.Stewart.Bpdts.Test.LocationApi.Json
{
    using System;
    using System.Buffers;
    using System.Buffers.Text;
    using System.Linq;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public class TextToDoubleConverter : JsonConverter<double>
    {
        public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                if (reader.TryGetDouble(out double valueAsDouble))
                { 
                    return valueAsDouble;
                }
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                ReadOnlySpan<byte> span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;
                if (Utf8Parser.TryParse(span, out long number, out int bytesConsumed) && span.Length == bytesConsumed)
                    return number;

                if (double.TryParse(reader.GetString(), out var dbl))
                    return dbl;

                if (Int64.TryParse(reader.GetString(), out number))
                    return number;
            }

            return reader.GetInt64();
        }

        public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }
}
