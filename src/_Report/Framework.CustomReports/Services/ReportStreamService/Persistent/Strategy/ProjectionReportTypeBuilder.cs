using Framework.Core;

namespace Framework.CustomReports.Services.Persistent.Strategy
{
    internal class ProjectionReportTypeBuilder : AnonymousTypeByPropertyBuilder<TypeMap<TypeMapMember>, TypeMapMember>
    {
        internal static ProjectionReportTypeBuilder Instance = new ProjectionReportTypeBuilder();
        private ProjectionReportTypeBuilder() : base(new AnonymousTypeBuilderStorage("Reports"))
        {

        }
    }
}