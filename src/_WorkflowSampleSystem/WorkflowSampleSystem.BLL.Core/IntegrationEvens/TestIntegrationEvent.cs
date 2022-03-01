using System;

using Framework.Cap.Abstractions;

namespace WorkflowSampleSystem.BLL.Core.IntegrationEvens
{
    public record TestIntegrationEvent(Guid CountryId) : IntegrationEvent;
}
