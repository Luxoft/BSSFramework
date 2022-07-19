using Framework.DomainDriven.SerializeMetadata;

namespace Framework.CustomReports.Domain
{
    public class ReportServiceContext<TBLLContext, TSecurityOperationCode> : IServiceMetadataContainer
    {
        public readonly TBLLContext Context;
        public SystemMetadata SystemMetadata => this.SystemMetadataTypeBuilder.SystemMetadata;

        public ISystemMetadataTypeBuilder SystemMetadataTypeBuilder { get; }

        public readonly ISecurityOperationCodeProvider<TSecurityOperationCode> SecurityOperationCodeProvider;

        public ReportServiceContext(TBLLContext context, ISystemMetadataTypeBuilder systemMetadataTypeBuilder, ISecurityOperationCodeProvider<TSecurityOperationCode> securityOperationCodeProvider)
        {
            this.Context = context;

            this.SystemMetadataTypeBuilder = systemMetadataTypeBuilder;

            this.SecurityOperationCodeProvider = securityOperationCodeProvider;
        }
    }
}
