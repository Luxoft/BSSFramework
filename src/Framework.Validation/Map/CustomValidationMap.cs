namespace Framework.Validation;

public abstract class CustomValidationMap : ValidationMapBase
{
    private readonly IValidationMap _defaultValidatorMap;


    protected CustomValidationMap(IServiceProvider serviceProvider) :
            base(serviceProvider)
    {
        this._defaultValidatorMap = new ValidationMap(serviceProvider);
    }


    protected abstract IClassValidationMap GetCustomClassMap(Type sourceType);

    protected sealed override IClassValidationMap<TSource> GetInternalClassMap<TSource>()
    {
        return (IClassValidationMap<TSource>)this.GetCustomClassMap(typeof(TSource)) ?? this._defaultValidatorMap.GetClassMap<TSource>();
    }
}
