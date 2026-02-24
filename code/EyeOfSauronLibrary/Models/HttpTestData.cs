using EyeOfSauronLibrary.Utilities;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace EyeOfSauronLibrary.Models
{
    public class HttpTestData : ITestData
    {
        [JsonProperty("Accept")]
        public string Accept { get; set; }

        [JsonProperty("AcceptLanguage")]
        public string AcceptLanguage { get; set; }

        [JsonProperty("Content")]
        public string Content { get; set; }

        [JsonProperty("ContentType")]
        public string ContentType { get; set; }

        [JsonProperty("ExpectedContentRegex")]
        public string ExpectedContentRegex { get; set; }

        [JsonProperty("ExpectedMaxDuration")]
        public int ExpectedMaxDuration { get; set; }

        [JsonProperty("ExpectedMinContentLength")]
        public int ExpectedMinContentLength { get; set; }

        [JsonProperty("ExpectedMinServerCertificateRemainingDays")]
        public int ExpectedMinServerCertificateRemainingDays { get; set; }

        [JsonProperty("ExpectedStatusCode")]
        public int ExpectedStatusCode { get; set; }

        [JsonProperty("Headers")]
        public string Headers { get; set; }

        [JsonProperty("Host")]
        public string Host { get; set; }

        [JsonProperty("Method")]
        public string Method { get; set; }

        [JsonProperty("Referer")]
        public string Referer { get; set; }

        [JsonProperty("Timeout")]
        public int Timeout { get; set; }

        [JsonProperty("Url")]
        public string Url { get; set; }

        [JsonProperty("UserAgent")]
        public string UserAgent { get; set; }

        public string Test()
        {
            var httpResult = HttpUtility.SendRequest(this.Url, this.Method, true, false, false, this.UserAgent, this.Accept, this.AcceptLanguage, this.Referer, this.Host, this.Headers?.Split('|'), !string.IsNullOrEmpty(this.Content) ? Convert.FromBase64String(this.Content) : null, this.ContentType, this.Timeout);

            if (this.ExpectedStatusCode > 0)
            {
                if (this.ExpectedStatusCode != httpResult.StatusCode)
                {
                    return $"StatusCode-{httpResult.StatusCode}";
                }
            }

            if (this.ExpectedMinContentLength > 0)
            {
                if ((httpResult.Content == null) || (httpResult.Content.Length < this.ExpectedMinContentLength))
                {
                    return "Content";
                }
            }

            if (!string.IsNullOrEmpty(this.ExpectedContentRegex))
            {
                if (httpResult.Content == null)
                {
                    return "Content";
                }

                var contentAsText = Encoding.UTF8.GetString(httpResult.Content);

                if (!Regex.IsMatch(contentAsText, this.ExpectedContentRegex))
                {
                    return "Content";
                }
            }

            if (this.ExpectedMinServerCertificateRemainingDays > 0)
            {
                if (httpResult.ServerCertificate != null)
                {
                    var expirationDate = DateTime.Parse(httpResult.ServerCertificate.GetExpirationDateString());

                    if (expirationDate.Subtract(DateTime.Now).TotalDays < this.ExpectedMinServerCertificateRemainingDays)
                    {
                        return $"ServerCertificateExpirationDate-{expirationDate:yyyy-MM-dd-HH-mm-ss}";
                    }
                }
            }

            if (this.ExpectedMaxDuration > 0)
            {
                if (httpResult.Duration > this.ExpectedMaxDuration)
                {
                    return $"Duration-{httpResult.Duration}";
                }
            }

            return null;
        }
    }
}