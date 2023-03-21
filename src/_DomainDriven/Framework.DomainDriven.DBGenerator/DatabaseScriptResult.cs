using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven.DBGenerator.Contracts;
using Framework.DomainDriven.DBGenerator.Team;

namespace Framework.DomainDriven.DBGenerator;

public static class DatabaseScriptResultFactory
{
    public static IDatabaseScriptResult Create(Dictionary<ApplyMigrationDbScriptMode, Lazy<IEnumerable<string>>> dictionary)
    {
        return new LazyDatabaseScriptResult(dictionary);
    }

    struct EvaluatedDatabaseScriptResult : IDatabaseScriptResult
    {
        private readonly Dictionary<ApplyMigrationDbScriptMode, IEnumerable<string>> dictionary;

        public EvaluatedDatabaseScriptResult(IDatabaseScriptResult source)
                : this()
        {
            this.dictionary = new Dictionary<ApplyMigrationDbScriptMode, IEnumerable<string>>();

            foreach (var applyMigrationDbScriptMode in GetSortedModes())
            {
                this.dictionary.Add(applyMigrationDbScriptMode, source[applyMigrationDbScriptMode].ToList());
            }
        }

        public IEnumerable<string> this[ApplyMigrationDbScriptMode mode]
        {
            get
            {
                IEnumerable<string> result = null;
                if (this.dictionary.TryGetValue(mode, out result))
                {
                    return result;
                }
                return new string[0];
            }
        }

        public IEnumerable<IEnumerable<string>> GetResults()
        {
            return this.dictionary.OrderBy(z => (int)z.Key).Select(z => z.Value);
        }

        public string ToNewLinesCombined()
        {
            return this.GetResults().Select(z => z.Join(Environment.NewLine)).Join(Environment.NewLine);
        }

        public IDatabaseScriptResult Evaluate()
        {
            return this;
        }
    }

    struct LazyDatabaseScriptResult : IDatabaseScriptResult
    {
        private readonly Dictionary<ApplyMigrationDbScriptMode, Lazy<IEnumerable<string>>> dictionary;

        public LazyDatabaseScriptResult(Dictionary<ApplyMigrationDbScriptMode, Lazy<IEnumerable<string>>> dictionary)
                : this()
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            this.dictionary = dictionary;
        }

        public IEnumerable<string> this[ApplyMigrationDbScriptMode mode]
        {
            get
            {
                Lazy<IEnumerable<string>> result = null;
                if (this.dictionary.TryGetValue(mode, out result))
                {
                    return result.Value;
                }
                return new string[0];
            }
        }

        public IEnumerable<IEnumerable<string>> GetResults()
        {
            return this.dictionary.OrderBy(z => (int)z.Key).Select(z => z.Value.Value);
        }

        public string ToNewLinesCombined()
        {
            var tempResult = this.GetResults().Select(z => z.Join(Environment.NewLine)).ToList();
            return tempResult.Join(Environment.NewLine);
        }

        public IDatabaseScriptResult Evaluate()
        {
            return new EvaluatedDatabaseScriptResult(this);
        }
    }

    public static IDatabaseScriptResult Combine(this IEnumerable<IDatabaseScriptResult> source)
    {
        var dict = GetSortedModes()
                .ToDictionary(z => z, z => new Lazy<IEnumerable<string>>(() => source.Where(q => null != q).Select(q => q[z]).SelectMany(q => q)));
        return Create(dict);
    }

    private static IOrderedEnumerable<ApplyMigrationDbScriptMode> GetSortedModes()
    {
        return Enum.GetValues(typeof(ApplyMigrationDbScriptMode)).OfType<ApplyMigrationDbScriptMode>().OrderBy(z => z);
    }

    public static IDatabaseScriptResult CreateEvaluated(IDatabaseScriptResult generateScript)
    {
        var result = generateScript.ToNewLinesCombined();
        return generateScript;
    }
}
