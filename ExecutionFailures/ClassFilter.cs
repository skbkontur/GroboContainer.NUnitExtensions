using System;

using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery.ExecutionFailures
{
    [Serializable]
    public class ClassFilter<T> : TestFilter
    {
        public override TNode AddToXml(TNode parentNode, bool recursive)
        {
            return parentNode.AddElement("class", expectedValue);
        }

        public override bool Match(ITest test)
        {
            return test.IsSuite && !(test is ParameterizedMethodSuite) && test.ClassName == expectedValue;
        }

        private readonly string expectedValue = typeof(T).FullName;
    }
}