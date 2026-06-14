using Anch.SecuritySystem.Attributes;

using MediatR;

namespace SampleSystem.BLL._Query.GetEmployees;

public class GetEmployeesHandler([ViewSecurity] IEmployeeBLL employeeBll) : IRequestHandler<GetEmployeesQuery, GetEmployeesResponse[]>
{
    public async Task<GetEmployeesResponse[]> Handle(GetEmployeesQuery request, CancellationToken ct)
    {
        var result = employeeBll.GetUnsecureQueryable().Where(x => x.Active).ToList();

        return result.Select(x => new GetEmployeesResponse(x.Id, x.NameEng.FullName)).ToArray();
    }
}

