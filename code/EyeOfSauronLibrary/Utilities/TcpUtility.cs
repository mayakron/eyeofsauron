using System;
using System.Diagnostics;
using System.Net.Sockets;

namespace EyeOfSauronLibrary.Utilities
{
    internal static class TcpUtility
    {
        public static TcpResult SendRequest(string destination, int port)
        {
            var result = new TcpResult();

            using (var tcpClient = new TcpClient())
            {
                try
                {
                    var stopwatch = new Stopwatch();

                    stopwatch.Start();

                    tcpClient.Connect(destination, port);

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

        public class TcpResult
        {
            public long Duration { get; set; }

            public Exception Exception { get; set; }

            public bool Success { get; set; }
        }
    }
}