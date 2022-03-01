using System;
using System.Collections.Generic;

using Framework.DomainDriven.DTOGenerator;
using Framework.DomainDriven.DTOGenerator.Client;
using Framework.DomainDriven.Generation.Domain;

namespace WorkflowSampleSystem.CodeGenerate.ClientDTO
{
    public class WorkflowSampleSystemClientDependencyGeneratePolicy : ClientDependencyGeneratePolicy
    {
        public WorkflowSampleSystemClientDependencyGeneratePolicy(IGeneratePolicy<RoleFileType> baseGeneratePolicy, IEnumerable<GenerateTypeMap> maps)
            : base(baseGeneratePolicy, maps)
        {
        }

        protected override bool InternalUsed(Type domainType, RoleFileType fileType)
        {
            if (base.InternalUsed(domainType, fileType))
            {
                return true;
            }
            else if (fileType == FileType.SimpleDTO)
            {
                return base.InternalUsed(domainType, fileType) || this.Used(domainType, WorkflowSampleSystemFileType.FullRefDTO)
                       || this.IsUsedProperty(WorkflowSampleSystemFileType.FullRefDTO, domainType, fileType);
            }
            else if (fileType == FileType.FullDTO)
            {
                return base.InternalUsed(domainType, fileType) || this.IsUsedProperty(WorkflowSampleSystemFileType.FullRefDTO, domainType, fileType);
            }
            else
            {
                return false;
            }
        }
    }
}