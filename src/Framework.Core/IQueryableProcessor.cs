using JetBrains.Annotations;

namespace Framework.Core;

public interface IQueryableProcessor<T>
{
    IQueryable<T> Process([NotNull] IQueryable<T> baseQueryable);
}
