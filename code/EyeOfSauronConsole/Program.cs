using EyeOfSauronLibrary;
using System;

namespace EyeOfSauronConsole
{
    public class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                var directoryPath = args[0];

                var root = TestRunner.Load(directoryPath);

                TestRunner.Schedule(root, new NotificationSender());

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"{ex.GetType()}: {ex.Message}");
            }
        }
    }
}