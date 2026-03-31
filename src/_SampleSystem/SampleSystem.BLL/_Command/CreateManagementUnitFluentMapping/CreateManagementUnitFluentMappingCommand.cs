using Bss.Platform.Mediation.Abstractions;

using Framework.Core;

namespace SampleSystem.BLL._Command.CreateManagementUnitFluentMapping;

public record CreateManagementUnitFluentMappingCommand(string Name, Period Period) : IRequest<Guid>;
