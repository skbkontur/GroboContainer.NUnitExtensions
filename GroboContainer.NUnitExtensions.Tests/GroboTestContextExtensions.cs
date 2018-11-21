namespace GroboContainer.NUnitExtensions.Tests
{
    public static class GroboTestContextExtensions
    {
        public static string SuiteName(this IGroboTestContext ctx)
        {
            return ctx.GetContextItem<string>("TestSuiteName");
        }

        public static string TestName(this IGroboTestContext ctx)
        {
            return ctx.GetContextItem<string>("TestName");
        }
    }
}