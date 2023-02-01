using System;

using Framework.DomainDriven.DTOGenerator;
using Framework.DomainDriven.Generation.Domain;
using Framework.DomainDriven.Serialization;

namespace SampleSystem.CodeGenerate.ServerDTO;

public class SampleSystemEventDTORoleGeneratePolicy : IGeneratePolicy<RoleFileType>
{
    public bool Used(Type domainType, RoleFileType identity)
    {
        if (identity.Role == DTORole.Event)
        {
            return true;
        }

        return false;
    }
}
