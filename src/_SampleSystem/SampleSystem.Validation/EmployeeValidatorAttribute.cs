using Framework.Validation.Attributes;
using Framework.Validation.Validators;

namespace SampleSystem.Validation;

public sealed class EmployeeValidatorAttribute : ClassValidatorAttribute
{
    public override IClassValidator CreateValidator() => new EmployeeValidator();
}
