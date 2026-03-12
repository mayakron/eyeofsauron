using System;

namespace EyeOfSauronLibrary.Models
{
    public interface INotificationSender
    {
        void NotifyTestFailure(Sequence sequence, Test test, Exception ex);

        void NotifyTestFailure(Sequence sequence, Test test, string failureId);

        void NotifyTestSuccess(Sequence sequence, Test test);
    }
}