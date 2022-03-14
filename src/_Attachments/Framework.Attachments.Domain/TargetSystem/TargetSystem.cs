using System.Collections.Generic;

using Framework.DomainDriven.Serialization;
using Framework.Persistent;
using Framework.Persistent.Mapping;
using Framework.Restriction;

namespace Framework.Attachments.Domain
{
    /// <summary>
    /// Целевая сиcтема
    /// </summary>
    /// <remarks>
    /// Целевая сиcтема может содержать подсистемы
    /// </remarks>
    [View]
    [Table(Name = nameof(TargetSystem), Schema = "configuration")]
    public class TargetSystem : BaseDirectory, IMaster<DomainType>
    {
        private readonly ICollection<DomainType> domainTypes = new List<DomainType>();

        /// <summary>
        /// Конструктор
        /// </summary>
        public TargetSystem()
        {
        }

        /// <summary>
        /// Коллекция доменных типов целевой системы
        /// </summary>
        [UniqueGroup]
        [CustomSerialization(CustomSerializationMode.ReadOnly)]
        public virtual IEnumerable<DomainType> DomainTypes
        {
            get { return this.domainTypes; }
        }

        #region IMaster<DomainType> Members

        ICollection<DomainType> IMaster<DomainType>.Details
        {
            get { return (ICollection<DomainType>)this.DomainTypes; }
        }

        #endregion
    }
}
