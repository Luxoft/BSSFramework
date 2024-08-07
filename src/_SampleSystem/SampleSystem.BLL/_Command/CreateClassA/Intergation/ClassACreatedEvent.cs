using Bss.Platform.Events.Abstractions;

namespace SampleSystem.BLL._Command.CreateClassA.Integration;

public record ClassACreatedEvent(Guid Id) : IIntegrationEvent;
