using System;
using System.Collections.Generic;
using System.Reflection;

using Framework.Workflow.Domain.Definition;

using JetBrains.Annotations;

namespace Framework.Workflow.BLL
{
    public partial interface ITargetSystemBLL
    {
        TargetSystem RegisterBase();

        TargetSystem Register<TPersistentDomainObjectBase>(bool isMain, [NotNull]IEnumerable<Assembly> assemblies);
    }

    public static class TargetSystemBLLExtensions
    {
        public static TargetSystem Register<TPersistentDomainObjectBase>(this ITargetSystemBLL bll, bool isMain)
        {
            if (bll == null) throw new ArgumentNullException(nameof(bll));

            return bll.Register<TPersistentDomainObjectBase>(isMain, new[] { typeof(TPersistentDomainObjectBase).Assembly });
        }
    }
}
