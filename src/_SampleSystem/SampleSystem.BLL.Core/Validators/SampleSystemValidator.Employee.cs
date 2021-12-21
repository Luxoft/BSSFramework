using System.Linq;

using Framework.Validation;

using SampleSystem.Domain;

namespace SampleSystem.BLL
{
    public partial class SampleSystemValidator
    {
        protected override ValidationResult GetEmployeeValidationResult(Employee source, SampleSystemOperationContext operationContext, IValidationState ownerState)
        {
            var exists = this.Context.Logics.Employee.GetUnsecureQueryable().Any(e => e.Id != source.Id && e.Pin == source.Pin);

            if (exists)
            {
                return ValidationResult.CreateError($"Employee with Pin '{source.Pin}' already exists.");
            }

            return base.GetEmployeeValidationResult(source, operationContext, ownerState);
        }
    }
}