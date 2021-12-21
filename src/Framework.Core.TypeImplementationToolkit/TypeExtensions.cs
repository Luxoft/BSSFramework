using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using JetBrains.Annotations;

namespace Framework.Core
{
    public static class TypeExtensions
    {

        public static bool IsGenericTypeImplementation([NotNull] this Type type, [NotNull] Type genericTypeDefinition, Type[] implementArguments = null)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (genericTypeDefinition == null) throw new ArgumentNullException(nameof(genericTypeDefinition));

            return type.IsGenericType && type.GetGenericTypeDefinition() == genericTypeDefinition
                                      && (implementArguments == null || type.GetGenericArguments().SequenceEqual(implementArguments));
        }

        public static Type GetGenericTypeImplementationArgument([NotNull] this Type type, [NotNull] Type genericTypeDefinition)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (genericTypeDefinition == null) throw new ArgumentNullException(nameof(genericTypeDefinition));

            return type.GetGenericTypeImplementationArguments(genericTypeDefinition, args => args.Single());
        }

        public static TResult GetGenericTypeImplementationArguments<TResult>([NotNull] this Type type, [NotNull] Type genericTypeDefinition, Func<Type[], TResult> getResult)
            where TResult : class
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (genericTypeDefinition == null) throw new ArgumentNullException(nameof(genericTypeDefinition));
            if (getResult == null) throw new ArgumentNullException(nameof(getResult));

            return type.IsGenericTypeImplementation(genericTypeDefinition)
                 ? getResult(type.GetGenericArguments())
                 : null;
        }


        public static bool IsInterfaceImplementation(this Type type, Type interfaceType, Type[] implementArguments = null)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (interfaceType == null) throw new ArgumentNullException(nameof(interfaceType));

            if (!interfaceType.IsInterface) throw new ArgumentNullException($"Type \"{interfaceType.Name}\" is not interface");

            var interfaces = type.GetAllInterfaces().ToArray();

            return interfaceType.IsGenericTypeDefinition
                 ? interfaces.Any(i => i.IsGenericTypeImplementation(interfaceType, implementArguments))
                 : interfaces.Contains(interfaceType);
        }

        public static Type[] GetInterfaceImplementationArguments(this Type type, Type interfaceType)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (interfaceType == null) throw new ArgumentNullException(nameof(interfaceType));

            return type.GetInterfaceImplementationArguments(interfaceType, v => v);
        }

        public static Type GetInterfaceImplementationArgument(this Type type, Type interfaceType)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (interfaceType == null) throw new ArgumentNullException(nameof(interfaceType));

            return type.GetInterfaceImplementationArguments(interfaceType, args => args.Single());
        }

        public static TResult GetInterfaceImplementationArguments<TResult>(this Type type, Type interfaceType, Func<Type[], TResult> getResult)
              where TResult : class
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (interfaceType == null) throw new ArgumentNullException(nameof(interfaceType));

            return type.GetInterfaceImplementation(interfaceType).Maybe(i => getResult(i.GetGenericArguments()));
        }

        public static Type GetInterfaceImplementation(this Type type, Type interfaceType)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (interfaceType == null) throw new ArgumentNullException(nameof(interfaceType));

            return type.IsInterfaceImplementation(interfaceType)
                 ? type.GetAllInterfaces().SingleOrDefault(i => IsGenericTypeImplementation(i, interfaceType))
                 : null;
        }



        public static IEnumerable<MethodInfo> GetAllInterfaceMethods(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return type.GetAllInterfaces().SelectMany(t => t.GetMethods());
        }

        public static IEnumerable<PropertyInfo> GetAllInterfaceProperties(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return type.GetAllInterfaces().SelectMany(t => t.GetProperties());
        }

        public static IEnumerable<Type> GetAllInterfaces(this Type type, bool unwrapSubInterfaceGenerics = true)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return type.IsInterface ? new[] { type }.Concat(type.GetInterfaces().Pipe(type.IsGenericTypeDefinition && unwrapSubInterfaceGenerics, res => res.Select(subType => subType.IsGenericType ? subType.GetGenericTypeDefinition() : subType).ToArray()))
                       : type.GetInterfaces();
        }
    }
}
