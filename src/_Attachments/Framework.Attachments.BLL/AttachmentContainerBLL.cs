using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Attachments.Domain;
using Framework.Configuration.BLL;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Persistent;
using Framework.SecuritySystem;

using JetBrains.Annotations;

using nuSpec.Abstraction;

namespace Framework.Attachments.BLL
{
    public partial class AttachmentContainerBLL : SecurityDomainBLLBase<AttachmentContainer, BLLBaseOperation>, IAttachmentContainerBLL
    {
        private readonly IAttachmentBLLContextModule contextModule;

        public AttachmentContainerBLL(IAttachmentBLLContextModule contextModule, ISpecificationEvaluator specificationEvaluator = null)
                : base(contextModule.Configuration, specificationEvaluator)
        {
            this.contextModule = contextModule;
        }

        public AttachmentContainerBLL(IAttachmentBLLContextModule contextModule, ISecurityProvider<AttachmentContainer> securityOperation, ISpecificationEvaluator specificationEvaluator = null)
                : base(contextModule.Configuration, securityOperation, specificationEvaluator)
        {
        }

        public override void Insert([NotNull] AttachmentContainer attachmentContainer, Guid id)
        {
            if (attachmentContainer == null) throw new ArgumentNullException(nameof(attachmentContainer));

            this.InsertWithoutCascade(attachmentContainer, id);

            new AttachmentBLL(this.contextModule).Insert(attachmentContainer.Attachments);

            base.Insert(attachmentContainer, id);
        }

        public override void Save(Domain.AttachmentContainer attachmentContainer)
        {
            if (attachmentContainer == null) throw new ArgumentNullException(nameof(attachmentContainer));

            this.contextModule.GetPersistentTargetSystemService(attachmentContainer.DomainType.TargetSystem).TryDenormalizeHasAttachmentFlag(attachmentContainer, true);

            base.Save(attachmentContainer);
        }

        public override void Remove(Domain.AttachmentContainer attachmentContainer)
        {
            if (attachmentContainer == null) throw new ArgumentNullException(nameof(attachmentContainer));

            this.contextModule.GetPersistentTargetSystemService(attachmentContainer.DomainType.TargetSystem).TryDenormalizeHasAttachmentFlag(attachmentContainer, false);

            base.Remove(attachmentContainer);
        }

        public IList<AttachmentContainer> GetNotSynchronizated()
        {
            var request = from attachmentContainer in this.GetFullList()

                          group attachmentContainer by attachmentContainer.DomainType into g

                          let objectIdents = g.ToList(c => c.ObjectId)

                          let targetSystemService = this.contextModule.GetPersistentTargetSystemService(g.Key.TargetSystem)

                          let failIdents = targetSystemService.GetNotExistsObjects(g.Key, objectIdents)

                          from container in g

                          where failIdents.Contains(container.Id)

                          select container;


            return request.ToList();
        }

        public AttachmentContainer GetObjectBy(Type type, Guid domainObjectId)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            var domainType = this.Context.GetDomainType(type, true);

            return this.GetObjectBy(container => container.DomainType == domainType && container.ObjectId == domainObjectId, false);
        }

        public AttachmentContainer GetObjectBy<TDomainObject>(TDomainObject domainObject)
            where TDomainObject : class, IIdentityObject<Guid>
        {
            if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

            return this.GetObjectBy(typeof (TDomainObject), domainObject.Id);
        }
    }
}
