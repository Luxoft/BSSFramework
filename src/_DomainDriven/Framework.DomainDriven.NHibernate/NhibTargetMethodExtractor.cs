using GenericQueryable.Services;

using NHibernate.Linq;

namespace Framework.DomainDriven.NHibernate;

public class NhibTargetMethodExtractor : TargetMethodExtractor
{
    protected override IReadOnlyList<Type> ExtensionsTypes { get; } = [typeof(LinqExtensionMethods)];
}
