namespace Framework.Authorization.Domain
{
    /// <summary>
    /// Класс представляет значения для конфигурирования авторизации
    /// </summary>
    public class Settings : DomainObjectBase
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="delegatePermissionLevel">Уровень делегирования</param>
        /// <param name="delegatePermissionRoleType">Тип делегирования</param>
        public Settings(DelegatePermissionLevel delegatePermissionLevel, DelegatePermissionRoleLevel delegatePermissionRoleType)
        {
            this.DelegatePermissionLevel = delegatePermissionLevel;
            this.DelegatePermissionRoleType = delegatePermissionRoleType;
        }

        /// <summary>
        /// Уровень делегирвоания
        /// </summary>
        public DelegatePermissionLevel DelegatePermissionLevel { get; private set; }

        /// <summary>
        /// Тип делегирвоания
        /// </summary>
        public DelegatePermissionRoleLevel DelegatePermissionRoleType { get; private set; }
    }
}