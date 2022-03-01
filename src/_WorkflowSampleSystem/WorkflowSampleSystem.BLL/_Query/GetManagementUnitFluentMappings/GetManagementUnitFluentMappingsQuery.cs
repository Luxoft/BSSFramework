using MediatR;

namespace WorkflowSampleSystem.BLL._Query.GetManagementUnitFluentMappings
{
    public record GetManagementUnitFluentMappingsQuery : IRequest<GetManagementUnitFluentMappingsResponse[]>;
}
