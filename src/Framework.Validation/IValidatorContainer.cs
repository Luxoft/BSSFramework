namespace Framework.Validation
{
    public interface IValidatorContainer<out TValidator>
        where TValidator : IValidator
    {
        TValidator Validator { get; }
    }

    public interface IValidatorContainer : IValidatorContainer<IValidator>
    {

    }
}