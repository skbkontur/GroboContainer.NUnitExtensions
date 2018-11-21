using System;
using System.Diagnostics.CodeAnalysis;

using FluentAssertions;

using GroboContainer.NUnitExtensions.Impl.TestContext;

using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Tests.ExecutionFailures
{
    [AndB]
    public class AndAPass : EdiTestMethodWrapperAttribute
    {
        public override void SetUp(string testName, IEditableEdiTestContext suiteContext, IEditableEdiTestContext methodContext)
        {
            EdiTestMachineryTrace.Log("AndA.SetUp()");
        }

        public override void TearDown(string testName, IEditableEdiTestContext suiteContext, IEditableEdiTestContext methodContext)
        {
            EdiTestMachineryTrace.Log("AndA.TearDown()");
        }
    }

    [AndB(exceptionInSetUp : true)]
    public class AndASetUpFailure : EdiTestMethodWrapperAttribute
    {
        public override void SetUp(string testName, IEditableEdiTestContext suiteContext, IEditableEdiTestContext methodContext)
        {
            EdiTestMachineryTrace.Log("AndA.SetUp()");
        }

        public override void TearDown(string testName, IEditableEdiTestContext suiteContext, IEditableEdiTestContext methodContext)
        {
            EdiTestMachineryTrace.Log("AndA.TearDown()");
        }
    }

    [AndB(exceptionInTearDown : true)]
    public class AndATearDownFailure : EdiTestMethodWrapperAttribute
    {
        public override void SetUp(string testName, IEditableEdiTestContext suiteContext, IEditableEdiTestContext methodContext)
        {
            EdiTestMachineryTrace.Log("AndA.SetUp()");
        }

        public override void TearDown(string testName, IEditableEdiTestContext suiteContext, IEditableEdiTestContext methodContext)
        {
            EdiTestMachineryTrace.Log("AndA.TearDown()");
        }
    }

    [AndC]
    public class AndB : EdiTestMethodWrapperAttribute
    {
        public AndB(bool exceptionInSetUp = false, bool exceptionInTearDown = false)
        {
            this.exceptionInSetUp = exceptionInSetUp;
            this.exceptionInTearDown = exceptionInTearDown;
        }

        public override void SetUp(string testName, IEditableEdiTestContext suiteContext, IEditableEdiTestContext methodContext)
        {
            EdiTestMachineryTrace.Log("AndB.SetUp()");
            if (exceptionInSetUp)
                throw new InvalidOperationException("Error in AndB.SetUp()");
        }

        public override void TearDown(string testName, IEditableEdiTestContext suiteContext, IEditableEdiTestContext methodContext)
        {
            EdiTestMachineryTrace.Log("AndB.TearDown()");
            if (exceptionInTearDown)
                throw new InvalidOperationException("Error in AndB.TearDown()");
        }

        private readonly bool exceptionInSetUp;
        private readonly bool exceptionInTearDown;
    }

    public class AndC : EdiTestMethodWrapperAttribute
    {
        public override void SetUp(string testName, IEditableEdiTestContext suiteContext, IEditableEdiTestContext methodContext)
        {
            EdiTestMachineryTrace.ClearTrace();
            EdiTestMachineryTrace.Log("AndC.SetUp()");
        }

        public override void TearDown(string testName, IEditableEdiTestContext suiteContext, IEditableEdiTestContext methodContext)
        {
            EdiTestMachineryTrace.Log("AndC.TearDown()");
        }
    }

    [Explicit("Intentionally fails with 'Error in FailureInTest_ExplicitTest.Test()' error")]
    [EdiTestFixture, AndAPass]
    public class FailureInTest_ExplicitTest
    {
        [Test]
        public void Test()
        {
            EdiTestMachineryTrace.Log("FailureInTest_ExplicitTest.Test()");
            throw new InvalidOperationException("Error in FailureInTest_ExplicitTest.Test()");
        }
    }

    [Explicit("Intentionally fails with 'Error in AndB.SetUp()' error")]
    [EdiTestFixture, AndASetUpFailure]
    public class FailureInSetUpTest_ExplicitTest
    {
        [Test]
        public void Test()
        {
            EdiTestMachineryTrace.Log("FailureInSetUpTest_ExplicitTest.Test()");
        }
    }

    [Explicit("Intentionally fails with 'Error in AndB.TearDown()' error")]
    [EdiTestFixture, AndATearDownFailure]
    public class FailureInTearDown_ExplicitTest
    {
        [Test]
        public void Test()
        {
            EdiTestMachineryTrace.Log("FailureInTearDown_ExplicitTest.Test()");
        }
    }

    [Explicit("Intentionally fails with 'GroboContainer.Impl.Exceptions.AmbiguousConstructorException' error")]
    [EdiTestFixture]
    public class FailureInFieldInjection_ExplicitTest
    {
        [EdiTestFixtureSetUp]
        public void TestFixtureSetUp(IEditableEdiTestContext suiteContext)
        {
            EdiTestMachineryTrace.Log("FailureInFieldInjection_ExplicitTest.TestFixtureSetUp()");
        }

        [Test]
        public void Test()
        {
            EdiTestMachineryTrace.Log("FailureInFieldInjection_ExplicitTest.Test()");
        }

#pragma warning disable 169
        [Injected]
        private readonly NonConstructableByContainerClass fieldForWhichContainerIsNotConfigured;
#pragma warning restore 169

        [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
        private class NonConstructableByContainerClass
        {
            public NonConstructableByContainerClass(string fieldForWhichContainerIsNotConfigured)
            {
                this.fieldForWhichContainerIsNotConfigured = fieldForWhichContainerIsNotConfigured;
            }

            [SuppressMessage("ReSharper", "NotAccessedField.Local")]
            private readonly string fieldForWhichContainerIsNotConfigured;
        }
    }

    public class TestFailuresInDifferentPartsTest
    {
        /// <summary>
        ///     Initially SetUps should be called in following order: AndC.SetUp(), AndB.SetUp(), AndA.SetUp()
        ///     TearDowns should be called in following order: AndA.TearDown(), AndB.TearDown(), AndC.TearDown()
        ///     In this test we check that this order persists if we encounter exception in test method
        /// </summary>
        [Test]
        public void TestFailureInTest()
        {
            var testResults = TestRunner.RunTestClass<FailureInTest_ExplicitTest>();
            var result = testResults[nameof(FailureInTest_ExplicitTest.Test)];
            result.Message.Should().Contain("Error in FailureInTest_ExplicitTest.Test()");
            result.Output.Should().Be(@"AndC.SetUp()
AndB.SetUp()
AndA.SetUp()
FailureInTest_ExplicitTest.Test()
AndA.TearDown()
AndB.TearDown()
AndC.TearDown()
");
        }

        /// <summary>
        ///     Exception occurs in AndB.SetUp, so next AndA.SetUp should not be called
        ///     TearDown is not called for AndA, for which SetUp was not called and for AndB, for which SetUp failed with exception
        /// </summary>
        [Test]
        public void TestFailureInSetUp()
        {
            var testResults = TestRunner.RunTestClass<FailureInSetUpTest_ExplicitTest>();
            var result = testResults[nameof(FailureInSetUpTest_ExplicitTest.Test)];
            result.Message.Should().Contain("Error in AndB.SetUp()");
            result.Output.Should().Be(@"AndC.SetUp()
AndB.SetUp()
AndC.TearDown()
");
        }

        /// <summary>
        ///     Exception occurs in AndB.TearDown, but we still need to call corresponding TearDown
        ///     for each EdiTestMethodWrapperAttribute for which SetUp was called
        ///     In this method we check that all necessary TearDowns are called and correct error message is displayed
        /// </summary>
        [Test]
        public void TestFailureInTearDown()
        {
            var testResults = TestRunner.RunTestClass<FailureInTearDown_ExplicitTest>();
            var result = testResults[nameof(FailureInTearDown_ExplicitTest.Test)];
            result.Message.Should().Contain("Error in AndB.TearDown()");
            result.Output.Should().Be(@"AndC.SetUp()
AndB.SetUp()
AndA.SetUp()
FailureInTearDown_ExplicitTest.Test()
AndA.TearDown()
AndB.TearDown()
AndC.TearDown()
");
        }

        [Test]
        public void TestFailureInFieldInjection()
        {
            var testResults = TestRunner.RunTestClass<FailureInFieldInjection_ExplicitTest>();
            var result = testResults[nameof(FailureInFieldInjection_ExplicitTest.Test)];
            result.Message.Should().Contain("GroboContainer.Impl.Exceptions.AmbiguousConstructorException");
            result.Output.Should().Be(@"FailureInFieldInjection_ExplicitTest.TestFixtureSetUp()
");
        }
    }
}