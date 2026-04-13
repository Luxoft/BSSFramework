using System.Collections.Immutable;

using Framework.Core;
using Framework.Core.TypeResolving;

namespace Framework.BLL.Domain.TargetSystem;

public record TargetSystemDomainInfo(ImmutableArray<DomainTypeInfo> Types, ITypeResolver<TypeNameIdentity> TypeResolver)
{
    public TargetSystemDomainInfo(ImmutableArray<DomainTypeInfo> domainTypes)
        : this(
            domainTypes,
            TypeResolverHelper.Create(new TypeSource([.. domainTypes.Select(v => v.Type)]), TypeSearchMode.Both))
    {
    }

    public static TargetSystemDomainInfo Base { get; } = new(
    [
        new DomainTypeInfo(typeof(string), new("{0255b380-68f9-43d5-a731-daf3b860ad09}")),
        new DomainTypeInfo(typeof(bool), new("{21B0FF17-B9E2-4F66-942D-2DFCA09DE861}")),
        new DomainTypeInfo(typeof(Guid), new("{24CEE0A5-330F-4B14-8C64-F4245F79FC6B}")),
        new DomainTypeInfo(typeof(int), new("{73F41360-864F-4C73-B5B3-893A6DF3E400}")),
        new DomainTypeInfo(typeof(DateTime), new("{4A4D65CB-C4A8-4EBC-A1DD-06C00A25D728}")),
        new DomainTypeInfo(typeof(decimal), new("{9499A3CB-26DB-4803-9C53-BB93A6645338}")),
        new DomainTypeInfo(typeof(byte), new("{FECF4BEF-DC2F-44B0-AE9C-9A28B1C5AD3A}")),
        new DomainTypeInfo(typeof(double), new("{68F69CA7-263B-4559-BD96-4A13A28823CC}")),
    ]);
}
