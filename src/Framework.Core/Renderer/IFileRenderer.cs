namespace Framework.Core
{
    public interface IFileRenderer
    {
        string FileExtension { get; }
    }

    public interface IFileRenderer<in TSource, out TResult> : IRenderer<TSource, TResult>, IFileRenderer
    {

    }
}