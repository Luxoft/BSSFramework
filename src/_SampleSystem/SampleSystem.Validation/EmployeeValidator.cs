using Framework.Validation;
using Framework.Validation.Validators;

using SampleSystem.Domain;
using SampleSystem.Domain.Employee;

namespace SampleSystem.Validation;

public sealed class EmployeeValidator : IClassValidator<Employee>
{
    public ValidationResult GetValidationResult(IClassValidationContext<Employee> validationContext)
    {
        var source = validationContext.Source;

        if (source.Pin == 1234)
        {
            return ValidationResult.CreateError("Employee Pin could not be set as '1234'");
        }

        return ValidationResult.Success;
    }
}
