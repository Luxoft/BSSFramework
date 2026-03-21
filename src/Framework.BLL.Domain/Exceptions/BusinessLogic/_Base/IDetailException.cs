namespace Framework.BLL.Domain.Exceptions.BusinessLogic._Base;

public interface IDetailException<out TDetail>
{
    TDetail Detail { get; }
}
