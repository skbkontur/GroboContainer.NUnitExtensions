using System;
using System.Reflection;
using System.Runtime.ExceptionServices;

using JetBrains.Annotations;

namespace GroboContainer.NUnitExtensions.Impl
{
    public static class ExceptionExtensions
    {
        public static void Rethrow([NotNull] this Exception exception)
        {
            ExceptionDispatchInfo.Capture(exception).Throw();
        }

        public static void RethrowInnerException([NotNull] this TargetInvocationException exception)
        {
            exception.InnerException?.Rethrow();
            throw exception;
        }
    }
}