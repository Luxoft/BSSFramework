using System;

using Framework.Core;

namespace SampleSystem.BLL._Query.GetManagementUnitFluentMappings
{
    public record GetManagementUnitFluentMappingsResponse(Guid Id, string Name, Guid? ParentId, Period Period);
}
