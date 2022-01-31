using System;

using Framework.DomainDriven.ServiceModel.IAD;

namespace Framework.CustomReports.WebApi;

[AutoRequest]
public class GetReportParameterValuePositionByTypeNameRequest
{
    [AutoRequestProperty(OrderIndex = 0)]
    public string typeName;

    [AutoRequestProperty(OrderIndex = 1)]
    public Guid id;

    [AutoRequestProperty(OrderIndex = 2)]
    public string odataQueryString;
}
