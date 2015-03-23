using System;

using JetBrains.Annotations;

using NUnit.Framework;

using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl;
using SKBKontur.Catalogue.Objects;

namespace SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery
{
    public abstract class EdiTestSuiteAttributeBase : Attribute, ITestAction
    {
        public ActionTargets Targets { get { return ActionTargets.Test; } }

        public void BeforeTest([NotNull] TestDetails testDetails)
        {
            EnsureWeAreInMethodContext(testDetails);
            EdiTestAction.BeforeTest(testDetails);
        }

        public void AfterTest([NotNull] TestDetails testDetails)
        {
            EnsureWeAreInMethodContext(testDetails);
            EdiTestAction.AfterTest(testDetails);
        }

        private static void EnsureWeAreInMethodContext([NotNull] TestDetails testDetails)
        {
            if(testDetails.IsSuite)
                throw new InvalidProgramStateException(string.Format("IsSuite == true for: {0}, Type: {1}", testDetails.FullName, testDetails.Type));
        }
    }
}