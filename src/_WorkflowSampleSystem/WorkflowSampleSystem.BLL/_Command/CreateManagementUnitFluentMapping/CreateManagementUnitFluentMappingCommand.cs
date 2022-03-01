using System;

using Framework.Core;

using MediatR;

namespace WorkflowSampleSystem.BLL._Command.CreateManagementUnitFluentMapping
{
    public record CreateManagementUnitFluentMappingCommand(string Name, Period Period) : IRequest<Guid>;
}
