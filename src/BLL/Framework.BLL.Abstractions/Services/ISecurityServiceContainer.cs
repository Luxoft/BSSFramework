namespace Framework.BLL.Services;

/// <summary>
///  Контейнер сервиса дл¤ проверок доступа
/// </summary>
/// <typeparam name="TSecurityService"></typeparam>
public interface ISecurityServiceContainer<out TSecurityService>
{
    TSecurityService SecurityService { get; }
}
