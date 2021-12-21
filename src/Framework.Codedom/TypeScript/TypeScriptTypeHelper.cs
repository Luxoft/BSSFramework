using System;
using System.Reflection;

using Framework.Core;

namespace Framework.CodeDom.TypeScript
{
    public static class TypeScriptTypeHelper
    {
        public static readonly Type MaybeFactory = new StaticTypeScriptType(nameof(MaybeFactory), "FrameworkCore");

        public static readonly Type UpdateItemDataFactory = new StaticTypeScriptType(nameof(UpdateItemDataFactory), "FrameworkCore");

        private class StaticTypeScriptType : BaseTypeImpl
        {
            public StaticTypeScriptType(string name, string @namespace)
            {
                this.Name = name;
                this.Namespace = @namespace;
            }


            public override string Name { get; }

            public override string Namespace { get; }

            protected override bool IsArrayImpl() => false;

            protected override TypeAttributes GetAttributeFlagsImpl()
            { 
                return TypeAttributes.Class;
            }
        }
    }
}
