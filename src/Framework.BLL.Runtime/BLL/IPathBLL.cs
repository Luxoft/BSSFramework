

namespace Framework.BLL.BLL;

public interface IPathBLL<out TDomainObject>
{
    TDomainObject GetByPath(string path);
}
