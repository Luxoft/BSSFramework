using System.IO;

using Framework.OData;

namespace Framework.DomainDriven.SerializeMetadata
{
    public interface IODataReportBuilder
    {
        ISelectOperationResult<object> GetDynamicResult(TypeHeader domainTypeHeader, SelectOperation selectOperation);

        Stream GetReport(TypeHeader domainTypeHeader, SelectOperation selectOperation);
    }
}