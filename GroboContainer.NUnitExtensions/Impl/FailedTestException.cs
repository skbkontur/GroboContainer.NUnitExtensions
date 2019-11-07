using System;

namespace GroboContainer.NUnitExtensions.Impl
{
    public class FailedTestException : Exception
    {
        public FailedTestException(string message, string stackTrace)
        {
            Message = message;
            StackTrace = stackTrace;
        }

        public override string Message { get; }

        public override string StackTrace { get; }
    }
}