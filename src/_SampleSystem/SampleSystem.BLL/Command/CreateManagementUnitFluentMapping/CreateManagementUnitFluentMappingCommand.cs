using Framework.Core;

using MediatR;

namespace SampleSystem.BLL.Command.CreateManagementUnitFluentMapping;

public record CreateManagementUnitFluentMappingCommand(string Name, Period Period) : IRequest<Guid>;
