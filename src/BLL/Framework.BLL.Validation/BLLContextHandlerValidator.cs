using Framework.Core;
using Framework.Validation;
using Framework.Validation.Map;

namespace Framework.BLL.Validation;

public class BLLContextHandlerValidator<TBLLContext, TOperationContext>(TBLLContext context, ValidatorCompileCache cache)
    : BLLContextValidator<TBLLContext, TOperationContext>(context, cache)
    where TBLLContext : class, IServiceProviderContainer
    where TOperationContext : struct, Enum
{
    private readonly Dictionary<Type, Delegate> handlers = [];

    public sealed override ValidationResult GetValidationResult<TSource>(TSource source, TOperationContext operationContext, IValidationState ownerState = null) => this.GetValidationResult(source, operationContext, ownerState, true);

    protected ValidationResult GetValidationResult<TSource>(TSource source, TOperationContext operationContext, IValidationState ownerState, bool useHandler)
    {
        var del = useHandler ? (Func<TSource, TOperationContext, IValidationState, ValidationResult>)this.handlers.GetValueOrDefault(typeof(TSource)) : null;

        var resultDel = del ?? base.GetValidationResult;

        return resultDel(source, operationContext, ownerState);
    }


    protected void RegisterHandler<TDomainObject>(Func<TDomainObject, TOperationContext, IValidationState, ValidationResult> func) => this.handlers.Add(typeof(TDomainObject), func);
}
