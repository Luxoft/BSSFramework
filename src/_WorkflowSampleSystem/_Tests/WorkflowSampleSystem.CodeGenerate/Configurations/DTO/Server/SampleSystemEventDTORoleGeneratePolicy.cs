using System;

using Framework.DomainDriven.DTOGenerator;
using Framework.DomainDriven.Generation.Domain;
using Framework.DomainDriven.Serialization;

using WorkflowSampleSystem.CustomReports;

namespace WorkflowSampleSystem.CodeGenerate.ServerDTO
{
    public class WorkflowSampleSystemEventDTORoleGeneratePolicy : IGeneratePolicy<RoleFileType>
    {
        public bool Used(Type domainType, RoleFileType identity)
        {
            if (typeof(ReportParameterBase).IsAssignableFrom(domainType))
            {
                return false;
            }

            if (identity.Role == DTORole.Event)
            {
                return true;
            }

            return false;
        }
    }
}