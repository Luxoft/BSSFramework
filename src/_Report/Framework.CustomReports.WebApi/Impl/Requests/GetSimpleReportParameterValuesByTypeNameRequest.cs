using Framework.DomainDriven.ServiceModel.IAD;

namespace Framework.CustomReports.WebApi;

[AutoRequest]
public class GetSimpleReportParameterValuesByTypeNameRequest
{
    [AutoRequestProperty(OrderIndex = 0)]
    public string typeName;

    [AutoRequestProperty(OrderIndex = 1)]
    public string odataQueryString;
}
