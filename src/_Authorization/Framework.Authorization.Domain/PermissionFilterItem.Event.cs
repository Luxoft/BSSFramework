using System.Collections.Generic;
using System.Linq;

using Framework.DomainDriven.Serialization;
using Framework.Persistent;

namespace Framework.Authorization.Domain
{
    /// <summary>
    /// Оповещение корпоративной шины (biztalk) об измнениях
    /// </summary>
    public partial class PermissionFilterItem
    {
        /// <summary>
        /// Вычисляемый принципал, содержащийся в пермиссии
        /// </summary>
        [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Integration | DTORole.Client)]
        [ExpandPath("Permission.Principal")]
        public virtual Principal Principal
        {
            get { return this.Permission.Principal; }
        }

        /// <summary>
        /// Вычисляемая роль, содержащийся в пермиссии
        /// </summary>
        [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Integration | DTORole.Client)]
        [ExpandPath("Permission.Role")]
        public virtual BusinessRole Role
        {
            get { return this.Permission.Role; }
        }
    }
}
