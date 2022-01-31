using System;
using System.Collections.Generic;

using Framework.DomainDriven.ServiceModel.IAD;

namespace Framework.CustomReports.WebApi;

[AutoRequest]
public class GetReportParameterValuePositionsByTypeNameRequest
{
    [AutoRequestProperty(OrderIndex = 0)]
    public string typeName;

    [AutoRequestProperty(OrderIndex = 1)]
    public IEnumerable<Guid> ids;

    [AutoRequestProperty(OrderIndex = 2)]
    public string odataQueryString;
}
