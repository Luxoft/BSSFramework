using System;

using Framework.Core;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;
using Framework.Projection.Contract;

namespace Framework.Workflow.Domain.Definition
{
    public partial class CommandParameter : IMiniCommandParameter
    {
        IMiniCommand IMiniCommandParameter.Command => this.Command;
    }

    [ProjectionContract(typeof(CommandParameter))]
    public interface IMiniCommandParameter : IVisualIdentityObject, IDefaultIdentityObject
    {
        [CustomSerialization(CustomSerializationMode.Ignore)]
        IMiniCommand Command { get; }

        bool IsReadOnly { get; }
    }
}