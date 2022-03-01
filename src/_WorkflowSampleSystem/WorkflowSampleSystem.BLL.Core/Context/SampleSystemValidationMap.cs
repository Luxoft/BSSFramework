using System.Collections.Generic;

using Framework.Validation;

using WorkflowSampleSystem.Domain;

namespace WorkflowSampleSystem.BLL
{
    public partial class WorkflowSampleSystemValidationMap
    {
        protected override IEnumerable<IPropertyValidator<Employee, long>> GetEmployee_ExternalIdValidators()
        {
            yield return new EmployeeExternalIdValidator();
        }
    }
}
