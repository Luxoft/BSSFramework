using System;
using System.Linq;
using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;

using Framework.CodeDom;
using Framework.Core;
using Framework.Restriction;

namespace Framework.DomainDriven.DTOGenerator.Client
{
    public static class CustomAttributeProviderExtensions
    {
        public static IEnumerable<CodeAttributeDeclaration> GetRestrictionCodeAttributeDeclarations(this ICustomAttributeProvider source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var reqAttr = source.GetCustomAttribute<RequiredAttribute>();

            if (reqAttr != null)
            {
                yield return reqAttr.GetType().ToTypeReference().ToAttributeDeclaration();
            }

            var maxLengthAttr = source.GetCustomAttribute<MaxLengthAttribute>();

            if (maxLengthAttr != null)
            {
                var args = maxLengthAttr.Value == int.MaxValue
                         ? new CodeAttributeArgument[0]
                         : new[] {  maxLengthAttr.Value.ToPrimitiveExpression().ToAttributeArgument() };

                yield return maxLengthAttr.GetType().ToTypeReference().ToAttributeDeclaration(args);
            }

            foreach (var keyAttr in ((IEnumerable<IUniqueAttribute>)source.GetCustomAttributes<UniqueGroupAttribute>())
                                                            .Concat(source.GetCustomAttributes<UniqueElementAttribute>()))
            {
               if (keyAttr.Key == null)
               {
                   yield return keyAttr.GetType().ToTypeReference().ToAttributeDeclaration();
               }
               else
               {
                   yield return keyAttr.GetType().ToTypeReference().ToAttributeDeclaration(keyAttr.Key.ToPrimitiveExpression().ToAttributeArgument());
               }
            }
        }
    }
}