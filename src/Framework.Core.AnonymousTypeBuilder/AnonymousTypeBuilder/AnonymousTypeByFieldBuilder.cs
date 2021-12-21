using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;

namespace Framework.Core
{
    public class AnonymousTypeByFieldBuilder<TMap, TMapMember> : AnonymousTypeByMemberBuilder<TMap, TMapMember, FieldBuilder>
        where TMap : class, ITypeMap<TMapMember>
        where TMapMember : ITypeMapMember
    {
        public AnonymousTypeByFieldBuilder(IAnonymousTypeBuilderStorage storage) : base(storage)
        {

        }


        protected override FieldBuilder ImplementMember(TypeBuilder typeBuilder, TMapMember member)
        {
            return typeBuilder.DefineField(member.Name, member.Type, FieldAttributes.Public);
        }


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Lazy<AnonymousTypeByFieldBuilder<TMap, TMapMember>> LazyDefault = LazyHelper.Create(() => new AnonymousTypeByFieldBuilder<TMap, TMapMember>(new AnonymousTypeBuilderStorage("DefaultByField_" + typeof(TMap).Name)));

        public static AnonymousTypeByFieldBuilder<TMap, TMapMember> Default
        {
            get { return LazyDefault.Value; }
        }
    }
}