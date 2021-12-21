using Framework.Core;

namespace Framework.Validation
{
    public interface IExtendedValidationDataContainer
    {
        IDynamicSource ExtendedValidationData { get; }
    }
}