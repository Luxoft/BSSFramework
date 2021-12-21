using System;
using System.IO;

using Framework.Core;
using Framework.Report;

namespace Framework.DomainDriven.ServiceModel.TemplateEvaluator
{
    public class TemplateEvaluatorFactory : ITemplateEvaluatorFactory
    {
        public virtual ITemplateEvaluator<TTemplate> Create<TTemplate>(object sourceObject)
        {
            if (sourceObject == null) throw new ArgumentNullException(nameof(sourceObject));

            if (sourceObject is Framework.Configuration.Domain.MessageTemplate messageTemplate && typeof(TTemplate) == typeof(string))
            {
                switch (messageTemplate.Renderer)
                {
                    case Framework.Configuration.Domain.TemplateRenderer.Spring:
                        return (ITemplateEvaluator<TTemplate>)Framework.Report.Spring.Text.TextTemplateEvaluator.Default;

                    case Framework.Configuration.Domain.TemplateRenderer.Razor:
                        return (ITemplateEvaluator<TTemplate>)Framework.Report.Razor.Text.TextTemplateEvaluator.Default;

                    default:
                        throw new ArgumentOutOfRangeException("messageTemplate.Renderer");
                }
            }
            else if (sourceObject is Exception && typeof(TTemplate) == typeof(string))
            {
                return (ITemplateEvaluator<TTemplate>)Framework.Report.Spring.Text.TextTemplateEvaluator.Default;
            }
            else if (sourceObject is Framework.Configuration.Domain.ITemplateContainer && typeof(TTemplate) == typeof(byte[]))
            {
                var attachment = sourceObject as Framework.Configuration.Domain.ITemplateContainer;

                var extension = Path.GetExtension(attachment.Name).Skip(".", false).ToLower();

                switch (extension)
                {
                    case "xls":
                    case "xlsx":
                    case "xlsm":
                        return (ITemplateEvaluator<TTemplate>)new Framework.Report.Excel.Engine.ExcelEPPlusTemplateEvaluator(extension);

                    case "html":
                        return (ITemplateEvaluator<TTemplate>)new Framework.Report.Spring.Text.TextTemplateEvaluator(extension);

                    default:
                        return null;
                }
            }
            else
            {
                throw new Exception($"Can't create templateEvaluator for type {typeof(TTemplate)} by object {sourceObject.ToFormattedString()}");
            }
        }
    }
}
