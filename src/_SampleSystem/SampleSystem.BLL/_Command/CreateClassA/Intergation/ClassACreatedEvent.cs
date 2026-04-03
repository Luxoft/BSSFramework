using Bss.Platform.Events.Abstractions;

namespace SampleSystem.BLL._Command.CreateClassA.Intergation;

public record ClassACreatedEvent(Guid Id) : IIntegrationEvent;
