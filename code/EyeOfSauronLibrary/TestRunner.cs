using EyeOfSauronLibrary.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace EyeOfSauronLibrary
{
    public class TestRunner
    {
        public static Root Load(string directoryPath)
        {
            var mainRoot = new Root() { Sequences = new List<Sequence>() };

            foreach (var filePath in Directory.GetFiles(directoryPath, "*.json", SearchOption.AllDirectories))
            {
                var fileRoot = JsonConvert.DeserializeObject<Root>(File.ReadAllText(filePath));

                mainRoot.Sequences.AddRange(fileRoot.Sequences);
            }

            return mainRoot;
        }

        public static void Schedule(Root root, INotificationSender notificationSender)
        {
            foreach (var sequence in root.Sequences)
            {
                new Thread(new ParameterizedThreadStart(Sequence)) { IsBackground = true }.Start(new SequenceThreadData { Sequence = sequence, NotificationSender = notificationSender });
            }
        }

        private static void Sequence(object parameter)
        {
            var sequenceThreadData = (SequenceThreadData)parameter;

            Sequence(sequenceThreadData.Sequence, sequenceThreadData.NotificationSender);
        }

        private static void Sequence(Sequence sequence, INotificationSender notificationSender)
        {
            while (true)
            {
                foreach (var test in sequence.Tests)
                {
                    try
                    {
                        var failureId = test.Data.Test();

                        if (string.IsNullOrEmpty(failureId))
                        {
                            notificationSender.NotifyTestSuccess(sequence, test);
                        }
                        else
                        {
                            notificationSender.NotifyTestFailure(sequence, test, failureId);

                            break;
                        }
                    }
                    catch (ThreadAbortException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        notificationSender.NotifyTestFailure(sequence, test, ex);

                        break;
                    }
                }

                Thread.Sleep(sequence.Frequency);
            }
        }
    }
}