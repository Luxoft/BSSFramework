using Framework.Core;

namespace Framework.Validation;

public interface IValidationMap : IServiceProviderContainer
{
    IClassValidationMap GetClassMap(Type type);
}
