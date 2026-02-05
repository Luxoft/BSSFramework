using Framework.Validation;

using Microsoft.Extensions.DependencyInjection;

using SampleSystem.Domain;

namespace SampleSystem.BLL;

public class EmployeeExternalIdValidator : IPropertyValidator<Employee, long>
{
    public ValidationResult GetValidationResult(IPropertyValidationContext<Employee, long> validationContext)
    {
        var source = validationContext.Source;

        if (source.ExternalId == 1)
        {
            return ValidationResult.Success;
        }

        var context = validationContext.ServiceProvider.GetRequiredService<ISampleSystemBLLContext>();
        var exists = context.Logics.Employee.GetUnsecureQueryable().Any(e => e.ExternalId == source.ExternalId);

        if (exists)
        {
            return ValidationResult.CreateError($"Employee with ExternalId '{source.ExternalId}' already exists.");
        }

        return ValidationResult.Success;
    }
}
