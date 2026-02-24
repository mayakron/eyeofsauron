using System.Net.NetworkInformation;

namespace EyeOfSauronLibrary.Utilities
{
    internal static class PingUtility
    {
        public static PingResult SendRequest(string destination)
        {
            var result = new PingResult();

            var ping = new Ping();

            var reply = ping.Send(destination);

            result.Status = reply.Status;

            result.Duration = reply.RoundtripTime;

            return result;
        }

        public class PingResult
        {
            public long Duration { get; set; }

            public IPStatus Status { get; set; }
        }
    }
}