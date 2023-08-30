namespace Framework.Core;

public interface IQueryableProcessor<T>
{
    IQueryable<T> Process(IQueryable<T> baseQueryable);
}
