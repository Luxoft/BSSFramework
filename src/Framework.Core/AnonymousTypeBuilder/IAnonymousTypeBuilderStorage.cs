using System.Reflection.Emit;

namespace Framework.Core.AnonymousTypeBuilder;

public interface IAnonymousTypeBuilderStorage
{
    ModuleBuilder ModuleBuilder { get; }
}
