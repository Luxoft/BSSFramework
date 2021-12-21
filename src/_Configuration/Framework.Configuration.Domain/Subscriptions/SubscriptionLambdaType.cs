namespace Framework.Configuration.Domain
{

    /// <summary>
    /// Константы, описывающие типы лямбд
    /// </summary>
    public enum SubscriptionLambdaType
    {
        /// <summary>
        /// Лямбда уcловия выполнения подписки/функция, принимающая предыдущее, текущее состояния объекта и возвращающая true или false
        /// </summary>
        /// <remarks>
        /// Сигнатура: Func<DomainType, DomainType, Boolean>
        /// Пример: (prev, current) => prev.Status != AbsenceRequestStatus.OnApprove && current.Status == AbsenceRequestStatus.OnApprove
        /// Принцип именования: DomainType: Status
        /// </remarks>
        Condition = 0,

        /// <summary>
        /// Лямбда, получающая список нетипизированных контекстов для ролей подписки (определяет контекст роли, по которой отсылается нотификация)
        /// </summary>
        /// <remarks>
        /// Сигнатура: Func<DomainType, DomainType, IEnumerable<FilterItemIdentity>>, где конструктор FilterItemIdentity("DomainTypeName", domainObjectId)
        /// Пример: (prev, current) => new [] {new FilterItemIdentity("ManagementUnit", current.Employee.ManagementUnit.Id)}
        /// Принцип именования: DomainType: DynamicSourceTypeName + "_Source"
        /// </remarks>
        DynamicSource = 1,

        /// <summary>
        /// Лямбда, получающая список получателей из доменного объекта
        /// </summary>
        /// <remarks>
        /// Сигнатура: Func< DomainType, DomainType, IEnumerable<NotificationMessageGenerationInfo>>, где конструктор NotificationMessageGenerationInfo(IEnumerable<IEmployee>,current,prev)} для списка либо NotificationMessageGenerationInfo(IEmployee,current,prev) для одного сотрудника
        /// Примеры: (prev, current) => new[] {new NotificationMessageGenerationInfo(new List<IEmployee>{current.Employee, current.Initiator},current,prev)} -(prev, current) => current.GetRecipients_OnRejected(prev) для определённого в доменном типе метода
        /// Принцип именования: DomainType: Status + "_Generation"
        /// </remarks>
        Generation = 3,

        /// <summary>
        /// Лямбда, получающая список типизированных контекстов для подписки (как и DynamicSource используется в рассылке по ролям)
        /// </summary>
        /// <remarks>
        /// Сигнатура: Func<DomainType, DomainType, IEnumerable<AuthDomainType>>
        /// Пример: (p, c) => new[] { c.Employee.Location }
        /// Принцип именования: DomainType: AuthDomainType + "_Source"
        /// </remarks>
        AuthSource = 4,

        /// <summary>
        /// Лямбда, получающая список аттачей из доменного объекта
        /// </summary>
        /// <remarks>
        /// Сигнатура: Func< DomainType, DomainType, IEnumerable<Attachment>>
        /// </remarks>
        Attachment = 5,
    }
}
