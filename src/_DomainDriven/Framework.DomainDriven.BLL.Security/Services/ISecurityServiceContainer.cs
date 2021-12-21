namespace Framework.DomainDriven.BLL.Security
{
    /// <summary>
    ///  онтейнер сервиса дл¤ проверок доступа
    /// </summary>
    /// <typeparam name="TSecurityService"></typeparam>
    public interface ISecurityServiceContainer<out TSecurityService>
    {
        TSecurityService SecurityService { get; }
    }
}