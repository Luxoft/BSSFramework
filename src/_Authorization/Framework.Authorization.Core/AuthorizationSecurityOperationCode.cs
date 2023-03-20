using Framework.Security;

namespace Framework.Authorization;

public enum AuthorizationSecurityOperationCode
{
    Disabled = 0,

    [SecurityOperation(SecurityOperationCode.PrincipalView)]
    PrincipalView,

    [SecurityOperation(SecurityOperationCode.PrincipalEdit)]
    PrincipalEdit,

    [SecurityOperation(SecurityOperationCode.BusinessRoleView)]
    BusinessRoleView,

    [SecurityOperation(SecurityOperationCode.BusinessRoleEdit)]
    BusinessRoleEdit,

    [SecurityOperation(SecurityOperationCode.OperationView)]
    OperationView,

    [SecurityOperation(SecurityOperationCode.OperationEdit)]
    OperationEdit,

    [SecurityOperation(SecurityOperationCode.AuthorizationImpersonate)]
    AuthorizationImpersonate,
}
