using System;
using System.Collections.Generic;
using System.Diagnostics;

using Framework.Core;

using Spring.Core;
using Spring.Expressions;

namespace Framework.Report.Spring
{
    public class PrimitiveTemplateEvaluator : ITemplateEvaluator<string, object>
    {
        private readonly bool _catchNullValueInNestedPath;


        public PrimitiveTemplateEvaluator(bool catchNullValueInNestedPath)
        {
            this._catchNullValueInNestedPath = catchNullValueInNestedPath;
        }


        public object Evaluate(string template, object rootObject = null, IReadOnlyDictionary<string, object> variables = null, bool throwEvaluateException = false)
        {
            if (this._catchNullValueInNestedPath)
            {
                try
                {
                    return ExpressionEvaluator.GetValue(rootObject, template, variables.Clone());
                }
                catch (NullValueInNestedPathException ex)
                {
                    Debug.WriteLine(ex.Message);

                    if (throwEvaluateException)
                    {
                        //throw new EvaluationException(ex.Message, ex, template); //TODO:Handle?
                        throw;
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                catch (Exception ex)
                {
                    if (throwEvaluateException)
                    {
                        throw new EvaluationException(ex.Message, ex, template);
                    }
                    else
                    {
                        Debug.WriteLine(ex.Message);
                        return ex.Message;
                    }
                }
            }
            else
            {
                try
                {
                    return (string)ExpressionEvaluator.GetValue(rootObject, template, variables.Clone());
                }
                catch (Exception ex)
                {
                    if (throwEvaluateException)
                    {
                        throw new EvaluationException(ex.Message, ex, template);
                    }
                    else
                    {
                        Debug.WriteLine(ex.Message);
                        return ex.Message;
                    }
                }
            }
        }

        public static readonly PrimitiveTemplateEvaluator Default = new PrimitiveTemplateEvaluator(true);
    }
}