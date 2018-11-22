# GroboContainer.NUnitExtensions

[![NuGet Status](https://img.shields.io/nuget/v/GroboContainer.NUnitExtensions.svg)](https://www.nuget.org/packages/GroboContainer.NUnitExtensions/)
[![Build status](https://ci.appveyor.com/api/projects/status/v4nkma5u54fkoorm?svg=true)](https://ci.appveyor.com/project/skbkontur/grobocontainer-nunitextensions)

NUnit extensions simplifying [DI-container](https://github.com/skbkontur/GroboContainer) management in tests.

## Release Notes

See [CHANGELOG](CHANGELOG.md).

## How to Use

First define GroboContainer configuration for your test suite:
```
public static class GroboTestMachineryContainerConfigurator
{
    public static ContainerConfiguration GetContainerConfiguration(string testSuiteName)
    {
        return new ContainerConfiguration(new[] { Assembly.LoadFrom("A.dll"), Assembly.LoadFrom("B.dll") });
    }
}
```
By convention this must be done in a static method `GetContainerConfiguration` in a class named exactly `GroboTestMachineryContainerConfigurator` which is placed in the same assembly as your tests.

Then define several helper attributes which will hold logic for different aspects of test environment configuration:
```
public class WithX : GroboTestSuiteWrapperAttribute
{
    public override void SetUp(string suiteName, Assembly testAssembly, IEditableGroboTestContext suiteContext)
    {
        suiteContext.Container.Configurator.ForAbstraction<IX>().UseType<X>();
    }
}

[WithX]
public class WithY : GroboTestSuiteWrapperAttribute
{
    public override void SetUp(string suiteName, Assembly testAssembly, IEditableGroboTestContext suiteContext)
    {
        suiteContext.Container.Configurator.ForAbstraction<Y>().UseInstances(new Y("qwerty"));
    }
}

public class WithFixedSeedRandom : GroboTestSuiteWrapperAttribute
{
    public override void SetUp(string suiteName, Assembly testAssembly, IEditableGroboTestContext suiteContext)
    {
        suiteContext.Container.Configurator.ForAbstraction<Random>().UseInstances(new Random(42));
    }
}

public class AndLogTestName : GroboTestMethodWrapperAttribute
{
    public override void SetUp(string testName, IEditableGroboTestContext suiteContext, IEditableGroboTestContext methodContext)
    {
        Console.Out.WriteLine($"Start running test: {testName}");
    }

    public override void TearDown(string testName, IEditableGroboTestContext suiteContext, IEditableGroboTestContext methodContext)
    {
        Console.Out.WriteLine($"Finished running test: {testName}");
    }
}
```

And now you can write your test cases with minimum container configuration boilerplate:
```
[GroboTestSuite("SomeTestSuite"), WithY, WithFixedSeedRandom, AndLogTestName]
public interface ISomeTestSuite { }

public class Part01 : ISomeTestSuite
{
    [Test]
    public void TestCase01()
    {
        x.Foo();
        y.Bar(rng.Next());
    }

    [Injected]
    private readonly IX x;

    [Injected]
    private readonly Y y;

    [Injected]
    private readonly Random rng;
}

public class Part02 : ISomeTestSuite
{
    [Test]
    public void TestCase02()
    {
        x.Foo();
    }

    [Injected]
    private readonly IX x;
}
```
Note that we don't specify `[WithX]` on `ISomeTestSuite` explicitly because `WithY` attribute is itself marked with `[WithX]`.

You can have as many test suites in a single assembly as you like. And each test suite can have its own environment configuration. In particular each test suite will be served with a separate DI-container instance.

Code defined in each inheritor of `GroboTestSuiteWrapperAttribute` is executed once per test suite for which it is applied no matter how many test classes constitute it. On the other hand code defined in inheritor of `GroboTestMethodWrapperAttribute` is executed once per test method.
