using System.Collections.Generic;

using GroboContainer.NUnitExtensions.Impl.TestContext;

using NUnit.Framework.Interfaces;

namespace GroboContainer.NUnitExtensions.Impl.SetupAttributes
{
    public class AndGroboSetUpMethodsAttribute : TestMethodWrapperAttribute
    {
        public override IEnumerable<GroboTestWrapperAttribute> DependsOn()
        {
            yield return new WithGroboContainerAttribute();
        }

        public override bool RunAfter(GroboTestWrapperAttribute other)
        {
            return other is GroboTestMethodWrapperAttribute;
        }

        public override void SetUp(ITest test, IEditableGroboTestContext suiteContext, IEditableGroboTestContext methodContext)
        {
            ReflectionHelpers.InvokeWrapperMethod(test.Method.MethodInfo.FindSetUpMethod(), test.Fixture);
        }

        public override void TearDown(ITest test, IEditableGroboTestContext suiteContext, IEditableGroboTestContext methodContext)
        {
            ReflectionHelpers.InvokeWrapperMethod(test.Method.MethodInfo.FindTearDownMethod(), test.Fixture);
        }
    }
}