using System;

using Framework.Core;
using Framework.DomainDriven.Attributes;
using Framework.Persistent;

namespace Framework.Attachments.Domain
{
    [NotAuditedClass]
    /// <summary>
    /// Тег для идентификации аттачмента
    /// </summary>
    public class AttachmentTag : BaseDirectory, IDetail<Attachment>
    {
        private readonly Attachment attachment;

        private string value;

        protected AttachmentTag()
        {

        }

        /// <summary>
        /// Конструктор создает тег с ссылкой на аттачмент
        /// </summary>
        /// <param name="attachment">Аттачмент</param>
        public AttachmentTag(Attachment attachment)
        {
            if (attachment == null) throw new ArgumentNullException(nameof(attachment));

            this.attachment = attachment;
            this.attachment.AddDetail(this);
        }

        /// <summary>
        /// Аттачмент тега
        /// </summary>
        public virtual Attachment Attachment
        {
            get { return this.attachment; }
        }

        /// <summary>
        /// Значение тега
        /// </summary>
        public virtual string Value
        {
            get { return this.value.TrimNull(); }
            set { this.value = value.TrimNull(); }
        }

        #region IDetail<Attachment> Members

        Attachment IDetail<Attachment>.Master
        {
            get { return this.Attachment; }
        }

        #endregion
    }
}
