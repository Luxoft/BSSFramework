using MediatR;

namespace WorkflowSampleSystem.BLL._Query.GetEmployees
{
    public record GetEmployeesQuery : IRequest<GetEmployeesResponse[]>;
}
