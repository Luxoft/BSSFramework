using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Framework.Attachments.Abstract;
using Framework.Attachments.Domain;
using Framework.Configuration.BLL;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.Exceptions;
using Framework.Persistent;
using Framework.SecuritySystem;

using JetBrains.Annotations;

namespace Framework.Attachments.BLL
{
    public class TargetSystemService<TBLLContext, TPersistentDomainObjectBase> : BLLContextContainer<IConfigurationBLLContext>, ITargetSystemService<TPersistentDomainObjectBase>

        where TBLLContext : class, ITypeResolverContainer<string>, ISecurityServiceContainer<IRootSecurityService<TBLLContext, TPersistentDomainObjectBase>>, IDefaultBLLContext<TPersistentDomainObjectBase, Guid>, IBLLOperationEventContext<TPersistentDomainObjectBase>
        where TPersistentDomainObjectBase : class, IIdentityObject<Guid>
    {
        private readonly IAttachmentBLLContextModule contextModule;

        private readonly Lazy<bool> lazyHasAttachments;

        private readonly Framework.Configuration.BLL.ITargetSystemService<TBLLContext> configTargetSystemService;

        /// <summary>
        /// Создаёт экземпляр класса <see cref="TargetSystemService{TBLLContext, TPersistentDomainObjectBase}" />.
        /// </summary>
        /// <param name="context">Контекст конфигурации.</param>
        public TargetSystemService(IAttachmentBLLContextModule contextModule, [NotNull] TargetSystem targetSystem)
            : base(contextModule.Configuration)
        {
            if (targetSystem == null) throw new ArgumentNullException(nameof(targetSystem));

            this.contextModule = contextModule;

            this.TargetSystem = targetSystem;

            this.configTargetSystemService = (Framework.Configuration.BLL.ITargetSystemService<TBLLContext>)this.Context.GetTargetSystemService(targetSystem);

            this.lazyHasAttachments = LazyHelper.Create(() => new AttachmentBLL(this.contextModule).GetUnsecureQueryable().Any(a => a.Container.DomainType.TargetSystem == this.TargetSystem));
        }

        public Configuration.BLL.ITargetSystemService ConfigTargetSystemService => this.configTargetSystemService;

        public TargetSystem TargetSystem
        {
            get;
        }


        public Type PersistentDomainObjectBaseType => typeof(TPersistentDomainObjectBase);

        public bool HasAttachments => this.lazyHasAttachments.Value;


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

            var logic = new AttachmentContainerBLL(this.contextModule);

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

            new DenormalizeHasAttachmentFlagProcessor(this.configTargetSystemService.TargetSystemContext, container.ObjectId, value).Process(container.DomainType.Name);
        }

        public IEnumerable<Guid> GetNotExistsObjects(DomainType domainType, IEnumerable<Guid> idents)
        {
            if (domainType == null) throw new ArgumentNullException(nameof(domainType));
            if (idents == null) throw new ArgumentNullException(nameof(idents));

            var domainObjectType = this.configTargetSystemService.TypeResolver.Resolve(domainType, true);

            var method = new Func<IEnumerable<Guid>, IEnumerable<Guid>>(this.GetNotExistsObjects<TPersistentDomainObjectBase>)
                        .CreateGenericMethod(domainObjectType);


            return (IEnumerable<Guid>)method.Invoke(this, new object[] { idents });
        }

        private IEnumerable<Guid> GetNotExistsObjects<TDomainObject>(IEnumerable<Guid> idents)
            where TDomainObject : class, TPersistentDomainObjectBase
        {
            if (idents == null) throw new ArgumentNullException(nameof(idents));

            var cachedIdents = idents.ToList();

            return this.configTargetSystemService.TargetSystemContext.Logics.Default
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

            return new AttachmentSecurityService<TBLLContext, TPersistentDomainObjectBase>(this.Context, this.configTargetSystemService.TargetSystemContext)
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
