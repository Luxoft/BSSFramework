using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Persistent;
using Framework.Workflow.Domain;
using Framework.Workflow.Domain.Definition;

using JetBrains.Annotations;

namespace Framework.Workflow.BLL
{
    public partial class TargetSystemBLL
    {
        public TargetSystem RegisterBase()
        {
            return this.Register(PersistentHelper.BaseTargetSystemName, true, false, PersistentHelper.BaseTargetSystemId, new[] { typeof(string), typeof(int), typeof(DateTime) });
        }

        public TargetSystem Register<TPersistentDomainObjectBase>(bool isMain, IEnumerable<Assembly> assemblies)
        {
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));

            this.Context.Logics.NamedLock.Lock(NamedLockOperation.UpdateTargetSystem);

            var domainTypes = assemblies.SelectMany(z => z.GetTypes())
                                        .Where(z => !z.IsAbstract)
                                        .Where(z => !z.IsGenericTypeDefinition)
                                        .Where(z => typeof(TPersistentDomainObjectBase).IsAssignableFrom(z));

            return this.Register(typeof(TPersistentDomainObjectBase).GetTargetSystemName(), false, isMain, typeof(TPersistentDomainObjectBase).GetTargetSystemId(), domainTypes);
        }

        private TargetSystem Register([NotNull] string targetSystemName, bool isBase, bool isMain, Guid id, [NotNull] IEnumerable<Type> domainTypes)
        {
            if (targetSystemName == null) throw new ArgumentNullException(nameof(targetSystemName));
            if (domainTypes == null) throw new ArgumentNullException(nameof(domainTypes));

            var dbTargetSystem = this.GetById(id, false);
            var targetSystem = dbTargetSystem ?? new TargetSystem(isBase, isMain) { Name = targetSystemName, Id = id };

            if (dbTargetSystem == null)
            {
                this.SaveOrInsert(targetSystem);
            }

            var newItems = domainTypes.Except(targetSystem.DomainTypes, (type, parameterType) => type.Name == parameterType.Name && type.Namespace == parameterType.NameSpace).ToArray();

            var newDomainTypes = newItems.ToArray(type => new DomainType(targetSystem)
            {
                Id = type.GetDomainTypeId(),
                Name = type.Name,
                NameSpace = type.Namespace,
                Role = isBase ? DomainTypeRole.Primitive : DomainTypeRole.Domain
            });

            if (newDomainTypes.Any())
            {
                foreach (var domainType in newDomainTypes)
                {
                    this.Context.Logics.DomainType.SaveOrInsert(domainType);
                }
            }

            return targetSystem;
        }
    }
}
