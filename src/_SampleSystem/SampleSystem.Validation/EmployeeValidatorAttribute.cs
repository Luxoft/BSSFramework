using Framework.Validation;
using Framework.Validation.Attributes._Base;

namespace SampleSystem.Validation;

public sealed class EmployeeValidatorAttribute : ClassValidatorAttribute
{
    public override IClassValidator CreateValidator() => new EmployeeValidator();
}
