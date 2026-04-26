using MediatR;

using Anch.SecuritySystem;

namespace SampleSystem.BLL._Query.GetManagementUnitFluentMappings;

public class GetManagementUnitFluentMappingsHandler(IManagementUnitFluentMappingBLLFactory managementUnitFluentMappingBllFactory)
    : IRequestHandler<GetManagementUnitFluentMappingsQuery,
        GetManagementUnitFluentMappingsResponse[]>
{
    private readonly IManagementUnitFluentMappingBLL managementUnitFluentMappingBll = managementUnitFluentMappingBllFactory.Create(SecurityRule.View);

    public async Task<GetManagementUnitFluentMappingsResponse[]> Handle(
            GetManagementUnitFluentMappingsQuery request,
            CancellationToken cancellationToken)
    {
        var result = this.managementUnitFluentMappingBll.GetUnsecureQueryable()
                         .Select(x => new GetManagementUnitFluentMappingsResponse(x.Id, x.Name, x.Parent.Id, x.Period))
                         .ToArray();

        return result;
    }
}
