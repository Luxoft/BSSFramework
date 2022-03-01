using System.Collections.Generic;

using Framework.Validation;

using SampleSystem.Domain;

namespace SampleSystem.BLL
{
    public partial class SampleSystemValidationMap
    {
        protected override IEnumerable<IPropertyValidator<Employee, long>> GetEmployee_ExternalIdValidators()
        {
            yield return new EmployeeExternalIdValidator();
        }
    }
}
