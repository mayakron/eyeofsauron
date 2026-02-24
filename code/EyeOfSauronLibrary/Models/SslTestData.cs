using EyeOfSauronLibrary.Utilities;
using Newtonsoft.Json;
using System;

namespace EyeOfSauronLibrary.Models
{
    public class SslTestData : ITestData
    {
        [JsonProperty("CheckCertificateRevocation")]
        public bool CheckCertificateRevocation { get; set; }

        [JsonProperty("Destination")]
        public string Destination { get; set; }

        [JsonProperty("DisableServerCertificateValidation")]
        public bool DisableServerCertificateValidation { get; set; }

        [JsonProperty("ExpectedMaxDuration")]
        public int ExpectedMaxDuration { get; set; }

        [JsonProperty("ExpectedMinServerCertificateRemainingDays")]
        public int ExpectedMinServerCertificateRemainingDays { get; set; }

        [JsonProperty("Host")]
        public string Host { get; set; }

        [JsonProperty("Port")]
        public int Port { get; set; }

        public string Test()
        {
            var sslResult = SslUtility.SendRequest(this.Destination, this.Port, this.Host, this.DisableServerCertificateValidation, null, System.Security.Authentication.SslProtocols.Tls12, this.CheckCertificateRevocation);

            if (!sslResult.Success)
            {
                return "Failure";
            }

            if (this.ExpectedMinServerCertificateRemainingDays > 0)
            {
                if (sslResult.ServerCertificate != null)
                {
                    var expirationDate = DateTime.Parse(sslResult.ServerCertificate.GetExpirationDateString());

                    if (expirationDate.Subtract(DateTime.Now).TotalDays < this.ExpectedMinServerCertificateRemainingDays)
                    {
                        return $"ServerCertificateExpirationDate-{expirationDate:yyyy-MM-dd-HH-mm-ss}";
                    }
                }
            }

            if (this.ExpectedMaxDuration > 0)
            {
                if (sslResult.Duration > this.ExpectedMaxDuration)
                {
                    return $"Duration-{sslResult.Duration}";
                }
            }

            return null;
        }
    }
}