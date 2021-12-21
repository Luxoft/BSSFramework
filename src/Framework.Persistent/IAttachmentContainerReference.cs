using System;

namespace Framework.Persistent
{
    public interface IAttachmentContainerReference<out T, out TIdent>
    {
        T DomainType { get; }

        TIdent ObjectId { get; }
    }

    public interface IAttachmentContainerReference<out T> : IAttachmentContainerReference<T, Guid>
    {
    }
}