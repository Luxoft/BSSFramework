using System;

using JetBrains.Annotations;

namespace Framework.Exceptions;

public class StaleDomainObjectStateException : ServiceFacadeException
{
    public Type DomainObjectType { get; private set; }
    public object DomainObjectIdent { get; private set; }

    public StaleDomainObjectStateException([NotNull]Exception exception, Type domainObjectType, object domainObjectIdent)
            : base(exception, $"Object '{domainObjectType.Name}' was updated or deleted by another transaction")
    {
        this.DomainObjectIdent = domainObjectIdent;
        this.DomainObjectType = domainObjectType;
    }
}
