using Newtonsoft.Json;

namespace EyeOfSauronLibrary.Models
{
    [JsonConverter(typeof(TestConverter))]
    public class Test
    {
        public ITestData Data { get; set; }

        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Type")]
        public string Type { get; set; }
    }
}