using System;
using System.Collections.Generic;

using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TestContext;

namespace SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery
{
    public static class EdiTestMachineryTrace
    {
        public static void Log(string message, IEditableEdiTestContext context = null)
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