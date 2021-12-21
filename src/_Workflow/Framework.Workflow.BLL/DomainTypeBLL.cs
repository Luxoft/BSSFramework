using System;

using Framework.Core;
using Framework.Workflow.Domain.Definition;

namespace Framework.Workflow.BLL
{
    public partial class DomainTypeBLL
    {
        public DomainType GetByType(Type domainObjectType)
        {
            if (domainObjectType == null) throw new ArgumentNullException(nameof(domainObjectType));

            var targetSystem = this.Context.GetTargetSystemService(domainObjectType, false).Maybe(targetSystemService => targetSystemService.TargetSystem)
                            ?? this.Context.Logics.TargetSystem.GetObjectBy(ts => ts.IsBase, true);

            return this.GetObjectBy(parameterType => parameterType.TargetSystem == targetSystem && parameterType.NameSpace == domainObjectType.Namespace && parameterType.Name == domainObjectType.Name, true);
        }

        public DomainType GetByPath(string path)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));

            var blocks = path.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            if (blocks.Length == 1)
            {
                var domainType = this.Context.GetTargetSystemServices().ToComposite(tss => tss.TypeResolverS).Resolve(blocks[0], true);

                return this.Context.GetDomainType(domainType);
            }
            else if (blocks.Length == 2)
            {
                var targetSystemService = this.Context.GetTargetSystemService(blocks[0]);

                var domainType = targetSystemService.TypeResolverS.Resolve(blocks[1]);

                return this.Context.GetDomainType(domainType);
            }
            else
            {
                throw new System.ArgumentException("invalid block count", nameof(path));
            }
        }
    }
}