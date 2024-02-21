using Framework.SecuritySystem;

using MediatR;

namespace SampleSystem.BLL._Query.GetManagementUnitFluentMappings;

public class GetManagementUnitFluentMappingsHandler : IRequestHandler<GetManagementUnitFluentMappingsQuery,
        GetManagementUnitFluentMappingsResponse[]>
{
    private readonly IManagementUnitFluentMappingBLL managementUnitFluentMappingBll;

    public GetManagementUnitFluentMappingsHandler(
            IManagementUnitFluentMappingBLLFactory managementUnitFluentMappingBllFactory) =>
            this.managementUnitFluentMappingBll = managementUnitFluentMappingBllFactory.Create(BLLSecurityMode.View);

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
