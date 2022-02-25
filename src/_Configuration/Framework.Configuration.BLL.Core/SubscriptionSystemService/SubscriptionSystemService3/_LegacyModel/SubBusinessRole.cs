using System;
using System.Collections.Generic;

using Framework.Persistent;

namespace Framework.Configuration.Domain
{
    /// <summary>
    /// Связь между подпиской и бизнес-ролью
    /// </summary>
    public class SubBusinessRole : IIdentityObject<Guid>
    {
        private Guid businessRoleId;

        public SubBusinessRole()
        {
        }

        public SubBusinessRole(Subscription subscription)
        {
            this.Subscription = subscription ?? throw new ArgumentNullException(nameof(subscription));
            ((ICollection<SubBusinessRole>)this.Subscription.SubBusinessRoles).Add(this);
        }

        public virtual Subscription Subscription { get; set; }

        /// <summary>
        /// ID бизнес-роли
        /// </summary>
        public virtual Guid BusinessRoleId
        {
            get { return this.businessRoleId; }
            set { this.businessRoleId = value; }
        }

        Guid IIdentityObject<Guid>.Id => Guid.Empty;
    }
}
