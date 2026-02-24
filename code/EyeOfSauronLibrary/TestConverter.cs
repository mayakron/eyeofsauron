using EyeOfSauronLibrary.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EyeOfSauronLibrary
{
    public class TestConverter : JsonConverter
    {
        public override bool CanConvert(System.Type objectType)
        {
            return objectType == typeof(Test);
        }

        public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jo = JObject.Load(reader);

            var test = new Test
            {
                Id = (string)jo["Id"],
                Type = (string)jo["Type"]
            };

            var dataToken = jo["Data"];

            if (dataToken != null && dataToken.Type != JTokenType.Null)
            {
                switch (test.Type)
                {
                    case "Http":
                        test.Data = dataToken.ToObject<HttpTestData>(serializer);
                        break;

                    case "Ping":
                        test.Data = dataToken.ToObject<PingTestData>(serializer);
                        break;

                    case "Ssl":
                        test.Data = dataToken.ToObject<SslTestData>(serializer);
                        break;

                    case "Tcp":
                        test.Data = dataToken.ToObject<TcpTestData>(serializer);
                        break;

                    default:
                        test.Data = null;
                        break;
                }
            }

            return test;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var test = (Test)value;

            var jo = new JObject
            {
                ["Id"] = test.Id,
                ["Type"] = test.Type
            };

            if (test.Data != null)
            {
                jo["Data"] = JObject.FromObject(test.Data, serializer);
            }

            jo.WriteTo(writer);
        }
    }
}