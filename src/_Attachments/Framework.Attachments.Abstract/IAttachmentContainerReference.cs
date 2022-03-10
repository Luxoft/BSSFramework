using System;

namespace Framework.Attachments.Abstract
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
