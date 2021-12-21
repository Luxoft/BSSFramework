namespace Framework.DomainDriven.BLL.Security
{
    public interface IImpersonateObject<out T>
    {
        T Impersonate(string principalName);
    }
}