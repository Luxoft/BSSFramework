using JetBrains.Annotations;

namespace Framework.DomainDriven
{
    public interface IMappingService<in TSource, in TTarget>
    {
        void Map([NotNull]TSource source, [NotNull]TTarget target);
    }
}
