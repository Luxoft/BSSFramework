using System;
using System.Collections.Generic;

using Framework.Core;

using JetBrains.Annotations;

namespace Framework.DomainDriven.BLL
{
    public interface IOperationBLLBase<in TDomainObject>
    {
        void Save([NotNull] TDomainObject domainObject);

        void Remove([NotNull] TDomainObject domainObject);
    }

    public static class OperationBLLBaseExtensions
    {
        public static void Save<TDomainObject>([NotNull] this IOperationBLLBase<TDomainObject> bll, [NotNull] IEnumerable<TDomainObject> domainObjects)
        {
            if (bll == null) throw new ArgumentNullException(nameof(bll));
            if (domainObjects == null) throw new ArgumentNullException(nameof(domainObjects));

            domainObjects.Foreach(bll.Save);
        }

        public static void Remove<TDomainObject>([NotNull] this IOperationBLLBase<TDomainObject> bll, [NotNull] IEnumerable<TDomainObject> domainObjects)
        {
            if (bll == null) throw new ArgumentNullException(nameof(bll));
            if (domainObjects == null) throw new ArgumentNullException(nameof(domainObjects));

            domainObjects.Foreach(bll.Remove);
        }
    }
}
