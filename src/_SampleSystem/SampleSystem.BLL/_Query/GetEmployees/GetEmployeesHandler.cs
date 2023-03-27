using Framework.SecuritySystem;

using MediatR;

namespace SampleSystem.BLL._Query.GetEmployees;

public class GetEmployeesHandler : IRequestHandler<GetEmployeesQuery, GetEmployeesResponse[]>
{
    private readonly IEmployeeBLL employeeBll;

    public GetEmployeesHandler(IEmployeeBLLFactory employeeBllFactory) =>
            this.employeeBll = employeeBllFactory.Create(BLLSecurityMode.View);

    public Task<GetEmployeesResponse[]> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
    {
        var result = this.employeeBll.GetUnsecureQueryable().Where(x => x.Active).ToList();

        return Task.FromResult(result.Select(x => new GetEmployeesResponse(x.Id, x.NameEng.FullName)).ToArray());
    }
}
