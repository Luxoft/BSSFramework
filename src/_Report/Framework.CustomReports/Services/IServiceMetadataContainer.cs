using Framework.DomainDriven.SerializeMetadata;

namespace Framework.CustomReports.Domain
{
    public interface IServiceMetadataContainer
    {
        SystemMetadata SystemMetadata { get; }
    }
}