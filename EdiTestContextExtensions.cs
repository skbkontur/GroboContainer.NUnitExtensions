using SKBKontur.Catalogue.NUnit.Extensions.EdiTestMachinery;

namespace SKBKontur.Catalogue.Core.Tests.NUnitExtensionTests.EdiTestMachinery
{
    public static class EdiTestContextExtensions
    {
        public static string SuiteName(this IEdiTestContext ctx)
        {
            return ctx.GetContextItem<string>("TestSuiteName");
        }

        public static string TestName(this IEdiTestContext ctx)
        {
            return ctx.GetContextItem<string>("TestName");
        }
    }
}