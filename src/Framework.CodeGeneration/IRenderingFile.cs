namespace Framework.CodeGeneration;

public interface IRenderingFile<out TRenderData> : IFileHeader
{
    TRenderData GetRenderData();
}
