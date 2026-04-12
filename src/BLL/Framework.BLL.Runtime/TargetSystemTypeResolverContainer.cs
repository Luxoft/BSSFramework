using Framework.BLL.Domain.TargetSystem;
using Framework.Core;
using Framework.Core.TypeResolving;

namespace Framework.BLL;

public class TargetSystemTypeResolverContainer(IEnumerable<TargetSystemInfo> targetSystemInfoList) : ITargetSystemTypeResolverContainer
{
    public ITypeResolver<string> TypeResolverS => field ??= targetSystemInfoList.Select(v => v.TypeResolver).ToComposite();

    public ITypeResolver<TypeNameIdentity> TypeResolver => field ??= this.TypeResolverS.OverrideInput((TypeNameIdentity v) => v.ToString());
}
