using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace EyeOfSauronLibrary.Utilities
{
    internal static class HttpUtility
    {
        public const int DefaultTimeoutInMilliseconds = 60000;

        static HttpUtility()
        {
            ServicePointManager.DefaultConnectionLimit = 100;

            // ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            // ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => { return true; };

            // ServicePointManager.CheckCertificateRevocationList = false;

            // ServicePointManager.Expect100Continue = false;
        }

        public static HttpResult SendRequest(string url, string method, bool keepAlive, bool allowCompression, bool allowCaching, string userAgent, string accept, string acceptLanguage, string referer, string host, string[] headers, byte[] content, string contentType, int timeoutInMilliseconds = DefaultTimeoutInMilliseconds)
        {
            var result = new HttpResult();

            var request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = method;

            request.KeepAlive = keepAlive;

            request.AllowAutoRedirect = false;

            request.AutomaticDecompression = allowCompression ? DecompressionMethods.GZip : DecompressionMethods.None;

            if (!allowCaching)
            {
                request.Headers.Add("Cache-Control: no-cache, no-store, must-revalidate");
                request.Headers.Add("Pragma: no-cache");
                request.Headers.Add("Expires: 0");
            }

            if (userAgent != null)
            {
                request.UserAgent = userAgent;
            }

            if (accept != null)
            {
                request.Accept = accept;
            }

            if (acceptLanguage != null)
            {
                request.Headers.Add("Accept-Language: " + acceptLanguage);
            }

            if (referer != null)
            {
                request.Referer = referer;
            }

            if (host != null)
            {
                request.Host = host;
            }

            if (headers != null)
            {
                foreach (string header in headers)
                {
                    request.Headers.Add(header);
                }
            }

            if (content != null)
            {
                request.ContentType = contentType;

                request.ContentLength = content.Length;

                request.GetRequestStream().Write(content, 0, content.Length);
            }

            request.Timeout = timeoutInMilliseconds;

            var stopwatch = new Stopwatch();

            stopwatch.Start();

            HttpWebResponse response;

            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                if (ex.Response == null)
                {
                    throw;
                }

                var exResponse = ex.Response as HttpWebResponse;

                if (exResponse == null)
                {
                    throw;
                }

                response = exResponse;
            }

            stopwatch.Stop();

            if (request.ServicePoint != null)
            {
                result.ServerCertificate = request.ServicePoint.Certificate;
            }

            result.StatusCode = (int)response.StatusCode;

            result.Headers = response.Headers;

            using (var responseMemoryStream = new MemoryStream())
            {
                using (var responseStream = response.GetResponseStream())
                {
                    responseStream.CopyTo(responseMemoryStream);
                }

                result.Content = responseMemoryStream.ToArray();
            }

            result.Duration = stopwatch.ElapsedMilliseconds;

            return result;
        }

        public class HttpResult
        {
            public byte[] Content { get; set; }

            public long Duration { get; set; }

            public WebHeaderCollection Headers { get; set; }

            public X509Certificate ServerCertificate { get; set; }

            public int StatusCode { get; set; }
        }
    }
}