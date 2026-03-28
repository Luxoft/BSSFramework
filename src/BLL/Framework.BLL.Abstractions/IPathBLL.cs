namespace Framework.BLL;

public interface IPathBLL<out TDomainObject>
{
    TDomainObject GetByPath(string path);
}
