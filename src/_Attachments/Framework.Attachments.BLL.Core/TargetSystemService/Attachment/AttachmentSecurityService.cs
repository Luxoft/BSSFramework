using System;
using System.Linq;
using System.Linq.Expressions;

using Framework.Attachments.Domain;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.Persistent;
using Framework.SecuritySystem;

namespace Framework.Attachments.BLL
{
    public class AttachmentSecurityService<TBLLContext, TPersistentDomainObjectBase> : BLLContextContainer<IAttachmentsBLLContext>, IAttachmentSecurityProviderSource

        where TBLLContext : class,

                            ITypeResolverContainer<string>,
                            ISecurityServiceContainer<IRootSecurityService<TBLLContext, TPersistentDomainObjectBase>>,
                            IDefaultBLLContext<TPersistentDomainObjectBase, Guid>


        where TPersistentDomainObjectBase : class, IIdentityObject<Guid>
    {
        private readonly TBLLContext _targetSystemContext;


        public AttachmentSecurityService(IAttachmentsBLLContext context, TBLLContext targetSystemContext)
            : base(context)
        {
            if (targetSystemContext == null) throw new ArgumentNullException(nameof(targetSystemContext));

            this._targetSystemContext = targetSystemContext;
        }


        public ISecurityProvider<TDomainObject> GetAttachmentSecurityProvider<TDomainObject>(Expression<Func<TDomainObject, AttachmentContainer>> containerPath, DomainType mainDomainType, BLLSecurityMode securityMode)
            where TDomainObject : PersistentDomainObjectBase
        {
            if (containerPath == null) throw new ArgumentNullException(nameof(containerPath));
            if (mainDomainType == null) throw new ArgumentNullException(nameof(mainDomainType));

            return new GetAttachmentSecurityProviderProcessor<TDomainObject>(this._targetSystemContext, this.Context, containerPath, securityMode).Process(mainDomainType.Name);
        }


        private class GetAttachmentSecurityProviderProcessor<TDomainObject> : TypeResolverDomainObjectProcessor<TBLLContext, TPersistentDomainObjectBase, ISecurityProvider<TDomainObject>>
            where TDomainObject : PersistentDomainObjectBase
        {
            private readonly IAttachmentsBLLContext _mainContext;

            private readonly Expression<Func<TDomainObject, AttachmentContainer>> _containerPath;
            private readonly BLLSecurityMode _securityMode;


            public GetAttachmentSecurityProviderProcessor(TBLLContext context, IAttachmentsBLLContext mainContext, Expression<Func<TDomainObject, AttachmentContainer>> containerPath, BLLSecurityMode securityMode)
                : base(context)
            {
                this._mainContext = mainContext;
                this._containerPath = containerPath ?? throw new ArgumentNullException(nameof(containerPath));
                this._securityMode = securityMode;
            }


            protected override ISecurityProvider<TDomainObject> Process<TMainDomainObject>()
            {
                var targetSystemSecurityProvider = this.Context.SecurityService.GetAttachmentSecurityProvider<TMainDomainObject>(this._securityMode);

                return new AttachmentSecurityProvider<TDomainObject, TMainDomainObject>(this._mainContext, targetSystemSecurityProvider, this.Context, this._containerPath);
            }
        }

        public class AttachmentSecurityProvider<TDomainObject, TTargetSystemDomainObject> : SecurityProviderBase<TDomainObject>

            where TDomainObject : PersistentDomainObjectBase
            where TTargetSystemDomainObject : class, TPersistentDomainObjectBase
        {
            private readonly Expression<Func<TDomainObject, AttachmentContainer>> containerPath;

            private readonly ISecurityProvider<TTargetSystemDomainObject> targetSystemSecurityProvider;

            private static readonly LambdaCompileCache CompileCache = new LambdaCompileCache();


            public AttachmentSecurityProvider(IAttachmentsBLLContext mainContext, ISecurityProvider<TTargetSystemDomainObject> targetSystemSecurityProvider, TBLLContext context, Expression<Func<TDomainObject, AttachmentContainer>> containerPath)
                : base(mainContext.AccessDeniedExceptionService)
            {
                this.Context = context;

                this.containerPath = containerPath ?? throw new ArgumentNullException(nameof(containerPath));
                this.targetSystemSecurityProvider = targetSystemSecurityProvider;
            }


            public TBLLContext Context { get; }

            public override IQueryable<TDomainObject> InjectFilter(IQueryable<TDomainObject> queryable)
            {
                var mainQueryable = this.Context.Logics.Default.Create<TTargetSystemDomainObject>().GetUnsecureQueryable();

                var filteredMainQuarable = this.targetSystemSecurityProvider.InjectFilter(mainQueryable);

                return filteredMainQuarable.Join(queryable, v => v.Id, this.containerPath.Select(v => v.ObjectId), (_, domainObject) => domainObject);
            }

            public override bool HasAccess(TDomainObject domainObject)
            {
                if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

                var targetDomainObject = this.Context.Logics.Default.Create<TTargetSystemDomainObject>().GetById(this.containerPath.Eval(domainObject, CompileCache).ObjectId, true);

                return this.targetSystemSecurityProvider.HasAccess(targetDomainObject);
            }

            public override UnboundedList<string> GetAccessors(TDomainObject domainObject)
            {
                var mainBLL = this.Context.Logics.Implemented.Create<TTargetSystemDomainObject>();

                var obj = mainBLL.GetById(this.containerPath.Eval(domainObject).ObjectId, true);

                return this.targetSystemSecurityProvider.GetAccessors(obj);
            }
        }
    }
}
