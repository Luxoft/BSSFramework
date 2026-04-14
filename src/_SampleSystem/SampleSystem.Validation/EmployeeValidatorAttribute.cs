using Framework.Validation;
using Framework.Validation.Attributes._Base;
using Framework.Validation.Validators;

namespace SampleSystem.Validation;

public sealed class EmployeeValidatorAttribute : ClassValidatorAttribute
{
    public override IClassValidator CreateValidator() => new EmployeeValidator();
}
