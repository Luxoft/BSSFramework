using Anch.SecuritySystem;

using MediatR;

namespace SampleSystem.BLL._Query.GetEmployees;

public class GetEmployeesHandler(IEmployeeBLLFactory employeeBllFactory) : IRequestHandler<GetEmployeesQuery, GetEmployeesResponse[]>
{
    private readonly IEmployeeBLL employeeBll = employeeBllFactory.Create(SecurityRule.View);

    public async Task<GetEmployeesResponse[]> Handle(GetEmployeesQuery request, CancellationToken ct)
    {
        var result = this.employeeBll.GetUnsecureQueryable().Where(x => x.Active).ToList();

        return result.Select(x => new GetEmployeesResponse(x.Id, x.NameEng.FullName)).ToArray();
    }
}

