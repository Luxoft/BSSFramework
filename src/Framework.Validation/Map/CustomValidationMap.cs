using System;

namespace Framework.Validation;

public abstract class CustomValidationMap : ValidationMapBase
{
    private readonly Framework.Validation.IValidationMap _defaultValidatorMap;


    protected CustomValidationMap(Framework.Core.IDynamicSource extendedValidationData) :
            base(extendedValidationData)
    {
        this._defaultValidatorMap = new Framework.Validation.ValidationMap(extendedValidationData);
    }


    protected abstract IClassValidationMap GetCustomClassMap(Type sourceType);

    protected sealed override IClassValidationMap<TSource> GetInternalClassMap<TSource>()
    {
        return (IClassValidationMap<TSource>)this.GetCustomClassMap(typeof(TSource)) ?? this._defaultValidatorMap.GetClassMap<TSource>();
    }
}
