using System;
using System.CodeDom;
using System.Reflection;

namespace Framework.DomainDriven.FacadeServiceProxyGenerator
{
    public struct GenerateAsyncFuncInfo
    {
        public Type BaseTypedAsyncFuncType { get; set; }

        public CodeTypeReference[] Generics { get; set; }

        public CodeMemberProperty Property { get; set; }

        public MethodInfo SourceMethod { get; set; }

        public bool IsContravariant { get; set; }
    }
}
