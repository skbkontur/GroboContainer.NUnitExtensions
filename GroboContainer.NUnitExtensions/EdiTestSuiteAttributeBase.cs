using System;

using GroboContainer.NUnitExtensions.Impl;

using JetBrains.Annotations;

using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace GroboContainer.NUnitExtensions
{
    public abstract class EdiTestSuiteAttributeBase : Attribute, ITestAction
    {
        public ActionTargets Targets => ActionTargets.Test;

        public void BeforeTest([NotNull] ITest testDetails)
        {
            EnsureWeAreInMethodContext(testDetails);
            EdiTestAction.BeforeTest(testDetails);
        }

        public void AfterTest([NotNull] ITest testDetails)
        {
            EnsureWeAreInMethodContext(testDetails);
            EdiTestAction.AfterTest(testDetails);
        }

        private static void EnsureWeAreInMethodContext([NotNull] ITest testDetails)
        {
            if (testDetails.IsSuite)
                throw new InvalidOperationException($"IsSuite == true for: {testDetails.FullName}, Type: {testDetails.TestType}");
        }
    }
}