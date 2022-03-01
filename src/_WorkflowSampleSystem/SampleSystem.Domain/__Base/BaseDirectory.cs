using Framework.Core;
using Framework.Persistent;
using Framework.Restriction;

namespace SampleSystem.Domain
{
    /// <summary>
    ///     Базовый персистентный класс для типов с именем
    /// </summary>
    public abstract class BaseDirectory : AuditPersistentDomainObjectBase, IVisualIdentityObject
    {
        /// <summary>
        ///     Название типа
        /// </summary>
        private string name;

        [VisualIdentity]
        [Required]
        [UniqueElement]
        public virtual string Name
        {
            get { return this.name.TrimNull(); }
            set { this.name = value.TrimNull(); }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
