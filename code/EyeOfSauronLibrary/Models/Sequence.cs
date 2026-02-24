using Newtonsoft.Json;
using System.Collections.Generic;

namespace EyeOfSauronLibrary.Models
{
    public class Sequence
    {
        [JsonProperty("Frequency")]
        public int Frequency { get; set; }

        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Tests")]
        public List<Test> Tests { get; set; }
    }
}