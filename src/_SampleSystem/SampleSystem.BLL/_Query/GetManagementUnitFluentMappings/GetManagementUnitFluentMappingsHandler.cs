using Anch.SecuritySystem;
using Anch.SecuritySystem.Attributes;

using MediatR;

namespace SampleSystem.BLL._Query.GetManagementUnitFluentMappings;

public class GetManagementUnitFluentMappingsHandler([ViewSecurity] IManagementUnitFluentMappingBLL managementUnitFluentMappingBll)
    : IRequestHandler<GetManagementUnitFluentMappingsQuery,
        GetManagementUnitFluentMappingsResponse[]>
{
    public async Task<GetManagementUnitFluentMappingsResponse[]> Handle(
            GetManagementUnitFluentMappingsQuery request,
            CancellationToken ct)
    {
        var result = managementUnitFluentMappingBll.GetUnsecureQueryable()
                                                   .Select(x => new GetManagementUnitFluentMappingsResponse(x.Id, x.Name, x.Parent.Id, x.Period))
                                                   .ToArray();

        return result;
    }
}

