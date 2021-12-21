using System;

using Framework.Core;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;

namespace Framework.Workflow.Domain
{
    [TargetSystem("{3A6857B1-E35D-48C8-8375-A34901213552}")]
    public abstract class PersistentDomainObjectBase : DomainObjectBase, IDefaultIdentityObject, IEquatable<PersistentDomainObjectBase>
    {
        private Guid id;

        #region Constructor

        protected PersistentDomainObjectBase()
        {

        }

        #endregion

        /// <summary>
        /// ID доменного объекта
        /// </summary>
        public virtual Guid Id
        {
            get { return this.id; }
            protected internal set { this.id = value; }
        }

        /// <summary>
        /// Признак того, что класс еще не сохранен в базе
        /// </summary>
        [CustomSerialization(CustomSerializationMode.Ignore)]
        public virtual bool IsNew => this.Id == Guid.Empty;

        public static bool operator ==(PersistentDomainObjectBase a, PersistentDomainObjectBase b)
        {
            return object.ReferenceEquals(a, b) || (!object.ReferenceEquals(a, null) && !object.ReferenceEquals(b, null) && a.Equals(b));
        }

        public static bool operator !=(PersistentDomainObjectBase a, PersistentDomainObjectBase b)
        {
            return !(a == b);
        }

        public virtual bool Equals(PersistentDomainObjectBase obj)
        {
            return object.ReferenceEquals(this, obj) || (!object.ReferenceEquals(obj, null) && this.Id == obj.Id && this.Id != Guid.Empty);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as PersistentDomainObjectBase);
        }

        public override int GetHashCode()
        {
            return this.Id.IsDefault() ? base.GetHashCode() : this.Id.GetHashCode();
        }
    }
}