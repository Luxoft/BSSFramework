using MediatR;

namespace SampleSystem.BLL.Command.CreateClassA;

public record CreateClassAEvent(int Value) : IRequest;