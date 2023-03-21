using System;
using System.Collections.Generic;

using Framework.DomainDriven.DTOGenerator;
using Framework.DomainDriven.DTOGenerator.Server;
using Framework.DomainDriven.Generation.Domain;

using SampleSystem.Domain;

namespace SampleSystem.CodeGenerate.ServerDTO;

public class SampleSystemServerDependencyGeneratePolicy : ServerDependencyGeneratePolicy
{
    public SampleSystemServerDependencyGeneratePolicy(IGeneratePolicy<RoleFileType> baseGeneratePolicy, IEnumerable<GenerateTypeMap> maps)
            : base(baseGeneratePolicy, maps)
    {
    }

    protected override bool InternalUsed(Type domainType, RoleFileType fileType)
    {
        if (domainType == typeof(Insurance) && fileType == ServerFileType.SimpleEventDTO)
        {
            return base.InternalUsed(domainType, fileType);
        }

        if (fileType == ServerFileType.BaseEventDTO)
        {
            return true;
        }

        if (base.InternalUsed(domainType, fileType))
        {
            return true;
        }
        else if (fileType == FileType.SimpleDTO)
        {
            return base.InternalUsed(domainType, fileType) || this.Used(domainType, SampleSystemFileType.FullRefDTO)
                                                           || this.IsUsedProperty(SampleSystemFileType.FullRefDTO, domainType, fileType);
        }
        else if (fileType == FileType.FullDTO)
        {
            return base.InternalUsed(domainType, fileType) || this.IsUsedProperty(SampleSystemFileType.FullRefDTO, domainType, fileType);
        }
        else
        {
            return false;
        }
    }
}
