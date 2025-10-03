using System.Reflection.Emit;

namespace Framework.Core;

public interface IAnonymousTypeBuilderStorage
{
    ModuleBuilder ModuleBuilder { get; }
}
