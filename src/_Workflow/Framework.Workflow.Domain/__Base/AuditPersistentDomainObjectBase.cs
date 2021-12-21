using System;
using System.ComponentModel;

using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;

namespace Framework.Workflow.Domain
{
    /// <summary>
    /// Базовый персистентный класс
    /// </summary>
    public abstract class AuditPersistentDomainObjectBase : PersistentDomainObjectBase, IDefaultAuditPersistentDomainObjectBase, IVersionObject<long>
    {
        private bool active = true;

        private DateTime? createDate;
        private string createdBy;

        private DateTime? modifyDate;
        private string modifiedBy;

        private long version;

        #region Constructor

        protected AuditPersistentDomainObjectBase()
        {

        }

        #endregion

        /// <summary>
        /// Дата создания доменного объекта
        /// </summary>
        public virtual DateTime? CreateDate
        {
            get { return this.createDate; }
            internal protected set { this.createDate = value; }
        }

        /// <summary>
        /// Логин сотрудника, создавшего доменный объект
        /// </summary>
        public virtual string CreatedBy
        {
            get { return this.createdBy.TrimNull(); }
            internal protected set { this.createdBy = value.TrimNull(); }
        }

        /// <summary>
        /// Дата изменения доменного объекта
        /// </summary>
        [SystemProperty]
        public virtual DateTime? ModifyDate
        {
            get { return this.modifyDate; }
            protected internal set { this.modifyDate = value; }
        }

        /// <summary>
        /// Логин сотрудника, изменившего доменный объект
        /// </summary>
        [SystemProperty]
        public virtual string ModifiedBy
        {
            get { return this.modifiedBy.TrimNull(); }
            protected internal set { this.modifiedBy = value.TrimNull(); }
        }

        /// <summary>
        /// Признак активности доменного объекта
        /// </summary>
        [DefaultValue(true)]
        [CustomSerialization(CustomSerializationMode.ReadOnly)]
        public virtual bool Active
        {
            get { return this.active; }
            set { this.active = value; }
        }

        /// <summary>
        /// Версия последнего изменения доменного объекта
        /// </summary>
        [Version]
        [SystemProperty]
        public virtual long Version
        {
            get { return this.version; }
            set { this.version = value; }
        }
    }
}