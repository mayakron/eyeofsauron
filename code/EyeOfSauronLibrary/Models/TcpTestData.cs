using EyeOfSauronLibrary.Utilities;
using Newtonsoft.Json;

namespace EyeOfSauronLibrary.Models
{
    public class TcpTestData : ITestData
    {
        [JsonProperty("Destination")]
        public string Destination { get; set; }

        [JsonProperty("ExpectedMaxDuration")]
        public int ExpectedMaxDuration { get; set; }

        [JsonProperty("Port")]
        public int Port { get; set; }

        public string Test()
        {
            var tcpResult = TcpUtility.SendRequest(this.Destination, this.Port);

            if (!tcpResult.Success)
            {
                return "Failure";
            }

            if (this.ExpectedMaxDuration > 0)
            {
                if (tcpResult.Duration > this.ExpectedMaxDuration)
                {
                    return $"Duration-{tcpResult.Duration}";
                }
            }

            return null;
        }
    }
}