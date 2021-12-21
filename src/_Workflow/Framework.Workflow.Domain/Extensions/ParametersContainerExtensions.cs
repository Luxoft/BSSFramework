using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Workflow.Domain.Definition;

namespace Framework.Workflow.Domain.Runtime
{

    public static class ParametersContainerExtensions
    {
        /// <summary>
        /// Достает из контейнера параметры
        /// </summary>
        /// <param name="parametersContainer"></param>
        /// <returns>Словарь параметров с ключом-именем параметра</returns>
        public static Dictionary<string, string> GetParameters(this IParametersContainer<IParameterInstanceBase<Parameter>> parametersContainer)
        {
            if (parametersContainer == null) throw new ArgumentNullException(nameof(parametersContainer));

            return parametersContainer.Parameters.ToDictionary(parameter => parameter.Definition.Name, parameter => parameter.Value);
        }
    }
}