using System.Reflection;

using ValidatorExpr = System.Collections.Generic.IReadOnlyDictionary<System.CodeDom.CodeExpression, Framework.Validation.IValidationData>;

namespace Framework.CodeGeneration.BLLGenerator.Configuration;

public interface IValidatorGenerator
{
    ValidatorExpr ClassValidators { get; }

    IReadOnlyDictionary<PropertyInfo, ValidatorExpr> PropertyValidators { get; }
}
