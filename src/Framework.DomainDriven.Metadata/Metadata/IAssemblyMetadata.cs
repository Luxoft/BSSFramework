using System.Collections.Generic;

using Framework.DomainDriven.BLL;

namespace Framework.DomainDriven.Metadata
{
    public interface IAssemblyMetadata : IPersistentDomainObjectBaseTypeContainer
    {
        IEnumerable<DomainTypeMetadata> DomainTypes { get; }
    }
}
