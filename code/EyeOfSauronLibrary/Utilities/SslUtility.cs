using System;
using System.Diagnostics;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace EyeOfSauronLibrary.Utilities
{
    internal static class SslUtility
    {
        public static SslResult SendRequest(string destination, int port, string host, bool disableServerCertificateValidation, X509CertificateCollection clientCertificates, SslProtocols enabledSslProtocols, bool checkCertificateRevocation)
        {
            var result = new SslResult();

            using (var tcpClient = new TcpClient())
            {
                try
                {
                    var stopwatch = new Stopwatch();

                    stopwatch.Start();

                    tcpClient.Connect(destination, port);

                    using (var sslStream = disableServerCertificateValidation ? new SslStream(tcpClient.GetStream(), false, new RemoteCertificateValidationCallback(ValidateServerCertificateAlways)) : new SslStream(tcpClient.GetStream(), false))
                    {
                        sslStream.AuthenticateAsClient(host, clientCertificates, enabledSslProtocols, checkCertificateRevocation);

                        result.ServerCertificate = sslStream.RemoteCertificate;
                    }

                    stopwatch.Stop();

                    result.Success = true;

                    result.Duration = stopwatch.ElapsedMilliseconds;
                }
                catch (Exception ex)
                {
                    result.Success = false;

                    result.Exception = ex;
                }
            }

            return result;
        }

        private static bool ValidateServerCertificateAlways(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public class SslResult
        {
            public long Duration { get; set; }

            public Exception Exception { get; set; }

            public X509Certificate ServerCertificate { get; set; }

            public bool Success { get; set; }
        }
    }
}