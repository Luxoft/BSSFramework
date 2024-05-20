using Framework.SecuritySystem;

using MediatR;

using SampleSystem.Domain;

namespace SampleSystem.BLL._Command.CreateManagementUnitFluentMapping;

public class CreateManagementUnitFluentMappingHandler(IManagementUnitFluentMappingBLLFactory managementUnitFluentMappingBllFactory)
    : IRequestHandler<CreateManagementUnitFluentMappingCommand, Guid>
{
    public async Task<Guid> Handle(CreateManagementUnitFluentMappingCommand request, CancellationToken cancellationToken)
    {
        var mu = new ManagementUnitFluentMapping { Name = request.Name, Period = request.Period };
        managementUnitFluentMappingBllFactory.Create(SecurityRule.Disabled).Save(mu);

        return mu.Id;
    }
}
