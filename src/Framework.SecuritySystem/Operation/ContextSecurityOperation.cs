using System;

using Framework.Core;
using Framework.HierarchicalExpand;

namespace Framework.SecuritySystem
{
    /// <summary>
    /// Констектстная операция доступа
    /// </summary>
    /// <typeparam name="TSecurityOperationCode">Код контекстной операции (Enum)</typeparam>
    public class ContextSecurityOperation<TSecurityOperationCode> : SecurityOperation<TSecurityOperationCode>
        where TSecurityOperationCode : struct, Enum
    {
        public ContextSecurityOperation(TSecurityOperationCode code, HierarchicalExpandType securityExpandType)
            : base(code)
        {
            if (this.Code.IsDefault()) { throw new ArgumentOutOfRangeException(nameof(code)); }

            this.SecurityExpandType = securityExpandType;
        }

        public HierarchicalExpandType SecurityExpandType { get; private set; }


        public override string ToString()
        {
            return $"{base.ToString()} | ExpandType = {this.SecurityExpandType}";
        }

        public ContextSecurityOperation<TSecurityOperationCode> OverrideExpand(HierarchicalExpandType securityExpandType)
        {
            return new ContextSecurityOperation<TSecurityOperationCode>(this.Code, securityExpandType);
        }
    }
}
