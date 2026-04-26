using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Reflection;

using Framework.Application.Domain;
using Framework.Database;
using Framework.FileGeneration.Configuration;

using Anch.SecuritySystem;

namespace Framework.CodeGeneration.Configuration;

public abstract class CodeGenerationEnvironment<TDomainObjectBase, TPersistentDomainObjectBase, TAuditPersistentDomainObjectBase, TIdent>(
    Expression<Func<TPersistentDomainObjectBase, TIdent>> identityPropertyExpr,
    Assembly? modelAssembly = null)
    : FileGenerationEnvironment<TDomainObjectBase, TPersistentDomainObjectBase, TAuditPersistentDomainObjectBase, TIdent>(identityPropertyExpr, modelAssembly),
        ICodeGenerationEnvironment
    where TPersistentDomainObjectBase : TDomainObjectBase, IIdentityObject<TIdent>
    where TAuditPersistentDomainObjectBase : TPersistentDomainObjectBase, IAuditObject
{
    public virtual ImmutableArray<Type> SecurityRuleTypeList { get; } = [typeof(SecurityRole)];

    public virtual bool IsHierarchical(Type type) => false;
}
