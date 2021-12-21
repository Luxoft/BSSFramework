using Framework.DomainDriven;
using Framework.Persistent;

namespace Framework.Workflow.Domain.Definition
{
    public abstract partial class ObjectMetadata : IMiniObjectMetadata
    {
    }

    //[ProjectionContract(typeof(ObjectMetadata))]
    public interface IMiniObjectMetadata : IDefaultIdentityObject, IVisualIdentityObject
    {
        string Value { get; }
    }
}