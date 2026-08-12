using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;

using Anch.Core;

namespace Framework.Core.AnonymousTypeBuilder;

public class AnonymousTypeByFieldBuilder<TMap, TMapMember>(IAnonymousTypeBuilderStorage storage) : AnonymousTypeByMemberBuilder<TMap, TMapMember, FieldBuilder>(storage)
    where TMap : class, ITypeMap<TMapMember>
    where TMapMember : ITypeMapMember
{
    protected override FieldBuilder ImplementMember(TypeBuilder typeBuilder, TMapMember member) => typeBuilder.DefineField(member.Name, member.Type, FieldAttributes.Public);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static readonly Lazy<AnonymousTypeByFieldBuilder<TMap, TMapMember>> LazyDefault = LazyHelper.Create(() => new AnonymousTypeByFieldBuilder<TMap, TMapMember>(new AnonymousTypeBuilderStorage("DefaultByField_" + typeof(TMap).Name)));

    public static AnonymousTypeByFieldBuilder<TMap, TMapMember> Default => LazyDefault.Value;
}
