using System.Reflection.Emit;
using System.Runtime.Serialization;

namespace Framework.Core;

public class AnonymousTypeByPropertyWithSerializeBuilder<TMap, TMapMember> : AnonymousTypeByPropertyBuilder<TMap, TMapMember>
        where TMap : class, ITypeMap<TMapMember>
        where TMapMember : ITypeMapMember
{
    public AnonymousTypeByPropertyWithSerializeBuilder(IAnonymousTypeBuilderStorage storage)
            : base(storage)
    {
    }

    protected override TypeBuilder DefineType(TMap typeMap)
    {
        var typeBuilder = base.DefineType(typeMap);

        typeBuilder.SetCustomAttribute(this.GetDataContractAttribute(typeMap));

        return typeBuilder;
    }

    protected override PropertyBuilder ImplementMember(TypeBuilder typeBuilder, TMapMember member)
    {
        var propertyBuilder = base.ImplementMember(typeBuilder, member);

        propertyBuilder.SetCustomAttribute(this.GetDataMemberAttribute(member));

        return propertyBuilder;
    }

    protected virtual CustomAttributeBuilder GetDataContractAttribute(TMap typeMap)
    {
        return new CustomAttributeBuilder(typeof(DataContractAttribute).GetConstructor(Type.EmptyTypes), new object[0]);
    }

    protected virtual CustomAttributeBuilder GetDataMemberAttribute(TMapMember member)
    {
        return new CustomAttributeBuilder(typeof (DataMemberAttribute).GetConstructor(Type.EmptyTypes), new object[0]);
    }
}
