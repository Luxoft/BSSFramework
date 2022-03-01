using MediatR;

namespace SampleSystem.BLL._Query.GetEmployees
{
    public record GetEmployeesQuery : IRequest<GetEmployeesResponse[]>;
}
