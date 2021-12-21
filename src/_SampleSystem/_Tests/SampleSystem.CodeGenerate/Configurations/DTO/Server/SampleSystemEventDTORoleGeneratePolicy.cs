using System;

using Framework.DomainDriven.DTOGenerator;
using Framework.DomainDriven.Generation.Domain;
using Framework.DomainDriven.Serialization;

using SampleSystem.CustomReports;

namespace SampleSystem.CodeGenerate.ServerDTO
{
    public class SampleSystemEventDTORoleGeneratePolicy : IGeneratePolicy<RoleFileType>
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