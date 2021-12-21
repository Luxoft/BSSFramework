using System;
using System.Collections.Generic;
using System.Linq;

using Framework.DomainDriven.DBGenerator.Team;

namespace Framework.DomainDriven.DBGenerator.Contracts
{
    public class DatabaseScriptResultDecorator : IDatabaseScriptResult
    {
        private readonly IDatabaseScriptResult _source;

        private readonly Func<string, string> _resultSelector;

        public DatabaseScriptResultDecorator(IDatabaseScriptResult source, Func<string, string> resultSelector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (resultSelector == null)
            {
                throw new ArgumentNullException(nameof(resultSelector));
            }

            this._source = source;
            this._resultSelector = resultSelector;
        }

        public IEnumerable<string> this[ApplyMigrationDbScriptMode mode]
        {
            get
            {
                return this._source[mode].Select(z => this._resultSelector(z));
            }
        }

        public IEnumerable<IEnumerable<string>> GetResults()
        {
            foreach (var result in this._source.GetResults())
            {
                yield return result.Select(this._resultSelector);
            }
        }

        public string ToNewLinesCombined()
        {
            return this._resultSelector(this._source.ToNewLinesCombined());
        }

        public IDatabaseScriptResult Evaluate()
        {
            return this._source.Evaluate();
        }
    }
}