using Bss.Platform.Events.Abstractions;

namespace SampleSystem.BLL.Command.CreateClassA.Integration;

public record ClassACreatedEvent(Guid Id) : IIntegrationEvent;

