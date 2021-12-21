using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

using Framework.Core;
using Framework.Validation;

namespace Framework.DomainDriven
{
    public class RemoveLinkedObjectsDALException : DALException<LinkedObjects>
    {
        public RemoveLinkedObjectsDALException(LinkedObjects args, string message)
            : base(args, message)
        {
        }

        public override string Message => $"{this.Args.Target.Name} cannot be removed because it is used in {this.Args.Source.Name}";

        public override ValidationException Convert()
        {
            return new ValidationException(this.Message);
        }
    }

    public struct DomainObjectInfo
    {
        public DomainObjectInfo(Type type, object objectIdent)
            : this()
        {
            this.Type = type;
            this.ObjectIdent = objectIdent;
        }

        public Type Type { get; }

        public object ObjectIdent { get; }
    }

    public struct UniqueConstraint
    {
        private static readonly Regex FieldNameRegex = new Regex("(\\S*)Id");

        public UniqueConstraint(DomainObjectInfo domainObjectInfo, string name, IEnumerable<string> properties)
            : this()
        {
            this.Name = name;
            this.ObjectInfo = domainObjectInfo;
            this.Properties = properties.Select(x => GetName(domainObjectInfo.Type.GetProperties(), x.Trim('[', ']'))).ToReadOnlyCollection();
        }

        public DomainObjectInfo ObjectInfo { get; }

        public string Name { get; }

        public ReadOnlyCollection<string> Properties { get; }

        private static string GetName(ICollection<PropertyInfo> properties, string columnName)
        {
            var property = properties.FirstOrDefault(x => string.Equals(x.Name, GetFieldName(columnName), StringComparison.InvariantCultureIgnoreCase));

            return property != null ? property.GetValidationName() : columnName;
        }

        private static string GetFieldName(string columnName)
        {
            var match = FieldNameRegex.Match(columnName);

            return match.Success ? match.Groups[1].Value : columnName;
        }
    }
}
