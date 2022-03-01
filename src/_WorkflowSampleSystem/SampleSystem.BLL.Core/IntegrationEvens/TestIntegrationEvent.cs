using System;

using Framework.Cap.Abstractions;

namespace SampleSystem.BLL.Core.IntegrationEvens
{
    public record TestIntegrationEvent(Guid CountryId) : IntegrationEvent;
}
