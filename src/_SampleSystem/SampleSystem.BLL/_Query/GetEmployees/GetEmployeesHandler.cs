using MediatR;

using Anch.SecuritySystem;

namespace SampleSystem.BLL._Query.GetEmployees;

public class GetEmployeesHandler(IEmployeeBLLFactory employeeBllFactory) : IRequestHandler<GetEmployeesQuery, GetEmployeesResponse[]>
{
    private readonly IEmployeeBLL employeeBll = employeeBllFactory.Create(SecurityRule.View);

    public async Task<GetEmployeesResponse[]> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
    {
        var result = this.employeeBll.GetUnsecureQueryable().Where(x => x.Active).ToList();

        return result.Select(x => new GetEmployeesResponse(x.Id, x.NameEng.FullName)).ToArray();
    }
}
