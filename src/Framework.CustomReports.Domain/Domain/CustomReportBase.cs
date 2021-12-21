using System;
using Framework.Core;

namespace Framework.CustomReports.Domain
{
    public abstract class CustomReportBase<TSecurityOperation, TParameter> : ICustomReport<TSecurityOperation>
        where TParameter : new()
    {
        public abstract Guid Id { get; }

        public virtual string Name => this.GetType().Name.SplitByUpper().Join(string.Empty);

        public virtual string Description => string.Empty;

        public abstract IAccessReportRight AccessReportRight { get; }

        public Type ParameterType => typeof(TParameter);

        public abstract TSecurityOperation SecurityOperation { get; }
    }
}
