using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using GroboContainer.NUnitExtensions.Impl.TopologicalSorting;

using JetBrains.Annotations;

using NUnit.Framework;

namespace GroboContainer.NUnitExtensions.Impl
{
    public static class ReflectionHelpers
    {
        public static void InvokeWrapperMethod([CanBeNull] MethodInfo wrapperMethod, [NotNull] object testFixture, params object[] @params)
        {
            if (wrapperMethod == null)
                return;
            try
            {
                var result = wrapperMethod.Invoke(testFixture, @params);
                if (wrapperMethod.ReturnType == typeof(Task))
                    (result as Task).GetAwaiter().GetResult();
            }
            catch (TargetInvocationException exception)
            {
                exception.RethrowInnerException();
            }
        }

        [NotNull]
        public static string GetMethodName([NotNull] this MethodInfo test)
        {
            return $"{test.GetFixtureType().FullName}.{test.Name}";
        }

        [NotNull]
        public static Type GetFixtureType([NotNull] this MethodInfo test)
        {
            var fixtureType = test.ReflectedType;
            if (fixtureType == null)
                throw new InvalidOperationException($"test.ReflectedType is null for: {test.Name}");
            return fixtureType;
        }

        [NotNull]
        public static string GetSuiteName([NotNull] this MethodInfo test)
        {
            return suiteNamesCache.GetOrAdd(test.GetFixtureType(), fixtureType =>
                {
                    var suiteNames = GetAttributesForTestFixture<GroboTestSuiteAttribute>(fixtureType).Select(x => x.SuiteName).ToList();
                    var testFixtureAttribute = GetAttributesForType<GroboTestFixtureAttribute>(fixtureType).SingleOrDefault();
                    if (testFixtureAttribute != null)
                        suiteNames.Add(fixtureType.FullName);
                    if (suiteNames.Count > 1)
                        throw new InvalidOperationException($"There are multiple suite names ({string.Join(", ", suiteNames)}) defined for: {test.GetMethodName()}");
                    var suiteName = suiteNames.SingleOrDefault();
                    if (string.IsNullOrEmpty(suiteName))
                        throw new InvalidOperationException($"Suite name is not defined for: {test.GetMethodName()}");
                    return suiteName;
                });
        }

        [NotNull]
        public static List<TestSuiteWrapperAttribute> GetSuiteWrappers([NotNull] this MethodInfo test)
        {
            return suiteWrappersForTest.GetOrAdd(test, GetWrappers<TestSuiteWrapperAttribute>);
        }

        [NotNull]
        public static List<TestMethodWrapperAttribute> GetMethodWrappers([NotNull] this MethodInfo test)
        {
            return methodWrappersForTest.GetOrAdd(test, GetWrappers<TestMethodWrapperAttribute>);
        }

        [CanBeNull]
        public static MethodInfo FindFixtureSetUpMethod([NotNull] this MethodInfo test)
        {
            return fixtureSetUpMethods.GetOrAdd(GetFixtureType(test), FindSingleMethodMarkedWith<GroboTestFixtureSetUpAttribute>);
        }

        [CanBeNull]
        public static MethodInfo FindSetUpMethod([NotNull] this MethodInfo test)
        {
            return setUpMethods.GetOrAdd(GetFixtureType(test), FindSingleMethodMarkedWith<GroboSetUpAttribute>);
        }

        [CanBeNull]
        public static MethodInfo FindTearDownMethod([NotNull] this MethodInfo test)
        {
            return tearDownMethods.GetOrAdd(GetFixtureType(test), FindSingleMethodMarkedWith<GroboTearDownAttribute>);
        }

        [CanBeNull]
        private static MethodInfo FindSingleMethodMarkedWith<TAttribute>([NotNull] Type fixtureType)
        {
            var methods = fixtureType
                          .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                          .Where(x => x.GetCustomAttributes(typeof(TAttribute), true).Any())
                          .ToList();
            if (methods.Count > 1)
                throw new InvalidOperationException($"There are multiple methods marked with {typeof(TAttribute).Name} attribute in: {fixtureType.FullName}");
            return methods.SingleOrDefault();
        }

