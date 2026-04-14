using Framework.Validation;

using SampleSystem.Domain;

namespace SampleSystem.Validation;

public sealed class EmployeeValidator : IClassValidator<Employee>
{
    public ValidationResult GetValidationResult(IClassValidationContext<Domain.Employee> validationContext)
    {
        var source = validationContext.Source;

        if (source.Pin == 1234)
        {
            return ValidationResult.CreateError("Employee Pin could not be set as '1234'");
        }

        return ValidationResult.Success;
    }
}
