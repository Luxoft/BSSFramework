using Framework.DomainDriven.Attributes;
using Framework.DomainDriven.BLL;
using Framework.Persistent;
using Framework.Restriction;
using Framework.Validation;

namespace Framework.Configuration.Domain
{
    /// <summary>
    /// Уникальный номер элемента системы
    /// </summary>
    [UniqueGroup]
    [BLLViewRole, BLLSaveRole, BLLRemoveRole]
    [ConfigurationViewDomainObject(ConfigurationSecurityOperationCode.SequenceView)]
    [ConfigurationEditDomainObject(ConfigurationSecurityOperationCode.SequenceEdit)]
    [NotAuditedClass]
    public class Sequence : BaseDirectory, INumberObject<long>
    {
        private long number;

        /// <summary>
        /// Значение элемента
        /// </summary>
        [Int64ValueValidator(Min = 0)]
        public virtual long Number
        {
            get { return this.number; }
            set { this.number = value; }
        }
    }
}