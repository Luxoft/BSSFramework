using Framework.SecuritySystem;

using MediatR;

using SampleSystem.Domain;

namespace SampleSystem.BLL._Command.CreateManagementUnitFluentMapping;

public class CreateManagementUnitFluentMappingHandler : IRequestHandler<CreateManagementUnitFluentMappingCommand, Guid>
{
    private readonly IManagementUnitFluentMappingBLL managementUnitFluentMappingBll;

    public CreateManagementUnitFluentMappingHandler(
            IManagementUnitFluentMappingBLLFactory managementUnitFluentMappingBllFactory) =>
            this.managementUnitFluentMappingBll = managementUnitFluentMappingBllFactory.Create(BLLSecurityMode.Disabled);

    public async Task<Guid> Handle(CreateManagementUnitFluentMappingCommand request, CancellationToken cancellationToken)
    {
        var mu = new ManagementUnitFluentMapping { Name = request.Name, Period = request.Period };
        this.managementUnitFluentMappingBll.Save(mu);

        return mu.Id;
    }
}
