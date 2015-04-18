﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using JetBrains.Annotations;

using NUnit.Framework;

using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl.TopologicalSorting;
using SKBKontur.Catalogue.Objects;

namespace SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery.Impl
{
    public static class ReflectionHelpers
    {
        [NotNull]
        public static string GetMethodName([NotNull] this MethodInfo test)
        {
            return string.Format("{0}.{1}", test.GetFixtureType().FullName, test.Name);
        }

        [NotNull]
        public static Type GetFixtureType([NotNull] this MethodInfo test)
        {
            var fixtureType = test.ReflectedType;
            if (fixtureType == null)
                throw new InvalidProgramStateException(string.Format("test.ReflectedType is null for: {0}", test.Name));
            return fixtureType;
        }

        [NotNull]
        public static string GetSuiteName([NotNull] this MethodInfo test)
        {
            return suiteNamesCache.GetOrAdd(test.GetFixtureType(), fixtureType =>
                {
                    var suiteNames = GetAttributesForTestFixture<EdiTestSuiteAttribute>(fixtureType).Select(x => x.SuiteName).ToList();
                    var testFixtureAttribute = GetAttributesForType<EdiTestFixtureAttribute>(fixtureType).SingleOrDefault();
                    if (testFixtureAttribute != null)
                        suiteNames.Add(fixtureType.FullName);
                    if (suiteNames.Count > 1)
                        throw new InvalidProgramStateException(string.Format("There are multiple suite names ({0}) defined for: {1}", string.Join(", ", suiteNames), test.GetMethodName()));
                    var suiteName = suiteNames.SingleOrDefault();
                    if (string.IsNullOrEmpty(suiteName))
                        throw new InvalidProgramStateException(string.Format("Suite name is not defined for: {0}", test.GetMethodName()));
                    return suiteName;
                });
        }

        [NotNull]
        public static List<EdiTestSuiteWrapperAttribute> GetSuiteWrappers([NotNull] this MethodInfo test)
        {
            return suiteWrappersForTest.GetOrAdd(test, GetWrappers<EdiTestSuiteWrapperAttribute>);
        }

        [NotNull]
        public static List<EdiTestMethodWrapperAttribute> GetMethodWrappers([NotNull] this MethodInfo test)
        {
            return methodWrappersForTest.GetOrAdd(test, GetWrappers<EdiTestMethodWrapperAttribute>);
        }

        [CanBeNull]
        public static MethodInfo FindFixtureSetUpMethod([NotNull] this MethodInfo test)
        {
            return fixtureSetUpMethods.GetOrAdd(GetFixtureType(test), FindSingleMethodMarkedWith<EdiTestFixtureSetUpAttribute>);
        }

        [CanBeNull]
        public static MethodInfo FindSetUpMethod([NotNull] this MethodInfo test)
        {
            return setUpMethods.GetOrAdd(GetFixtureType(test), FindSingleMethodMarkedWith<EdiSetUpAttribute>);
        }

        [CanBeNull]
        public static MethodInfo FindTearDownMethod([NotNull] this MethodInfo test)
        {
            return tearDownMethods.GetOrAdd(GetFixtureType(test), FindSingleMethodMarkedWith<EdiTearDownAttribute>);
        }

        [CanBeNull]
        private static MethodInfo FindSingleMethodMarkedWith<TAttribute>([NotNull] Type fixtureType)
        {
            var methods = fixtureType
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(x => x.GetCustomAttributes(typeof(TAttribute), true).Any())
                .ToList();
            if (methods.Count > 1)
                throw new InvalidProgramStateException(string.Format("There are multiple methods marked with {0} attribute in: {1}", typeof(TAttribute).Name, fixtureType.FullName));
            return methods.SingleOrDefault();
        }

        public static void EnsureNunitAttributesAbscence([NotNull] this MethodInfo test)
        {
            var fixtureType = GetFixtureType(test);
            if (nunitAttributesPresence.GetOrAdd(fixtureType, HasMethodMarkedWithNUnitAttribute))
                throw new InvalidProgramStateException(string.Format("Prohibited NUnit attributes ({0}) are used in: {1}", string.Join(", ", forbiddenNunitMethodAttributes.Select(x => x.Name)), fixtureType.FullName));
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

        private static bool HasMethodMarkedWithNUnitAttribute([NotNull] Type fixtureType)
        {
            return forbiddenNunitMethodAttributes.Any(a => fixtureType.GetMethods(BindingFlags.Instance | BindingFlags.Public).Any(m => IsMarkedWithAttribute(m, a)));
        }

        private static bool IsMarkedWithAttribute([NotNull] MethodInfo method, [NotNull] Type attribute)
        {
            return method.GetCustomAttributes(attribute, true).Any();
        }

        [NotNull]
        private static List<TWrapper> GetWrappers<TWrapper>([NotNull] MethodInfo test) where TWrapper : EdiTestWrapperAttribute
        {
            var fixtureType = GetFixtureType(test);
            var wrappersForFixture = wrappersForFixtureCache.GetOrAdd(fixtureType, GetAttributesForTestFixture<EdiTestWrapperAttribute>);
            var wrappersForMethod = GetAttributesForMethod<TWrapper>(test);
            var visitedWrappers = new HashSet<EdiTestWrapperAttribute>();
            var nodes = new ConcurrentDictionary<EdiTestWrapperAttribute, DependencyNode<EdiTestWrapperAttribute>>();
            var queue = new Queue<EdiTestWrapperAttribute>(wrappersForMethod.Concat(wrappersForFixture));
            while (queue.Count > 0)
            {
                var wrapper = queue.Dequeue();
                if (!visitedWrappers.Add(wrapper))
                    continue;
                var node = nodes.GetOrAdd(wrapper, Node.Create);
                var wrapperDependencies = wrappersForWrapperCache.GetOrAdd(wrapper.GetType(), x => GetAttributesForType<EdiTestWrapperAttribute>(x).ToList());
                foreach (var wrapperDependency in wrapperDependencies)
                {
                    queue.Enqueue(wrapperDependency);
                    var nodeDependency = nodes.GetOrAdd(wrapperDependency, Node.Create);
                    node.DependsOn(nodeDependency);
                }
            }
            return nodes.Values.OrderTopologically().Select(x => x.Payload).OfType<TWrapper>().ToList();
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
        private static readonly ConcurrentDictionary<Type, MethodInfo> fixtureSetUpMethods = new ConcurrentDictionary<Type, MethodInfo>();
        private static readonly ConcurrentDictionary<Type, MethodInfo> setUpMethods = new ConcurrentDictionary<Type, MethodInfo>();
        private static readonly ConcurrentDictionary<Type, MethodInfo> tearDownMethods = new ConcurrentDictionary<Type, MethodInfo>();
        private static readonly ConcurrentDictionary<Type, List<EdiTestWrapperAttribute>> wrappersForFixtureCache = new ConcurrentDictionary<Type, List<EdiTestWrapperAttribute>>();
        private static readonly ConcurrentDictionary<Type, List<EdiTestWrapperAttribute>> wrappersForWrapperCache = new ConcurrentDictionary<Type, List<EdiTestWrapperAttribute>>();
        private static readonly ConcurrentDictionary<MethodInfo, List<EdiTestSuiteWrapperAttribute>> suiteWrappersForTest = new ConcurrentDictionary<MethodInfo, List<EdiTestSuiteWrapperAttribute>>();
        private static readonly ConcurrentDictionary<MethodInfo, List<EdiTestMethodWrapperAttribute>> methodWrappersForTest = new ConcurrentDictionary<MethodInfo, List<EdiTestMethodWrapperAttribute>>();
    }
}