namespace Framework.FileGeneration;

public interface IFileGenerator<out TRenderingData>
{
    IEnumerable<TRenderingData> GetFileGenerators();
}

public interface IFileGenerator<out TRenderingData, out TRenderer> : IFileGenerator<TRenderingData>
{
    TRenderer Renderer { get; }
}
