using Bss.Platform.Events.Abstractions;

namespace SampleSystem.BLL.Core.IntegrationEvens;

public record TestIntegrationEvent(Guid CountryId) : IIntegrationEvent;
