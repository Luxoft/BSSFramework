using Bss.Platform.Events.Abstractions;

namespace SampleSystem.BLL.Command.CreateClassA.Intergation;

public record ClassACreatedEvent(Guid Id) : IIntegrationEvent;
