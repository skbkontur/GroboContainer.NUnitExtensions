using System;

using JetBrains.Annotations;

using NUnit.Framework;
using NUnit.Framework.Interfaces;

using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl;
using SKBKontur.Catalogue.Objects;

namespace SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery
{
    public abstract class EdiTestSuiteAttributeBase : Attribute, ITestAction
    {
        public ActionTargets Targets { get { return ActionTargets.Test; } }

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
                throw new InvalidProgramStateException($"IsSuite == true for: {testDetails.FullName}, Type: {testDetails.TestType}");
        }
    }
}