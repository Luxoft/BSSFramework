﻿using Framework.SecuritySystem;

namespace Framework.Authorization;

public static class AuthorizationSecurityOperation
{
    public static readonly DisabledSecurityOperation Disabled = new();

    public static readonly NonContextSecurityOperation<Guid> PrincipalView = new(nameof(PrincipalView), new Guid("{5031A272-B730-4E65-9D56-50B3E0441C4F"));

    public static readonly NonContextSecurityOperation<Guid> PrincipalEdit = new(nameof(PrincipalEdit), new Guid("{3DC58D7B-85A0-43E1-8E54-E23F9A360E7B}"));

    public static readonly NonContextSecurityOperation<Guid> BusinessRoleView = new(nameof(BusinessRoleView), new Guid("{641CB98E-F47D-4F4A-9183-D969BA1434D4}"));

    public static readonly NonContextSecurityOperation<Guid> BusinessRoleEdit = new(nameof(BusinessRoleEdit), new Guid("{44F519AE-ACD8-487B-94BA-BBD897DF687A}"));

    public static readonly NonContextSecurityOperation<Guid> OperationView = new(nameof(OperationView), new Guid("{9B0F5A86-5ECC-44DB-B325-8F2FCD7C2E46}"));

    public static readonly NonContextSecurityOperation<Guid> OperationEdit = new(nameof(OperationEdit), new Guid("{7D148DAD-D4F7-45E3-B087-2125ABCD8A58}"));

    public static readonly NonContextSecurityOperation<Guid> AuthorizationImpersonate = new(nameof(BusinessRoleEdit), new Guid("{E48D7030-FC38-4416-8C7F-F08764D884E3}"));
}
