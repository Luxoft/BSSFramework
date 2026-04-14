namespace Framework.Validation.Attributes._Base;

public abstract class ValidatorAttribute : Attribute, IValidationData
{
    public int OperationContext { get; set; } = int.MaxValue;

    public object? CustomError { get; set; }
}
