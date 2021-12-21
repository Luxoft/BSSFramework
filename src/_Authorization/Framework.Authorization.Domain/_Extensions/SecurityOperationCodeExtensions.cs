using System;

using Framework.DomainDriven.BLL.Security;

namespace Framework.Authorization.Domain
{
    public static class SecurityOperationCodeExtensions
    {
        public static Operation GetAuthorizationOperation<TSecurityOperationCode>(this TSecurityOperationCode secOperation)
            where TSecurityOperationCode : struct, Enum
        {
            var operation = new Operation
            {
                Name = secOperation.ToString(),
                Description = secOperation.GetDescription()
            };

            return operation;
        }
    }
}
