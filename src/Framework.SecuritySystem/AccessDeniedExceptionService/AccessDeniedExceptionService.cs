using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.Persistent;

using Framework.SecuritySystem.Exceptions;

namespace Framework.SecuritySystem;

public class AccessDeniedExceptionService<TPersistentDomainObjectBase, TIdent> : IAccessDeniedExceptionService<TPersistentDomainObjectBase>
        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
{
    public Exception GetAccessDeniedException(string message) =>
            new AccessDeniedException<TIdent>(string.Empty, default, message);

    public Exception GetAccessDeniedException<TDomainObject>(TDomainObject domainObject, IReadOnlyDictionary<string, object> extensions = null, Func<string, string> formatMessageFunc = null)
            where TDomainObject : class, TPersistentDomainObjectBase =>
            new AccessDeniedException<TIdent>(
                                              typeof(TDomainObject).Name,
                                              domainObject.Id,
                                              this.GetAccessDeniedExceptionMessage(
                                                                                   domainObject,
                                                                                   extensions,
                                                                                   formatMessageFunc ?? (v => v)));

    protected virtual string GetAccessDeniedExceptionMessage<TDomainObject>(TDomainObject domainObject, IReadOnlyDictionary<string, object> extensions = null, Func<string, string> formatMessageFunc = null)
            where TDomainObject : class, TPersistentDomainObjectBase
    {
        if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

        var displayExtensions = extensions.EmptyIfNull().Where(pair => !pair.Key.EndsWith("_Raw")).ToDictionary();

        var elements = this.GetAccessDeniedExceptionMessageElements(domainObject).Concat(displayExtensions).ToDictionary();

        return elements.GetByFirst((first, other) =>
                                   {
                                       var messagePrefix = domainObject.Id.IsDefault() ? "You have no permissions to create object"

                                                                   : "You have no permissions to access object";

                                       var messageBody = $" with {this.PrintElement(first.Key, first.Value)}";

                                       var messagePostfix = other.Any() ? $" ({other.Join(", ", pair => this.PrintElement(pair.Key, pair.Value))})" : "";

                                       var realFormatMessageFunc = formatMessageFunc ?? (v => v);

                                       return realFormatMessageFunc(messagePrefix + messageBody + messagePostfix);
                                   });
    }

    protected virtual string PrintElement(string key, object value)
    {
        return $"{key} = '{value}'";
    }

    protected virtual IEnumerable<KeyValuePair<string, object>> GetAccessDeniedExceptionMessageElements<TDomainObject>(TDomainObject domainObject)
            where TDomainObject : class, TPersistentDomainObjectBase
    {
        if (domainObject is IVisualIdentityObject identityObject)
        {
            var name = identityObject.Name;

            yield return new KeyValuePair<string, object>("name", name);
        }

        yield return new KeyValuePair<string, object>("type", typeof(TDomainObject).Name);

        yield return new KeyValuePair<string, object>("id", domainObject.Id);
    }
}
