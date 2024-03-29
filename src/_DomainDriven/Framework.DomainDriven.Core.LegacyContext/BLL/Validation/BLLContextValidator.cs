﻿using Framework.Core;
using Framework.Core.Serialization;
using Framework.Validation;

namespace Framework.DomainDriven.BLL;

public class BLLContextValidator<TBLLContext, TOperationContext> : BLLContextContainer<TBLLContext>, IValidator, IServiceProviderContainer
        where TBLLContext : class, IServiceProviderContainer
        where TOperationContext : struct, Enum
{
    private static readonly ISerializer<int, TOperationContext> OperationSerializer = Serializer.GetDefault<int, TOperationContext>();

    private readonly ValidatorCompileCache cache;


    protected BLLContextValidator(TBLLContext context, ValidatorCompileCache cache)
            : base(context)
    {
        if (cache == null) throw new ArgumentNullException(nameof(cache));

        this.cache = cache;
    }


    public virtual ValidationResult GetValidationResult<TSource>(TSource source, TOperationContext operationContext, IValidationState ownerState = null)
    {
        return this.cache.GetValidationResult(new ValidationContextBase<TSource>(this, OperationSerializer.Format(operationContext), source, ownerState));
    }


    ValidationResult IValidator.GetValidationResult<TSource>(TSource source, int operationContext, IValidationState ownerState)
    {
        return this.GetValidationResult(source, OperationSerializer.Parse(operationContext), ownerState);
    }

    #region IExtendedValidationDataContainer Members

    IServiceProvider IServiceProviderContainer.ServiceProvider => this.Context.ServiceProvider;

    #endregion
}


public class BLLContextHandlerValidator<TBLLContext, TOperationContext> : BLLContextValidator<TBLLContext, TOperationContext>
        where TBLLContext : class, IServiceProviderContainer
        where TOperationContext : struct, Enum
{
    private readonly Dictionary<Type, Delegate> handlers = new Dictionary<Type, Delegate>();


    public BLLContextHandlerValidator(TBLLContext context, ValidatorCompileCache cache)
            : base(context, cache)
    {
    }

    public sealed override ValidationResult GetValidationResult<TSource>(TSource source, TOperationContext operationContext, IValidationState ownerState = null)
    {
        return this.GetValidationResult(source, operationContext, ownerState, true);
    }


    protected ValidationResult GetValidationResult<TSource>(TSource source, TOperationContext operationContext, IValidationState ownerState, bool useHandler)
    {
        var del = useHandler ? (Func<TSource, TOperationContext, IValidationState, ValidationResult>)this.handlers.GetValueOrDefault(typeof(TSource)) : null;

        var resultDel = del ?? base.GetValidationResult;

        return resultDel(source, operationContext, ownerState);
    }


    protected void RegisterHandler<TDomainOject>(Func<TDomainOject, TOperationContext, IValidationState, ValidationResult> func)
    {
        if (func == null) throw new ArgumentNullException(nameof(func));

        this.handlers.Add(typeof(TDomainOject), func);
    }
}
