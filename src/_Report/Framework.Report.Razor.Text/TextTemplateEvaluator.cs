using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;

using JetBrains.Annotations;

using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace Framework.Report.Razor.Text
{
    public class TextTemplateEvaluator :
        ITemplateEvaluator<string>//,
        //ITemplateEvaluator<Dictionary<string, string>>
    {
        private readonly IRazorEngineService _razorEngine;

        private readonly IDictionaryCache<string, string> _templateNameGeneerator = new DictionaryCache<string, string>(_ => Guid.NewGuid().ToString()).WithLock();

        public TextTemplateEvaluator([NotNull] IRazorEngineService razorEngine)
        {
            if (razorEngine == null) throw new ArgumentNullException(nameof(razorEngine));

            this._razorEngine = razorEngine;
        }

        public string Evaluate(string template, [NotNull] object rootObject, IReadOnlyDictionary<string, object> variables = null, bool throwEvaluateException = false)
        {
            if (rootObject == null) throw new ArgumentNullException(nameof(rootObject));
            if (variables.Maybe(v => v.Any())) throw new NotSupportedException();

            var modelType = rootObject.GetType();

            var templateName = this._templateNameGeneerator[template];

            CompileTemplate(this._razorEngine, templateName, template, modelType);

            var message = this._razorEngine.Run(templateName, modelType, rootObject);

            return message;
        }

        //public Dictionary<string, string> Evaluate(Dictionary<string, string> template, object rootObject = null, Dictionary<string, object> variables = null, bool throwEvaluateException = false)
        //{
        //    throw new NotImplementedException();
        //}

        //var model = new ObjectVersion<EmployeeTransfer>
        //            {
        //                Current = employeeTransfer,
        //                Previous = null
        //            };

        //            var config = new TemplateServiceConfiguration();
        //            config.Debug = false;
        //            config.DisableTempFileLocking = true; // loads the files in-memory (gives the templates full-trust permissions)
        //            config.CachingProvider = new DefaultCachingProvider(t => { }); //disables the warnings

        //            var razorEngineService = RazorEngineService.Create(config);

        //            var layoutName = "MyLayout";

        //            if (!razorEngineService.IsTemplateCached(layoutName, null))
        //            {
        //                razorEngineService.AddTemplate(layoutName, layoutTemplate);
        //                razorEngineService.Compile(layoutName);
        //            }

        //            var templateName = "MyTemplate";

        //            //var modelType = typeof(ObjectVersion<EmployeeTransfer>);
        //            var modelType = model.GetType();

        //            if (!razorEngineService.IsTemplateCached(templateName, modelType))
        //            {
        //                razorEngineService.AddTemplate(templateName, messageTemplate);
        //                razorEngineService.Compile(templateName, modelType);
        //            }

        //            var message = razorEngineService.Run(templateName, modelType, model);


        private static void CompileTemplate(IRazorEngineService service, string name, string template, Type modelType = null)
        {
            if (!service.IsTemplateCached(name, modelType))
            {
                service.AddTemplate(name, template);
                service.Compile(name, modelType);
            }
        }

        public static readonly TextTemplateEvaluator Default = new TextTemplateEvaluator(RazorEngineService.Create(TemplateServiceConfigurationHelper.CreateDefault()));
    }

    public class TemplateServiceConfigurationHelper
    {
        public static TemplateServiceConfiguration CreateDefault()
        {
            var config = new TemplateServiceConfiguration();

            config.Debug = false;

            // loads the files in-memory (gives the templates full-trust permissions)
            config.DisableTempFileLocking = true;

            // disables the warnings
            config.CachingProvider = new DefaultCachingProvider(t => { });

            return config;
        }
    }
}
