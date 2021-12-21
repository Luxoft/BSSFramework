using MediatR;

namespace SampleSystem.BLL._Query.GetManagementUnitFluentMappings
{
    public record GetManagementUnitFluentMappingsQuery : IRequest<GetManagementUnitFluentMappingsResponse[]>;
}
