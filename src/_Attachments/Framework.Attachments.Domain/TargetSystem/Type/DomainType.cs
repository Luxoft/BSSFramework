using System;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;
using Framework.Persistent.Mapping;
using Framework.Restriction;

namespace Framework.Attachments.Domain
{
    /// <summary>
    /// Описание доменного типа целевой системы
    /// </summary>
    [View]
    [BLLRole]
    [Table(Name = nameof(DomainType), Schema = "configuration")]
    public class DomainType : BaseDirectory, ITargetSystemElement<TargetSystem>, IDetail<TargetSystem>, IDomainType
    {
        private readonly TargetSystem targetSystem;

        private bool hasSecurityAttachment;

        private string nameSpace;

        protected DomainType()
        {
        }

        /// <summary>
        /// Конструктор доменного типа целовой системы
        /// </summary>
        /// <param name="targetSystem">Целевая система</param>
        public DomainType(TargetSystem targetSystem)
        {
            if (targetSystem == null) throw new ArgumentNullException(nameof(targetSystem));

            this.targetSystem = targetSystem;
            this.targetSystem.AddDetail(this);
        }

        /// <summary>
        /// Целевая система
        /// </summary>
        public virtual TargetSystem TargetSystem
        {
            get { return this.targetSystem; }
        }

        /// <summary>
        /// Название доменного типа
        /// </summary>
        [CustomSerialization(CustomSerializationMode.ReadOnly)]
        public override string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }

        /// <summary>
        /// Пространство имен
        /// </summary>
        [UniqueElement]
        [CustomSerialization(CustomSerializationMode.ReadOnly)]
        public virtual string NameSpace
        {
            get { return this.nameSpace.TrimNull(); }
            set { this.nameSpace = value.TrimNull(); }
        }

        /// <summary>
        /// Признак того, что прикрепляемые аттачменты к типу являются секурными и требуют предварительного формирования ключа POST-запросом при отдаче аттачмента через GET-метод rest-фасада
        /// </summary>
        [CustomSerialization(CustomSerializationMode.ReadOnly)]
        public virtual bool HasSecurityAttachment
        {
            get { return this.hasSecurityAttachment; }
            internal protected set { this.hasSecurityAttachment = value; }
        }

        /// <summary>
        /// Полное имя типа
        /// </summary>
        public virtual string FullTypeName
        {
            get
            {
                return string.IsNullOrEmpty(this.NameSpace)
                           ? this.Name
                           : $"{this.NameSpace}.{this.Name}";
            }
        }

        #region IDetail<TargetSystem> Members

        TargetSystem IDetail<TargetSystem>.Master
        {
            get { return this.TargetSystem; }
        }

        #endregion
    }
}
