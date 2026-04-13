using Framework.Core;

namespace Framework.CodeGeneration.BLLGenerator.Fetching;

public interface IFetchPathFactory<in T>
{
    IEnumerable<PropertyPath> Create(Type startDomainType, T rule);
}
