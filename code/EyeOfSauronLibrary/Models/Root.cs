using Newtonsoft.Json;
using System.Collections.Generic;

namespace EyeOfSauronLibrary.Models
{
    public class Root
    {
        [JsonProperty("Sequences")]
        public List<Sequence> Sequences { get; set; }
    }
}