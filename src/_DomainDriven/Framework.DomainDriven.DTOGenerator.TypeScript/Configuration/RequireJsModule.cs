using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.Configuration
{
    /// <summary>
    /// RequireJs module declaration
    /// </summary>
    public class RequireJsModule
    {
        private static readonly Regex Expression = new Regex(@"\{(?<named>.+)\}|\*\s+as\s+(?<direct>[^\s]+)", RegexOptions.Compiled);

        public RequireJsModule(string name, string referencePath, params string[] nameSpaces)
        {
            this.Name = name;
            this.ReferencePath = referencePath;
            this.NameSpaces = nameSpaces.ToList();
            this.DefaultNamspace = nameSpaces.FirstOrDefault();
            this.ModuleName = ExtractModuleName(name);
        }

        public string ModuleName { get; internal set; }

        public string ReferencePath { get; private set; }

        public IEnumerable<string> NameSpaces { get; private set; }

        public string DefaultNamspace { get; private set; }

        public string Name { get; private set; }

        public string InterfacePath { get; set; }

        private static string ExtractModuleName(string ss)
        {
            var match = Expression.Match(ss);
            if (!match.Success)
            {
                throw new Exception("Couldn't extract module name");
            }

            if (match.Groups["direct"].Success)
            {
                return match.Groups["direct"].Value.Trim();
            }

            var value = match.Groups["named"].Value.Trim();
            return value.Split(',').Length > 1 ? string.Empty : value;
        }
    }
}
