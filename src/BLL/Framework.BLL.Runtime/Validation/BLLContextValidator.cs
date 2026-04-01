using Framework.Core;
using Framework.Core.Serialization;
using Framework.Validation;

namespace Framework.BLL.Validation;

public class BLLContextValidator<TBLLContext, TOperationContext>(TBLLContext context, ValidatorCompileCache cache) : BLLContextContainer<TBLLContext>(context), IValidator, IServiceProviderContainer
        where TBLLContext : class, IServiceProviderContainer
        where TOperationContext : struct, Enum
{
    private static readonly ISerializer<int, TOperationContext> OperationSerializer = Serializer.GetDefault<int, TOperationContext>();

    public virtual ValidationResult GetValidationResult<TSource>(TSource source, TOperationContext operationContext, IValidationState ownerState = null) => cache.GetValidationResult(new ValidationContextBase<TSource>(this, OperationSerializer.Format(operationContext), source, ownerState));

    ValidationResult IValidator.GetValidationResult<TSource>(TSource source, int operationContext, IValidationState ownerState) => this.GetValidationResult(source, OperationSerializer.Parse(operationContext), ownerState);

    #region IExtendedValidationDataContainer Members

    IServiceProvider IServiceProviderContainer.ServiceProvider => this.Context.ServiceProvider;

    #endregion
}
