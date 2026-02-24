using EyeOfSauronLibrary.Models;
using System;

namespace EyeOfSauronLibrary.Utilities
{
    internal static class NotificationUtility
    {
        public static void NotifyTestFailure(Sequence sequence, Test test, Exception ex)
        {
            var created = DateTime.UtcNow;
            var message = $"{created:yyyy-MM-dd HH:mm:ss}\t{sequence.Id}\t{test.Id}\tKO\tException\t{ex.GetType()}: {ex.Message}";

            Console.Error.WriteLine(message);

            Console.WriteLine(message);
        }

        public static void NotifyTestFailure(Sequence sequence, Test test, string failureId)
        {
            var created = DateTime.UtcNow;
            var message = $"{created:yyyy-MM-dd HH:mm:ss}\t{sequence.Id}\t{test.Id}\tKO\t{failureId}";

            Console.Error.WriteLine(message);

            Console.WriteLine(message);
        }

        public static void NotifyTestSuccess(Sequence sequence, Test test)
        {
            var created = DateTime.UtcNow;
            var message = $"{created:yyyy-MM-dd HH:mm:ss}\t{sequence.Id}\t{test.Id}\tOK";

            Console.WriteLine(message);
        }
    }
}