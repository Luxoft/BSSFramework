using Bss.Platform.Mediation.Abstractions;

namespace SampleSystem.BLL._Command.CreateClassA
{
    public record CreateClassAEvent(int value) : IRequest;
}
