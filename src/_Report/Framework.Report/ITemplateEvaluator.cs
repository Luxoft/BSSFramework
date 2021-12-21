using System.Collections.Generic;

using JetBrains.Annotations;

namespace Framework.Report
{
    public interface ITemplateEvaluator<in TTemplate, out TResult>
    {
        TResult Evaluate([NotNull] TTemplate template, object rootObject = null, IReadOnlyDictionary<string, object> variables = null, bool throwEvaluateException = false);
    }

    public interface ITemplateEvaluator<TTemplate> : ITemplateEvaluator<TTemplate, TTemplate>
    {

    }

    public interface ITemplateEvaluatorFactory
    {
        ITemplateEvaluator<TTemplate> Create<TTemplate>([NotNull] object sourceObject);
    }
}
