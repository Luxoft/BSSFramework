using System;
using System.Collections.Generic;

using Framework.Core;
using Framework.DomainDriven.Attributes;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;

namespace Framework.Configuration.Domain
{
    /// <summary>
    /// Описание высокоуровнего действия пользователя в системе
    /// </summary>
    /// <remarks>
    /// Обычно заполняется из клиента
    /// </remarks>
    [ConfigurationViewDomainObject(ConfigurationSecurityOperationCode.UserActionView)]
    [ConfigurationEditDomainObject(ConfigurationSecurityOperationCode.Disabled)]
    [BLLViewRole]
    [NotAuditedClass]
    [Obsolete("v10.0 Кандидат на вылет в следующих версиях")]
    public class UserAction : AuditPersistentDomainObjectBase, IMaster<UserActionObject>, IVisualIdentityObject
    {
        private DomainType domainType;

        private readonly ICollection<UserActionObject> objectIdentities = new List<UserActionObject>();

        private string userName;

        private string name;

        public UserAction()
        {
        }

        /// <summary>
        /// Доменный тип объекта, над которым производится действие в системе
        /// </summary>
        public virtual DomainType DomainType
        {
            get
            {
                return this.domainType;
            }

            set
            {
                this.domainType = value;
            }
        }

        /// <summary>
        /// Имя высокоуровневой операции
        /// </summary>
        public virtual string Name
        {
            get
            {
                return this.name.TrimNull();
            }

            set
            {
                this.name = value.TrimNull();
            }
        }

        /// <summary>
        /// Логин пользователя
        /// </summary>
        [CustomSerialization(CustomSerializationMode.Ignore)]
        public virtual string UserName
        {
            get
            {
                return this.userName.TrimNull();
            }

            set
            {
                this.userName = value.TrimNull();
            }
        }

        #region IMaster<UserActionObject> Members

        public virtual IEnumerable<UserActionObject> ObjectIdentities
        {
            get
            {
                return this.objectIdentities;
            }
        }

        public virtual void RemoveUserActionObject(UserActionObject userActionObject)
        {
            this.RemoveDetail(userActionObject);
        }

        public virtual void AddUserActionObject(UserActionObject userActionObject)
        {
            this.AddDetail(userActionObject);
        }

        ICollection<UserActionObject> IMaster<UserActionObject>.Details
        {
            get { return (ICollection<UserActionObject>)this.ObjectIdentities; }
        }

        #endregion

        public virtual UserActionObject CreateDetail()
        {
            return new UserActionObject(this);
        }
    }
}
