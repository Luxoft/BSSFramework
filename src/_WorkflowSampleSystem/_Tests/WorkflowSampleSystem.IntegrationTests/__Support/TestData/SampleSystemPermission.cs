using System;
using System.Collections.Generic;

using Automation.Utils;

using WorkflowSampleSystem.Generated.DTO;
using WorkflowSampleSystem.IntegrationTests.__Support.Utils;

namespace WorkflowSampleSystem.IntegrationTests.__Support.TestData
{
    public class WorkflowSampleSystemPermission : IPermissionDefinition
    {
        public WorkflowSampleSystemPermission()
        {
        }

        public WorkflowSampleSystemPermission(BusinessRole role)
        {
            this.Role = role;
        }

        public WorkflowSampleSystemPermission(
            BusinessRole role,
            BusinessUnitIdentityDTO? businessUnit,
            ManagementUnitIdentityDTO? managementUnit,
            LocationIdentityDTO? location)
        {
            this.Role = role;
            this.BusinessUnit = businessUnit;
            this.ManagementUnit = managementUnit;
            this.Location = location;
        }

        public BusinessRole Role { get; set; }

        public ManagementUnitIdentityDTO? ManagementUnit { get; set; }

        public BusinessUnitIdentityDTO? BusinessUnit { get; set; }

        public LocationIdentityDTO? Location { get; set; }

        public IEnumerable<Tuple<string, Guid>> GetEntities()
        {
            if (this.ManagementUnit != null)
            {
                yield return Tuple.Create(DefaultConstants.ENTITY_TYPE_MANAGEMENT_UNIT_NAME, ((ManagementUnitIdentityDTO)this.ManagementUnit).Id);
            }

            if (this.BusinessUnit != null)
            {
                yield return Tuple.Create(DefaultConstants.ENTITY_TYPE_FINANCIAL_BUSINESS_UNIT_NAME, ((BusinessUnitIdentityDTO)this.BusinessUnit).Id);
            }

            if (this.Location != null)
            {
                yield return Tuple.Create(DefaultConstants.ENTITY_TYPE_LOCATION_NAME, ((LocationIdentityDTO)this.Location).Id);
            }
        }

        public string GetRoleName()
        {
            return this.Role.GetName();
        }
    }
}
