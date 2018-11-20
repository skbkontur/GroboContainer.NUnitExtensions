using System.Collections.Generic;
using System.Reflection;

using NUnit.Framework.Api;
using NUnit.Framework.Interfaces;

namespace SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.ExecutionFailures
{
    public static class TestRunner
    {
        public static Dictionary<string, ITestResult> RunTestClass<T>()
        {
            var testListener = new FailedTestMethodsListener<T>();
            var runner = new NUnitTestAssemblyRunner(new DefaultTestAssemblyBuilder());
            runner.Load(Assembly.GetExecutingAssembly(), new Dictionary<string, object> {{"StopOnError", false}});
            runner.Run(testListener, new ClassFilter<T>());
            // todo (p.vostretsov, 19.10.2018): Need to 'exit' the program to call DomainUnload
            return testListener.TestResults;
        }
    }
}