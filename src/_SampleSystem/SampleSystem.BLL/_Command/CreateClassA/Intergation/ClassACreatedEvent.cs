using Framework.Cap.Abstractions;

namespace SampleSystem.BLL._Command.CreateClassA.Intergation
{
    public record ClassACreatedEvent(Guid Id) : IntegrationEvent;
}
