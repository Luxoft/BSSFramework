using System;
using System.Linq;

using Framework.DomainDriven.DTOGenerator;
using Framework.DomainDriven.Generation.Domain;
using Framework.Workflow.Domain;

namespace Framework.Workflow.TestGenerate
{
    public class WorkflowCustomGeneratePolicy : IGeneratePolicy<RoleFileType>
    {
        public bool Used(Type domainType, RoleFileType fileType)
        {
            if (domainType == null) throw new ArgumentNullException(nameof(domainType));
            if (fileType == null) throw new ArgumentNullException(nameof(fileType));

            if (fileType == FileType.StrictDTO)
            {
                return new[]
                {
                    typeof(AvailableTaskInstanceMainFilterModel),
                    typeof(StartWorkflowRequest),
                    typeof(ExecuteCommandRequest),
                    typeof(MassExecuteCommandRequest),
                    typeof(AvailableTaskInstanceUntypedMainFilterModel),
                }.Contains(domainType);
            }
            else if (fileType == FileType.ProjectionDTO)
            {
                return true;

                //return new[]
                //{
                //    typeof(AvailableTaskInstanceWorkflowGroup)
                //}.Contains(domainType);
            }
            else
            {
                return false;
            }
        }
    }
}
