// ReSharper disable once CheckNamespace
namespace Framework.Validation.Attributes;

public abstract class ValidatorAttribute : Attribute, IValidationData
{
    public int OperationContext { get; set; } = int.MaxValue;

    public object? CustomError { get; set; }
}
