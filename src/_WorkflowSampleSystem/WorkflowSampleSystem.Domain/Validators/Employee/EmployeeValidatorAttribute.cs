using Framework.Validation;

namespace WorkflowSampleSystem.Domain.Validators.Employee
{
    public sealed class EmployeeValidatorAttribute : ClassValidatorAttribute
    {
        public override IClassValidator CreateValidator()
        {
            return new EmployeeValidator();
        }
    }
}