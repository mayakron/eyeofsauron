using EyeOfSauronLibrary.Utilities;
using Newtonsoft.Json;

namespace EyeOfSauronLibrary.Models
{
    public class PingTestData : ITestData
    {
        [JsonProperty("Destination")]
        public string Destination { get; set; }

        [JsonProperty("ExpectedMaxDuration")]
        public int ExpectedMaxDuration { get; set; }

        public string Test()
        {
            var pingResult = PingUtility.SendRequest(this.Destination);

            if (pingResult.Status != System.Net.NetworkInformation.IPStatus.Success)
            {
                return pingResult.Status.ToString();
            }

            if (this.ExpectedMaxDuration > 0)
            {
                if (pingResult.Duration > this.ExpectedMaxDuration)
                {
                    return $"Duration-{pingResult.Duration}";
                }
            }

            return null;
        }
    }
}