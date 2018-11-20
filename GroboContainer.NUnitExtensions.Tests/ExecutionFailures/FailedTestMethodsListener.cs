using System.Collections.Generic;

using NUnit.Framework.Interfaces;

namespace SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.ExecutionFailures
{
    public class FailedTestMethodsListener<T> : ITestListener
    {
        public void TestStarted(ITest test)
        {
        }

        public void TestFinished(ITestResult result)
        {
            if (result.ResultState.Status == TestStatus.Failed && result.ResultState.Site != FailureSite.Child && result.Test.TypeInfo?.Type == typeof(T))
                TestResults[result.Name] = result;
        }

        public void TestOutput(TestOutput output)
        {
        }

        public void SendMessage(TestMessage message)
        {
        }

        public Dictionary<string, ITestResult> TestResults { get; } = new Dictionary<string, ITestResult>();
    }
}