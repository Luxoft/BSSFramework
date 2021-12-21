using Framework.Core.Serialization;

namespace Framework.OData
{
    public interface ISelectOperationParserContainer
    {
        IParser<string, SelectOperation> SelectOperationParser { get; }
    }
}