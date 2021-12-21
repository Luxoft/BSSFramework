namespace Framework.DomainDriven.Generation
{
    public interface IRenderingFile<out TRenderData> : IFileHeader
    {
        TRenderData GetRenderData();
    }
}