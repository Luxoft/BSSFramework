using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Framework.Core;
using Framework.Core.Serialization;
using Framework.DomainDriven.Attributes;
using Framework.DomainDriven.BLL;
using Framework.Persistent;
using Framework.Restriction;
using Framework.Security;

namespace Framework.Configuration.Domain
{
    [DomainObjectAccess, BLLRemoveRole(CustomImplementation = true)]
    [NotAuditedClass]
    public class Attachment : BaseDirectory, IDetail<AttachmentContainer>, IMaster<AttachmentTag>, ITemplateContainer
    {
        private byte[] content;

        private readonly AttachmentContainer container;

        private readonly ICollection<AttachmentTag> tags = new List<AttachmentTag>();

        /// <summary>
        /// Конструктор
        /// </summary>
        protected Attachment()
        {

        }

        /// <summary>
        /// Конструктор создает аттачмент с ссылкой на контейнер
        /// </summary>
        /// <param name="container"></param>
        public Attachment(AttachmentContainer container)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));

            this.container = container;
            this.container.AddDetail(this);
        }

        /// <summary>
        /// Контейнер аттачментов
        /// </summary>
        public virtual AttachmentContainer Container
        {
            get { return this.container; }
        }

        /// <summary>
        /// Коллекция тегов аттачмента
        /// </summary>
        [UniqueGroup]
        public virtual IEnumerable<AttachmentTag> Tags
        {
            get { return this.tags; }
        }

        /// <summary>
        /// Тело аттачмента
        /// </summary>
        [Required]
        public virtual byte[] Content
        {
            get { return this.content; }
            set { this.content = value; }
        }

        /// <summary>
        /// Все теги в виде словаря
        /// </summary>
        /// <returns>Тег</returns>
        public virtual Dictionary<string, string> GetTagsDict()
        {
            return this.Tags.ToDictionary(tag => tag.Name, tag => tag.Value);
        }

        /// <summary>
        /// Метод проверяет наличие типизированного тега
        /// </summary>
        /// <typeparam name="T">Тип тега</typeparam>
        /// <param name="tagName">Имя тега</param>
        /// <param name="selector">Селектор тега</param>
        /// <returns>"True"-тег найден, "False"-тег отсутствует</returns>
        public virtual bool HasTag<T>(string tagName, Func<T, bool> selector)
        {
            return MaybeExtensions.SelectMany<string, T>(this.GetTagsDict().GetMaybeValue(tagName), ParserHelper.TryParse<T>)
                                     .Select(selector)
                                     .GetValueOrDefault();
        }

        /// <summary>
        /// Метод проверяет наличие нетипизированного тега
        /// </summary>
        /// <param name="tagName">Имя тега</param>
        /// <returns>"True"-тег найден, "False"-тег отсутствует</returns>
        public virtual bool HasTag(string tagName)
        {
            return this.HasTag<bool>(tagName, v => v);
        }

        #region IDetail<AttachmentContainer> Members

        AttachmentContainer IDetail<AttachmentContainer>.Master
        {
            get { return this.Container; }
        }

        #endregion

        #region IMaster<AttachmentTag> Members

        ICollection<AttachmentTag> IMaster<AttachmentTag>.Details
        {
            get { return (ICollection<AttachmentTag>)this.Tags; }
        }

        #endregion
}
}
