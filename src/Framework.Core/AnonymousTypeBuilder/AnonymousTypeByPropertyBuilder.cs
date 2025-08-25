using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;

using CommonFramework;

namespace Framework.Core;

public class AnonymousTypeByPropertyBuilder<TMap, TMapMember> : AnonymousTypeByMemberBuilder<TMap, TMapMember, PropertyBuilder>
        where TMap : class, ITypeMap<TMapMember>
        where TMapMember : ITypeMapMember
{
    public AnonymousTypeByPropertyBuilder(IAnonymousTypeBuilderStorage storage) :
            base(storage)
    {
    }

    protected override PropertyBuilder ImplementMember(TypeBuilder typeBuilder, TMapMember member)
    {
        var fieldBuilder = typeBuilder.DefineField("_" + member.Name, member.Type, FieldAttributes.Private);

        var propertyBuilder = typeBuilder.DefineProperty(member.Name, PropertyAttributes.None, member.Type, Type.EmptyTypes);

        {
            var getMethod = typeBuilder.DefineMethod("get_" + member.Name, MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.NewSlot | MethodAttributes.HideBySig | MethodAttributes.SpecialName, member.Type, Type.EmptyTypes);

            var ilGenerator = getMethod.GetILGenerator();
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldfld, fieldBuilder);
            ilGenerator.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getMethod);
        }

        {
            var setMethod = typeBuilder.DefineMethod("set_" + member.Name, MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.NewSlot | MethodAttributes.HideBySig | MethodAttributes.SpecialName, typeof(void), new[] { member.Type });

            var ilGenerator = setMethod.GetILGenerator();
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldarg_1);
            ilGenerator.Emit(OpCodes.Stfld, fieldBuilder);
            ilGenerator.Emit(OpCodes.Ret);

            propertyBuilder.SetSetMethod(setMethod);
        }

        return propertyBuilder;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static readonly Lazy<AnonymousTypeByPropertyBuilder<TMap, TMapMember>> LazyDefault = LazyHelper.Create(() => new AnonymousTypeByPropertyBuilder<TMap, TMapMember>(new AnonymousTypeBuilderStorage("DefaultByProperty_" + typeof(TMap).Name)));

    public static AnonymousTypeByPropertyBuilder<TMap, TMapMember> Default
    {
        get { return LazyDefault.Value; }
    }
}
