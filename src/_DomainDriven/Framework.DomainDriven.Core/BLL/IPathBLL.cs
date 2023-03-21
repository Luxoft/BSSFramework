using JetBrains.Annotations;

namespace Framework.DomainDriven.BLL;

public interface IPathBLL<out TDomainObject>
{
    TDomainObject GetByPath([NotNull]string path);
}
