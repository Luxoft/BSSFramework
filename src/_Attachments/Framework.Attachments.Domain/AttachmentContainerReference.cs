using System;

using Framework.Attachments.Abstract;
using Framework.Configuration.Domain;
using Framework.Restriction;

namespace Framework.Attachments.Domain
{
    /// <summary>
    /// Ссылка на контейнер аттачмента
    /// </summary>
    public class AttachmentContainerReference : DomainObjectBase, IAttachmentContainerReference<DomainType>
    {
        private DomainType domainType;

        private Guid objectId;

        /// <summary>
        /// Тип доменного объекта
        /// </summary>
        [Required]
        public virtual DomainType DomainType
        {
            get { return this.domainType; }
            set { this.domainType = value; }
        }

        /// <summary>
        /// Идентификатор доменного объекта
        /// </summary>
        [Required]
        public virtual Guid ObjectId
        {
            get { return this.objectId; }
            set { this.objectId = value; }
        }
    }
}
