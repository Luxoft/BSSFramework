using System;

namespace Framework.Validation
{
    public interface IValidationMap : IExtendedValidationDataContainer
    {
        IClassValidationMap GetClassMap(Type type);
    }
}