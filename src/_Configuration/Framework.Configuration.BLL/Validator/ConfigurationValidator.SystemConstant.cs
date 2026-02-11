using Framework.Configuration.Domain;
using Framework.Core.Serialization;
using Framework.Validation;

namespace Framework.Configuration.BLL;

public partial class ConfigurationValidator
{
    protected override ValidationResult GetSystemConstantValidationResult(SystemConstant source, OperationContextBase operationContext, IValidationState ownerState)
    {
        var baseResult = base.GetSystemConstantValidationResult(source, operationContext, ownerState);

        if (baseResult.HasErrors)
        {
            return baseResult;
        }
        else
        {
            return ValidationResult.TryCatch(() =>
                                             {
                                                 var domainType = this.Context.ComplexDomainTypeResolver.Resolve(source.Type);

                                                 try
                                                 {
                                                     this.Context.SystemConstantSerializerFactory.Validate(domainType, source.Value);
                                                 }
                                                 catch (Exception ex)
                                                 {
                                                     throw new ValidationException($"Can't convert value \"{source.Value}\" to type \"{source.Type.FullTypeName}\"", ex);
                                                 }
                                             });
        }
    }
}
