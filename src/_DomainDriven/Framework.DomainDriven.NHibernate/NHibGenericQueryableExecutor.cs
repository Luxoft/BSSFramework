using Framework.GenericQueryable;

using NHibernate.Linq;

namespace Framework.DomainDriven.NHibernate;

public class NHibGenericQueryableExecutor : GenericQueryableExecutor
{
    protected override Type ExtensionsType { get; } = typeof(LinqExtensionMethods);
}
