namespace Framework.FileGeneration;

public interface IRenderingFile<out TRenderData> : IFileHeader
{
    TRenderData GetRenderData();
}
