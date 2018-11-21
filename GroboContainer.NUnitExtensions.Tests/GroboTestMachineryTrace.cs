using System;
using System.Collections.Generic;

using GroboContainer.NUnitExtensions.Impl.TestContext;

namespace GroboContainer.NUnitExtensions.Tests
{
    public static class GroboTestMachineryTrace
    {
        public static void Log(string message, IEditableGroboTestContext context = null)
        {
            TraceLines.Add(message);
            Console.Out.WriteLine(message);
            if (context != null)
                Console.Out.WriteLine("Context: {0}", context);
        }

        public static void ClearTrace()
        {
            TraceLines.Clear();
        }

        public static readonly List<string> TraceLines = new List<string>();
    }
}