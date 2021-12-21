using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.Persistent;

using Framework.Workflow.BLL;
using Framework.Workflow.Domain;
using Framework.Workflow.Domain.Definition;

namespace Framework.Workflow.Generated.DTO
{
    public class WorkflowGraphMappingService : BLLContextContainer<IWorkflowBLLContext>, IMappingService<WorkflowStrictDTO, Workflow.Domain.Definition.Workflow>
    {
        private readonly bool _isMerge;


        public WorkflowGraphMappingService(IWorkflowBLLContext context, bool isMerge)
            : base(context)
        {
            this._isMerge = isMerge;
        }


        public void Map(WorkflowStrictDTO source, Domain.Definition.Workflow target)
        {
            var internalMappingService = new InternalWorkflowGraphMappingService(this.Context, this._isMerge);

            source.MapToDomainObject(internalMappingService, target);

            return;
        }


        private class InternalWorkflowGraphMappingService : WorkflowServerPrimitiveDTOMappingService
        {
            private static readonly HashSet<Type> ExternalRefTypes = new[] { typeof(DomainType), typeof(TargetSystem) }.ToHashSet();

            private static readonly HashSet<Type> GraphTypes = new[]
            {
                typeof(Domain.Definition.Workflow),
                typeof(WorkflowLambda),
                typeof(Role),

                typeof(ConditionState),
                typeof(State),
                typeof(ParallelState),

                typeof(ConditionStateEvent),
                typeof(ParallelStateFinalEvent),
                typeof(StateTimeoutEvent),
                typeof(StateDomainObjectEvent),
                typeof(CommandEvent),
            }.ToHashSet();



            private readonly bool _isMerge;


            private readonly Dictionary<Type, Dictionary<Guid, PersistentDomainObjectBase>> _items =

                new Dictionary<Type, Dictionary<Guid, PersistentDomainObjectBase>>();



            public InternalWorkflowGraphMappingService(IWorkflowBLLContext context, bool isMerge)
                : base(context)
            {
                this._isMerge = isMerge;
            }


            public override IDTOMappingVersionService<AuditPersistentDomainObjectBase, long> VersionService
            {
                get { return this._isMerge ? base.VersionService : IgnoreVersionService.Value; }
            }


            protected override TDomainObject ToDomainObject<TMappingObject, TDomainObject>(TMappingObject mappingObject, Func<TDomainObject> createFunc)
            {
                if (GraphTypes.Contains(typeof(TDomainObject)) && mappingObject.Id.IsDefault())
                {
                    throw new System.ArgumentException($"Empty Id in object \"{typeof(TMappingObject).Name}\"", nameof(mappingObject));
                }

                var versionObject = (IVersionObject<long>)mappingObject;

                var domainObject = this._isMerge && versionObject.Version != 0 ? base.GetById<TDomainObject>(mappingObject.Id)
                                                                               : createFunc();

                this.MapToDomainObject(mappingObject, domainObject);

                if (GraphTypes.Contains(typeof(TDomainObject)))
                {
                    this._items.GetValueOrCreate(typeof(TDomainObject), () => new Dictionary<Guid, PersistentDomainObjectBase>())
                               .Add(mappingObject.Id, domainObject);
                }

                return domainObject;
            }

            public override TDomainObject GetById<TDomainObject>(Guid ident, IdCheckMode checkMode = IdCheckMode.SkipEmpty, LockRole lockRole = LockRole.None)
            {
                if (ExternalRefTypes.Contains(typeof(TDomainObject)) || ident.IsDefault())
                {
                    return base.GetById<TDomainObject>(ident, checkMode);
                }
                else
                {
                    return this.GetByIdWithDerived<TDomainObject>(ident).FromMaybe(() => $"\"{typeof(TDomainObject).Name}\" with Id \"{ident}\" not found");
                }
            }

            private TDomainObject GetByIdWithDerived<TDomainObject>(Guid ident)
                where TDomainObject : class
            {
                var request = from item in this._items

                              where typeof(TDomainObject).IsAssignableFrom(item.Key)

                              let dict = item.Value

                              let res = dict.GetValueOrDefault(ident)

                              where res != null

                              select res.AsCast<TDomainObject>();

                return request.SingleOrDefault();
            }
        }


        private class IgnoreVersionService : IDTOMappingVersionService<AuditPersistentDomainObjectBase, long>
        {
            private IgnoreVersionService()
            {

            }

            public long GetVersion<TDomainObject>(long mappingObjectVersion, TDomainObject domainObject)
                where TDomainObject : AuditPersistentDomainObjectBase, IVersionObject<long>
            {
                return domainObject.Version;
            }


            public static readonly IgnoreVersionService Value = new IgnoreVersionService();
        }
    }
}