        public static void EnsureNunitAttributesAbsence([NotNull] this MethodInfo test)
        {
            var fixtureType = GetFixtureType(test);
            if (nunitAttributesPresence.GetOrAdd(fixtureType, HasMethodMarkedWithNUnitAttribute))
                throw new InvalidOperationException($"Prohibited NUnit attributes ({string.Join(", ", forbiddenNunitMethodAttributes.Select(x => x.Name))}) are used in: {fixtureType.FullName}");
        }

        public static bool HasNunitAttributes([NotNull] this MethodInfo test)
        {
            var fixtureType = GetFixtureType(test);
            return nunitAttributesPresence.GetOrAdd(fixtureType, HasMethodMarkedWithNUnitAttribute);
        }

        [NotNull]
        public static List<FieldInfo> GetFieldsForInjection([NotNull] this Type fixtureType)
        {
            return fieldsForInjection.GetOrAdd(fixtureType, DoGetFieldsForInjection);
        }

        [NotNull]
        private static List<FieldInfo> DoGetFieldsForInjection([CanBeNull] Type fixtureType)
        {
            if (fixtureType == null)
                return new List<FieldInfo>();
            return fixtureType
                   .GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                   .Where(x => x.GetCustomAttributes(typeof(InjectedAttribute), false).Any())
                   .Concat(DoGetFieldsForInjection(fixtureType.BaseType))
                   .ToList();
        }

        [NotNull]
        public static List<PropertyInfo> GetPropertiesForInjection([NotNull] this Type fixtureType)
        {
            return propertiesForInjection.GetOrAdd(fixtureType, DoGetPropertiesForInjection);
        }

        [NotNull]
        private static List<PropertyInfo> DoGetPropertiesForInjection([CanBeNull] Type fixtureType)
        {
            if (fixtureType == null)
                return new List<PropertyInfo>();
            return fixtureType
                   .GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                   .Where(x => x.GetCustomAttributes(typeof(InjectedAttribute), false).Any())
                   .Concat(DoGetPropertiesForInjection(fixtureType.BaseType))
                   .ToList();
        }

        private static bool HasMethodMarkedWithNUnitAttribute([NotNull] Type fixtureType)
        {
            return forbiddenNunitMethodAttributes.Any(a => fixtureType.GetMethods(BindingFlags.Instance | BindingFlags.Public).Any(m => IsMarkedWithAttribute(m, a)));
        }

        private static bool IsMarkedWithAttribute([NotNull] MethodInfo method, [NotNull] Type attribute)
        {
            return method.GetCustomAttributes(attribute, true).Any();
        }

        [NotNull]
        private static List<TWrapper> GetWrappers<TWrapper>([NotNull] MethodInfo test) where TWrapper : GroboTestWrapperAttribute
        {
            var fixtureType = GetFixtureType(test);
            var wrappersForFixture = wrappersForFixtureCache.GetOrAdd(fixtureType, GetAttributesForTestSuiteAndFixture<GroboTestWrapperAttribute>);
            var wrappersForMethod = GetAttributesForMethod<TWrapper>(test);
            var visitedWrappers = new HashSet<GroboTestWrapperAttribute>();
            var nodes = new ConcurrentDictionary<GroboTestWrapperAttribute, DependencyNode<GroboTestWrapperAttribute>>();
            var queue = new Queue<GroboTestWrapperAttribute>(wrappersForMethod.Concat(wrappersForFixture));
            while (queue.Count > 0)
            {
                var wrapper = queue.Dequeue();
                if (!visitedWrappers.Add(wrapper))
                    continue;
                nodes.GetOrAdd(wrapper, Node.Create);
                foreach (var wrapperDependency in GetWrapperDependencies(wrapper))
                    queue.Enqueue(wrapperDependency);
            }

            foreach (var wrapper in nodes.Keys)
            {
                var staticDeps = GetWrapperDependencies(wrapper).Select(x => nodes[x]);
                var dynamicDeps = nodes.Where(x => !x.Key.Equals(wrapper) && wrapper.RunAfter(x.Key)).Select(x => x.Value);
                nodes[wrapper].DependsOn(staticDeps.Concat(dynamicDeps).ToArray());
            }

            return nodes.Values.OrderTopologically().Select(x => x.Payload).OfType<TWrapper>().ToList();
        }

