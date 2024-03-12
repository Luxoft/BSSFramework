using Framework.Core;

namespace Framework.DomainDriven.BLL;

public interface IOperationBLLBase<in TDomainObject>
{
    void Save(TDomainObject domainObject);

    void Remove(TDomainObject domainObject);
}

public static class OperationBLLBaseExtensions
{
    public static void Save<TDomainObject>(this IOperationBLLBase<TDomainObject> bll, IEnumerable<TDomainObject> domainObjects)
    {
        if (bll == null) throw new ArgumentNullException(nameof(bll));
        if (domainObjects == null) throw new ArgumentNullException(nameof(domainObjects));

        domainObjects.Foreach(bll.Save);
    }

    public static void Remove<TDomainObject>(this IOperationBLLBase<TDomainObject> bll, IEnumerable<TDomainObject> domainObjects)
    {
        if (bll == null) throw new ArgumentNullException(nameof(bll));
        if (domainObjects == null) throw new ArgumentNullException(nameof(domainObjects));

        domainObjects.Foreach(bll.Remove);
    }
}
