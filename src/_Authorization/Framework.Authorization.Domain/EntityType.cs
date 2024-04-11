﻿using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.DomainDriven.Tracking.LegacyValidators;
using Framework.SecuritySystem.ExternalSystem;

namespace Framework.Authorization.Domain;

/// <summary>
/// Описание доменных типов, в контексте которых выдаются права пользователю
/// </summary>
/// <remarks>
/// В коде контекст описывается следующими сущностями:
/// <seealso cref="SecurityContextType"/>
/// <seealso cref="PermissionRestriction"/>
/// Типы, в контексте которых выдаются права пользователю, записываются вручную на уровне SQL в базу конкретной системы
/// </remarks>
[BLLViewRole]
public class SecurityContextType : BaseDirectory, ISecurityContextType<Guid>
{
    /// <summary>
    /// Вычисляемое название доменного типа
    /// </summary>
    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    public override string Name
    {
        get { return base.Name; }
        set { base.Name = value; }
    }
}
