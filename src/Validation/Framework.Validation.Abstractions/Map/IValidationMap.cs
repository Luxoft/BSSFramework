using Framework.Core;

namespace Framework.Validation.Map;

public interface IValidationMap : IServiceProviderContainer
{
    IClassValidationMap GetClassMap(Type type);
}
