using System;

namespace Framework.Configuration.Domain
{
    /// <summary>
    /// Связь между подпиской и бизнес-ролью
    /// </summary>
    public class SubBusinessRole
    {
        private Guid businessRoleId;
        
        /// <summary>
        /// ID бизнес-роли
        /// </summary>
        public virtual Guid BusinessRoleId
        {
            get { return this.businessRoleId; }
            set { this.businessRoleId = value; }
        }
    }
}
