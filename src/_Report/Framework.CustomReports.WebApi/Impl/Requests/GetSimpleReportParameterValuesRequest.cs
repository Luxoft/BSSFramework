using Framework.Configuration.Generated.DTO;
using Framework.DomainDriven.ServiceModel.IAD;

namespace Framework.CustomReports.WebApi;

[AutoRequest]
public class GetSimpleReportParameterValuesRequest
{
    [AutoRequestProperty(OrderIndex = 0)]
    public ReportParameterIdentityDTO identity;

    [AutoRequestProperty(OrderIndex = 1)]
    public string odataQueryString;
}
