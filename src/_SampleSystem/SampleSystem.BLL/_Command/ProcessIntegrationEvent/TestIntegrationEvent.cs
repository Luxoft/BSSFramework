using Bss.Platform.Events.Abstractions;

namespace SampleSystem.BLL._Command.ProcessIntegrationEvent;

public record TestIntegrationEvent(Guid CountryId) : IIntegrationEvent;
