using Framework.Validation;
using Framework.Validation.Validators;

using SampleSystem.Domain.Employee;

namespace SampleSystem.Validation;

public sealed class EmployeeValidator : IClassValidator<Employee>
{
    public const int ForbiddenPin = 1234;

    public ValidationResult GetValidationResult(IClassValidationContext<Employee> validationContext)
    {
        var source = validationContext.Source;

        if (source.Pin == EmployeeValidator.ForbiddenPin)
        {
            return ValidationResult.CreateError($"Employee Pin could not be set as '{EmployeeValidator.ForbiddenPin}'");
        }

        return ValidationResult.Success;
    }
}
