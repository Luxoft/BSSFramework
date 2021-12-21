using System;

using Framework.DomainDriven.BLL;
using Framework.Workflow.Domain.Definition;

using JetBrains.Annotations;

namespace Framework.Workflow.BLL
{
    public partial interface IDomainTypeBLL : IPathBLL<DomainType>
    {
        DomainType GetByType([NotNull] Type domainObjectType);
    }
}
