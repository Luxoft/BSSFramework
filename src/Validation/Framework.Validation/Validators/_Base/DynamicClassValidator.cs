using Anch.Core;

// ReSharper disable once CheckNamespace
namespace Framework.Validation.Validators;

public abstract class DynamicClassValidator : IDynamicClassValidator
{
    public IClassValidator GetValidator(Type type, IServiceProvider serviceProvider)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));
        if (serviceProvider is null) throw new ArgumentNullException(nameof(serviceProvider));

        return new Func<IServiceProvider, IClassValidator>(this.GetValidator<object>)
               .CreateGenericMethod(type)
               .Invoke<IClassValidator>(this, serviceProvider);
    }

    protected abstract IClassValidator GetValidator<TSource>(IServiceProvider serviceProvider);
}

