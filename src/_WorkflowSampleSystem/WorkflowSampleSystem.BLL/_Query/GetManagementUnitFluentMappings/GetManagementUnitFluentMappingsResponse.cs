using System;

using Framework.Core;

namespace WorkflowSampleSystem.BLL._Query.GetManagementUnitFluentMappings
{
    public record GetManagementUnitFluentMappingsResponse(Guid Id, string Name, Guid? ParentId, Period Period);
}
