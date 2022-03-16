using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Framework.Attachments.Abstract;
using Framework.Attachments.Domain;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.Exceptions;
using Framework.Persistent;
using Framework.SecuritySystem;

using JetBrains.Annotations;

namespace Framework.Attachments.BLL
{
    public class TargetSystemService<TBLLContext, TPersistentDomainObjectBase> : BLLContextContainer<IAttachmentsBLLContext>, ITargetSystemService<TPersistentDomainObjectBase>

        where TBLLContext : class, ITypeResolverContainer<string>, IDefaultBLLContext<TPersistentDomainObjectBase, Guid>
        where TPersistentDomainObjectBase : class, IIdentityObject<Guid>
    {
        private readonly IRootSecurityService<TBLLContext, TPersistentDomainObjectBase> attachmentSecurityService;

        private readonly Lazy<bool> lazyHasAttachments;

        /// <summary>
        /// Создаёт экземпляр класса <see cref="TargetSystemService{TBLLContext, TPersistentDomainObjectBase}" />.
        /// </summary>
        /// <param name="context">Контекст конфигурации.</param>
        public TargetSystemService(IAttachmentsBLLContext context, [NotNull] TBLLContext targetSystemContext, [NotNull] TargetSystem targetSystem, [NotNull] IRootSecurityService<TBLLContext, TPersistentDomainObjectBase> attachmentSecurityService)
            : base(context)
        {
            if (targetSystem == null) throw new ArgumentNullException(nameof(targetSystem));

            this.attachmentSecurityService = attachmentSecurityService ?? throw new ArgumentNullException(nameof(attachmentSecurityService));

            this.TargetSystemContext = targetSystemContext ?? throw new ArgumentNullException(nameof(targetSystemContext));

            this.TargetSystem = targetSystem;

            this.lazyHasAttachments = LazyHelper.Create(() => this.Context.Logics.Attachment.GetUnsecureQueryable().Any(a => a.Container.DomainType.TargetSystem == this.TargetSystem));
            this.TypeResolver = this.TargetSystemContext.TypeResolver.OverrideInput((DomainType domainType) => domainType.FullTypeName).WithCache().WithLock();
        }

        public TargetSystem TargetSystem { get; }

        public TBLLContext TargetSystemContext { get; }
        public ITypeResolver<DomainType> TypeResolver { get; private set; }


        public Type PersistentDomainObjectBaseType => typeof(TPersistentDomainObjectBase);

        public bool HasAttachments => this.lazyHasAttachments.Value;

        public bool IsAssignable(Type domainType)
        {
            return typeof(TPersistentDomainObjectBase).IsAssignableFrom(domainType);
        }

        public void TryRemoveAttachments<TDomainObject>([NotNull] IEnumerable<TDomainObject> domainObjects)
            where TDomainObject : class, TPersistentDomainObjectBase
        {
            if (domainObjects == null) throw new ArgumentNullException(nameof(domainObjects));

            var idents = domainObjects.ToList(obj => obj.Id);

            if (!idents.Any())
            {
                return;
            }

            var domainType = this.Context.GetDomainType(typeof(TDomainObject), false);

            if (domainType == null)
            {
                return;
            }

            const int maxSqlParametersCount = 2000;
            var idsParts = idents.Split(maxSqlParametersCount);

            var logic = this.Context.Logics.AttachmentContainer;

            foreach (var ids in idsParts)
            {
                var containers = logic.GetUnsecureQueryable()
                    .Where(ac => ac.DomainType == domainType && ids.Contains(ac.ObjectId)).ToList();

                logic.Remove(containers);
            }
        }

        public void TryDenormalizeHasAttachmentFlag([NotNull] AttachmentContainer container, bool value)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));

            new DenormalizeHasAttachmentFlagProcessor(this.TargetSystemContext, container.ObjectId, value).Process(container.DomainType.Name);
        }

        public IEnumerable<Guid> GetNotExistsObjects(DomainType domainType, IEnumerable<Guid> idents)
        {
            if (domainType == null) throw new ArgumentNullException(nameof(domainType));
            if (idents == null) throw new ArgumentNullException(nameof(idents));

            var domainObjectType = this.TypeResolver.Resolve(domainType, true);

            var method = new Func<IEnumerable<Guid>, IEnumerable<Guid>>(this.GetNotExistsObjects<TPersistentDomainObjectBase>)
                    .CreateGenericMethod(domainObjectType);


            return (IEnumerable<Guid>)method.Invoke(this, new object[] { idents });
        }

        private IEnumerable<Guid> GetNotExistsObjects<TDomainObject>(IEnumerable<Guid> idents)
                where TDomainObject : class, TPersistentDomainObjectBase
        {
            if (idents == null) throw new ArgumentNullException(nameof(idents));

            var cachedIdents = idents.ToList();

            return this.TargetSystemContext.Logics.Default
                       .Create<TDomainObject>()
                       .GetUnsecureQueryable()
                       .Where(obj => !cachedIdents.Contains(obj.Id))
                       .Select(obj => obj.Id);
        }

        public ISecurityProvider<TDomainObject> GetAttachmentSecurityProvider<TDomainObject>(Expression<Func<TDomainObject, AttachmentContainer>> containerPath, DomainType mainDomainType, BLLSecurityMode securityMode)
                where TDomainObject : PersistentDomainObjectBase
        {
            if (containerPath == null) throw new ArgumentNullException(nameof(containerPath));
            if (mainDomainType == null) throw new ArgumentNullException(nameof(mainDomainType));

            return new AttachmentSecurityService<TBLLContext, TPersistentDomainObjectBase>(this.Context, this.TargetSystemContext, this.attachmentSecurityService)
                    .GetAttachmentSecurityProvider(containerPath, mainDomainType, securityMode);
        }


        void ITargetSystemService.TryRemoveAttachments(Array domainObjects)
        {
            if (domainObjects == null) throw new ArgumentNullException(nameof(domainObjects));

            var domainObjectType = domainObjects.GetElementType();

            if (!typeof(TPersistentDomainObjectBase).IsAssignableFrom(domainObjectType))
            {
                throw new BusinessLogicException("Domain Type {0} must be derived from {1}", domainObjectType.Name, typeof(TPersistentDomainObjectBase).Name);
            }

            var method = new Action<TPersistentDomainObjectBase[]>(this.TryRemoveAttachments)
                .Method
                .GetGenericMethodDefinition()
                .MakeGenericMethod(domainObjectType);

            method.Invoke(this, new object[] { domainObjects });
        }

        private class DenormalizeHasAttachmentFlagProcessor : TypeResolverDomainObjectProcessor<TBLLContext, TPersistentDomainObjectBase>
        {
            private readonly Guid _objectId;
            private readonly bool _value;


            public DenormalizeHasAttachmentFlagProcessor(TBLLContext context, Guid objectId, bool value)
                : base(context)
            {
                this._objectId = objectId;
                this._value = value;
            }


            protected override void Process<TDomainObject>()
            {
                var bll = this.Context.Logics.Default.Create<TDomainObject>();

                var domainObject = bll.GetById(this._objectId);

                if (domainObject is IAttachmentContainerHeader)
                {
                    (domainObject as IAttachmentContainerHeader).HasAttachments = this._value;

                    bll.Save(domainObject);
                }
            }
        }
    }
}
