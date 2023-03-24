// compile with: /doc:DocFileName.xml
// text for class Status

namespace Framework.Authorization.Domain;

/// <summary>
/// Константы, описывающие возможные статусы пермиссии
/// </summary>
public enum PermissionStatus
{
    /// <summary>
    /// Статус устанавливается при необходимости подтверждения операции
    /// </summary>
    Approving = 0,

    /// <summary>
    /// Статус устанавливается при отмене запроса на подтвеждение операции
    /// </summary>
    Rejected = 1,

    /// <summary>
    /// Статус устанавливается:
    /// 1) если операция не требует подтверждения
    /// 2) если операция утверждена
    /// </summary>
    Approved = 2
}
