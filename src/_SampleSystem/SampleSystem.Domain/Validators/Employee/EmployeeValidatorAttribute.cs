using Framework.Validation;

namespace SampleSystem.Domain.Validators.Employee
{
    public sealed class EmployeeValidatorAttribute : ClassValidatorAttribute
    {
        public override IClassValidator CreateValidator()
        {
            return new EmployeeValidator();
        }
    }
}