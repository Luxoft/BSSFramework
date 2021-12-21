using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Framework.Core;

using JetBrains.Annotations;

namespace Framework.DomainDriven.Metadata
{
    public class AssemblyInfo : IAssemblyInfo
    {
        private readonly ITypeSource typeSource;

        public AssemblyInfo([NotNull] string name, [NotNull] string fullName, ITypeSource typeSource)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
            if (string.IsNullOrWhiteSpace(fullName)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(fullName));
            if (typeSource == null) throw new ArgumentNullException(nameof(typeSource));

            this.Name = name;
            this.FullName = fullName;
            this.typeSource = typeSource;
        }

        public string Name { get; }

        public string FullName { get; }


        public IEnumerable<Type> GetTypes()
        {
            return this.typeSource.GetTypes();
        }

        public static AssemblyInfo Create([NotNull] Assembly assembly, Func<Type, bool> typeFilter = null)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));

            var typeSource = typeFilter == null ? new TypeSource(assembly) : new TypeSource(assembly.GetTypes().Where(typeFilter).ToArray());

            return new AssemblyInfo(assembly.GetName().Name, assembly.FullName, typeSource);
        }
    }
}
