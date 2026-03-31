using Bss.Platform.Mediation.Abstractions;

namespace SampleSystem.BLL._Query.GetEmployees;

public record GetEmployeesQuery : IRequest<GetEmployeesResponse[]>;
