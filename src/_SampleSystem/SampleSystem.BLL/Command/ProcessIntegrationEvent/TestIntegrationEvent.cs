using Bss.Platform.Events.Abstractions;

namespace SampleSystem.BLL.Command.ProcessIntegrationEvent;

public record TestIntegrationEvent(Guid CountryId) : IIntegrationEvent;