        [NotNull]
        private static List<TAttribute> GetAttributesForTestSuiteAndFixture<TAttribute>([NotNull] Type fixtureType)
        {
            var suiteAttribute = GetAttributesForTestFixture<GroboTestSuiteAttributeBase>(fixtureType).Single();
            return GetAttributesForTestFixture<TAttribute>(suiteAttribute.GetType()).Concat(GetAttributesForTestFixture<TAttribute>(fixtureType)).ToList();
        }

        [NotNull]
        private static List<TAttribute> GetAttributesForTestFixture<TAttribute>([NotNull] Type fixtureType)
        {
            return GetAllTypesToSearchForAttributes(fixtureType).SelectMany(GetAttributesForType<TAttribute>).ToList();
        }

        [NotNull]
        private static List<Type> GetAllTypesToSearchForAttributes([CanBeNull] Type type)
        {
            if (type == null)
                return new List<Type>();
            return new[] {type}
                   .Union(type.GetInterfaces())
                   .Union(GetAllTypesToSearchForAttributes(type.BaseType))
                   .Distinct()
                   .ToList();
        }

        [NotNull]
        private static List<TAttribute> GetAttributesForMethod<TAttribute>([NotNull] MethodInfo method)
        {
            return method.GetCustomAttributes(typeof(TAttribute), true).Cast<TAttribute>().ToList();
        }

        [NotNull]
        private static IEnumerable<TAttribute> GetAttributesForType<TAttribute>([NotNull] Type type)
        {
            return type.GetCustomAttributes(typeof(TAttribute), true).Cast<TAttribute>();
        }

        private static List<GroboTestWrapperAttribute> GetWrapperDependencies(GroboTestWrapperAttribute wrapper)
        {
            return wrappersForWrapperCache.GetOrAdd(wrapper.GetType(), x => wrapper.DependsOn().ToList());
        }

        private static readonly Type[] forbiddenNunitMethodAttributes =
            {
                typeof(SetUpAttribute),
                typeof(TearDownAttribute),
                typeof(OneTimeSetUpAttribute),
                typeof(OneTimeTearDownAttribute),
            };

        private static readonly ConcurrentDictionary<Type, string> suiteNamesCache = new ConcurrentDictionary<Type, string>();
        private static readonly ConcurrentDictionary<Type, bool> nunitAttributesPresence = new ConcurrentDictionary<Type, bool>();
        private static readonly ConcurrentDictionary<Type, List<FieldInfo>> fieldsForInjection = new ConcurrentDictionary<Type, List<FieldInfo>>();
        private static readonly ConcurrentDictionary<Type, List<PropertyInfo>> propertiesForInjection = new ConcurrentDictionary<Type, List<PropertyInfo>>();
        private static readonly ConcurrentDictionary<Type, MethodInfo> fixtureSetUpMethods = new ConcurrentDictionary<Type, MethodInfo>();
        private static readonly ConcurrentDictionary<Type, MethodInfo> setUpMethods = new ConcurrentDictionary<Type, MethodInfo>();
        private static readonly ConcurrentDictionary<Type, MethodInfo> tearDownMethods = new ConcurrentDictionary<Type, MethodInfo>();
        private static readonly ConcurrentDictionary<Type, List<GroboTestWrapperAttribute>> wrappersForFixtureCache = new ConcurrentDictionary<Type, List<GroboTestWrapperAttribute>>();
        private static readonly ConcurrentDictionary<Type, List<GroboTestWrapperAttribute>> wrappersForWrapperCache = new ConcurrentDictionary<Type, List<GroboTestWrapperAttribute>>();
        private static readonly ConcurrentDictionary<MethodInfo, List<TestSuiteWrapperAttribute>> suiteWrappersForTest = new ConcurrentDictionary<MethodInfo, List<TestSuiteWrapperAttribute>>();
        private static readonly ConcurrentDictionary<MethodInfo, List<TestMethodWrapperAttribute>> methodWrappersForTest = new ConcurrentDictionary<MethodInfo, List<TestMethodWrapperAttribute>>();
    }
}