using Framework.DomainDriven.ServiceModelGenerator;

namespace Framework.CustomReports.Generation.Facade
{
    public static class CustomReportMethodIdentityType
    {
        public static readonly MethodIdentityType GetCustomReport = new MethodIdentityType(() => GetCustomReport);
    }
}