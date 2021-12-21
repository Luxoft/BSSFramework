using System;
using System.Collections.Generic;

using Framework.DomainDriven.Attributes;
using Framework.DomainDriven.BLL;
using Framework.Persistent;
using Framework.Restriction;
using Framework.Security;

namespace Framework.Configuration.Domain
{
    /// <summary>
    /// Контейнер аттачментов
    /// </summary>
    [DomainObjectAccess]
    [UniqueGroup]
    [NotAuditedClass]
    [BLLRole]
    public class AttachmentContainer : AuditPersistentDomainObjectBase,
        IMaster<Attachment>,
        IAttachmentContainerReference<DomainType>,
        IDomainTypeElement<DomainType>
    {
        private readonly ICollection<Attachment> attachments = new List<Attachment>();

        private DomainType domainType;

        private Guid objectId;

        /// <summary>
        /// Коллекция аттачментов
        /// </summary>
        [UniqueGroup]
        public virtual IEnumerable<Attachment> Attachments
        {
            get { return this.attachments; }
        }

        /// <summary>
        /// Доменный тип, к которому привязана коллекция аттачментов
        /// </summary>
        [Required]
        [UniqueElement]
        public virtual DomainType DomainType
        {
            get { return this.domainType; }
            set { this.domainType = value; }
        }

        /// <summary>
        /// Идентификатор доменного объекта, к которому прикреплен аттачмент
        /// </summary>
        [Required]
        [UniqueElement]
        public virtual Guid ObjectId
        {
            get { return this.objectId; }
            set { this.objectId = value; }
        }

        #region IMaster<Attachment> Members

        ICollection<Attachment> IMaster<Attachment>.Details
        {
            get { return (ICollection<Attachment>)this.Attachments; }
        }

        #endregion
    }
}
