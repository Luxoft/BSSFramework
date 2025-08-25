using CommonFramework;

using Framework.Core;

namespace Framework.Validation;

public abstract class DynamicClassValidator : IDynamicClassValidator
{
    public IClassValidator GetValidator(Type type, IServiceProvider serviceProvider)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

        return new Func<IServiceProvider, IClassValidator>(this.GetValidator<object>)
               .CreateGenericMethod(type)
               .Invoke<IClassValidator>(this, serviceProvider);
    }

    protected abstract IClassValidator GetValidator<TSource>(IServiceProvider serviceProvider);
}
