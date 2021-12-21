using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Configuration.Domain;
using Framework.DomainDriven.BLL;
using Framework.Persistent;

using JetBrains.Annotations;

namespace Framework.Configuration.BLL
{
    public partial class AttachmentBLL
    {
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
                this.Context.Logics.AttachmentContainer.Save(attachment.Container);
            }

            base.Save(attachment);
        }


        public override void Remove(Attachment attachment)
        {
            if (attachment == null) throw new ArgumentNullException(nameof(attachment));

            var container = attachment.Container;

            base.Remove(attachment);

            container.RemoveDetail(attachment);

            var containerBLL = this.Context.Logics.AttachmentContainer;

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

            return this.GetObjectsBy(attachment => attachment.Container.DomainType == domainType && attachment.Container.ObjectId == domainObjectId);
        }

        public IList<Attachment> GetObjectsBy<TDomainObject>(TDomainObject domainObject)
            where TDomainObject : IIdentityObject<Guid>
        {
            if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

            return this.GetObjectsBy(typeof (TDomainObject), domainObject.Id);
        }
    }
}
