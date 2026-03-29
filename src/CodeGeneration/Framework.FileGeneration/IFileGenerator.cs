namespace Framework.FileGeneration;

public interface IFileGenerator<out TFileFactory>
{
    IEnumerable<TFileFactory> GetFileGenerators();
}

public interface IFileGenerator<out TFileFactory, out TRenderer> : IFileGenerator<TFileFactory>
{
    TRenderer Renderer { get; }
}
