using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Attachments.Domain;
using Framework.Configuration.BLL;
using Framework.DomainDriven.BLL;
using Framework.Persistent;
using Framework.SecuritySystem;

using JetBrains.Annotations;

using nuSpec.Abstraction;

namespace Framework.Attachments.BLL
{
    public class AttachmentBLL : SecurityDomainBLLBase<Attachment, BLLBaseOperation>, IAttachmentBLL
    {
        private readonly IAttachmentBLLContextModule contextModule;

        public AttachmentBLL(IAttachmentBLLContextModule contextModule, ISpecificationEvaluator specificationEvaluator = null)
                : base(contextModule.Configuration, specificationEvaluator)
        {
            this.contextModule = contextModule;
        }

        public AttachmentBLL(IAttachmentBLLContextModule contextModule, ISecurityProvider<Attachment> securityOperation, ISpecificationEvaluator specificationEvaluator = null)
                : base(contextModule.Configuration, securityOperation, specificationEvaluator)
        {
            this.contextModule = contextModule;
        }

        public override void Insert([NotNull] Attachment attachment, Guid id)
        {
            if (attachment == null) throw new ArgumentNullException(nameof(attachment));

            this.InsertWithoutCascade(attachment, id);

            this.Context.Logics.Default.Create<AttachmentTag>().Insert(attachment.Tags);

            base.Insert(attachment, id);
        }

        public override void Save(Attachment attachment)
        {
            if (attachment == null) throw new ArgumentNullException(nameof(attachment));

            if (attachment.Container.IsNew)
            {
                new AttachmentContainerBLL(this.contextModule).Save(attachment.Container);
            }

            base.Save(attachment);
        }


        public override void Remove(Attachment attachment)
        {
            if (attachment == null) throw new ArgumentNullException(nameof(attachment));

            var container = attachment.Container;

            base.Remove(attachment);

            container.RemoveDetail(attachment);

            var containerBLL = new AttachmentContainerBLL(this.contextModule);

            if (container.Attachments.Any())
            {
                containerBLL.Save(container);
            }
            else
            {
                containerBLL.Remove(container);
            }
        }

        public IList<Attachment> GetObjectsBy(Type type, Guid domainObjectId)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            var domainType = this.Context.GetDomainType(type, true);

            return this.GetListBy(attachment => attachment.Container.DomainType == domainType && attachment.Container.ObjectId == domainObjectId);
        }

        public IList<Attachment> GetObjectsBy<TDomainObject>(TDomainObject domainObject)
            where TDomainObject : IIdentityObject<Guid>
        {
            if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

            return this.GetObjectsBy(typeof (TDomainObject), domainObject.Id);
        }
    }
}
