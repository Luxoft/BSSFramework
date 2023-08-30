using System.Net;

using Framework.Core;
using Framework.HierarchicalExpand;

// compile with: /doc:DocFileName.xml
/// text for class SecurityOperationAttribute
namespace Framework.Security;

/// <summary>
/// Описание секьюрной операции
/// </summary>
/// <remarks>
/// Cекьюрная операция - описание действия, на основе которого рассчитывается доступ к каждому объекту системы
/// </remarks>
[AttributeUsage(AttributeTargets.Field)]
public class SecurityOperationAttribute : Attribute
{
    private HierarchicalExpandType? _securityExpandType;

    /// <summary>
    /// Конструктор копирует данные из атрибута секьюрной операции
    /// </summary>
    /// <param name="securityOperationCode"></param>
    public SecurityOperationAttribute(SecurityOperationCode securityOperationCode)
    {
        var attr = securityOperationCode.GetSecurityOperationAttribute();

        this.GroupName = attr.GroupName;
        this.Description = attr.Description;
        this.IsContext = attr.IsContext;
        this.Guid = attr.Guid;
        this.AdminHasAccess = attr.AdminHasAccess;

        this.DomainType = attr.DomainType;
        this.IsClient = attr.IsClient;

        this._securityExpandType = attr._securityExpandType;
    }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="groupName">Название операции</param>
    /// <param name="isContext">Признак контекстной операции</param>
    /// <param name="guidString">Уникальный идентификатор операции</param>
    public SecurityOperationAttribute(string groupName, bool isContext, string guidString)
            : this (groupName, isContext, guidString, null)
    {

    }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="groupName">Название операции</param>
    /// <param name="isContext">Признак контекстной операции</param>
    /// <param name="guidString">Уникальный идентификатор операции</param>
    /// <param name="description">Описание операции</param>
    public SecurityOperationAttribute(string groupName, bool isContext, string guidString, string description)
            : this(groupName, isContext, guidString, description, true)
    {

    }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="groupName">Название операции</param>
    /// <param name="isContext">Признак контекстной операции</param>
    /// <param name="guidString">Уникальный идентификатор операции</param>
    /// <param name="description">Описание операции</param>
    /// <param name="adminHasAccess">Признак того, что админимтратор имеет доступ к операции</param>
    public SecurityOperationAttribute(string groupName, bool isContext, string guidString, string description, bool adminHasAccess)
    {
        this.GroupName = groupName;
        this.Description = description;
        this.IsContext = isContext;
        this.Guid = new Guid(guidString);
        this.AdminHasAccess = adminHasAccess;
        this.DomainType = "";
    }

    /// <summary>
    /// Признак контекстной операци
    /// </summary>
    public bool IsContext { get; private set; }

    /// <summary>
    /// ID операции
    /// </summary>
    /// <remarks>Будет сохранен в базу Authorization <seealso cref="Authorization.Domain.Operation"/></remarks>
    public Guid Guid { get; private set; }

    /// <summary>
    /// Описание секьюрной операции
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// Название группы схожих по смыслу операций
    /// </summary>
    public string GroupName { get; private set; }

    /// <summary>
    /// Признак того, что админимтратор имеет доступ к операции
    /// </summary>
    public bool AdminHasAccess { get; private set; }

    /// <summary>
    /// Типы расширения прав по дереву
    /// </summary>
    public HierarchicalExpandType SecurityExpandType
    {
        get { return this._securityExpandType.ToMaybe().GetValue(() => new Exception("Default expandType not overrided")); }
        set { this._securityExpandType = value; }
    }

    /// <summary>
    /// Вычисляемое свойство SecurityExpandType
    /// </summary>
    /// <remarks>
    /// SecurityExpandType != null, то значение "true", SecurityExpandType == "null", тот значение "false"
    /// </remarks>
    public bool OverridedSecurityExpandType
    {
        get { return this._securityExpandType != null; }
    }

    /// <summary>
    /// Доменный тип
    /// </summary>
    public string DomainType { get; set; }

    /// <summary>
    /// Признак того, что операция используется на клиенту
    /// </summary>
    /// <remarks>
    /// Помеченная флагом IsClient = true операция не используется для генерации секьюрных операций
    /// </remarks>
    public bool IsClient { get; set; }
}
