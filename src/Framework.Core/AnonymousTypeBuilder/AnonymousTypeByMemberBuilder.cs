using System.Reflection;
using System.Reflection.Emit;

using CommonFramework;

namespace Framework.Core;

public abstract class AnonymousTypeByMemberBuilder<TMap, TMapMember, TMemberBuilder> : IAnonymousTypeBuilder<TMap>
        where TMap : class, ITypeMap<TMapMember>
        where TMapMember: ITypeMapMember
{
    private readonly IAnonymousTypeBuilderStorage _storage;

    private readonly Dictionary<Type, Type> _typeBuilderCache = new Dictionary<Type, Type>();


    protected AnonymousTypeByMemberBuilder(IAnonymousTypeBuilderStorage storage)
    {
        if (storage == null) throw new ArgumentNullException(nameof(storage));

        this._storage = storage;
    }


    protected virtual TypeBuilder DefineType(TMap typeMap)
    {
        if (typeMap == null) throw new ArgumentNullException(nameof(typeMap));

        return this._storage.ModuleBuilder.DefineType(typeMap.Name, TypeAttributes.Class | TypeAttributes.Public);
    }



    protected abstract TMemberBuilder ImplementMember(TypeBuilder typeBuilder, TMapMember member);


    protected virtual void PostBuildType(TMap typeMap, TypeBuilder typeBuilder, TMemberBuilder[] members)
    {

    }

    public Type GetAnonymousType(TMap typeMap)
    {
        if (typeMap == null) throw new ArgumentNullException(nameof(typeMap));

        var typeBuilder = this.DefineType(typeMap);


        var membersRequest = from member in typeMap.Members

                             let memberType = this._typeBuilderCache.GetValueOrDefault(member.Type, () => member.Type)

                             select this.ImplementMember(typeBuilder, member);

        this.PostBuildType(typeMap, typeBuilder, membersRequest.ToArray());

        var type = typeBuilder.CreateTypeInfo();

        this._typeBuilderCache.Add(type, typeBuilder);

        return type;
    }
}
