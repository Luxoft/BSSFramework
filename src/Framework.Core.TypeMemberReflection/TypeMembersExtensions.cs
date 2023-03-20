using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Framework.Core;

public static class TypeMembersExtensions
{
    public static Type GetMemberType(this Type type, string memberName, bool raiseIfNotFound)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (memberName == null) throw new ArgumentNullException(nameof(memberName));

        return type.GetMemberType(memberName, raiseIfNotFound ? () => new Exception($"Member \"{memberName}\" not found in type \"{type.Name}\"") : default(Func<Exception>));
    }

    public static Type GetMemberType(this Type type, string memberName, Func<Exception> raiseIfNotFoundException)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (memberName == null) throw new ArgumentNullException(nameof(memberName));

        var memberType = type.GetMemberType(memberName);

        if (memberType == null && raiseIfNotFoundException != null)
        {
            throw raiseIfNotFoundException();
        }

        return memberType;
    }

    public static Type GetMemberType(this Type type, string memberName)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (memberName == null) throw new ArgumentNullException(nameof(memberName));

        if (type.IsInterface)
        {
            var property = type.GetAllInterfaceProperties().FirstOrDefault(prop => prop.Name == memberName);

            if (property != null)
            {
                return property.PropertyType;
            }
        }

        return type.GetProperty(memberName).Maybe(p => p.PropertyType) ?? type.GetField(memberName).Maybe(f => f.FieldType);
    }

    public static PropertyInfo GetProperty(this Type type, string propertyName, bool raiseIfNotFound)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

        return type.GetProperty(propertyName, raiseIfNotFound ? () => new Exception($"Property \"{propertyName}\" not found in type \"{type.Name}\"") : default(Func<Exception>));
    }

    public static PropertyInfo GetProperty(this Type type, string propertyName, Func<Exception> raiseIfNotFoundException)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

        var property = type.GetProperty(propertyName);

        if (property == null && raiseIfNotFoundException != null)
        {
            throw raiseIfNotFoundException();
        }

        return property;
    }

    public static PropertyInfo GetProperty(this Type type, string propertyName, StringComparison stringComparison, bool raiseIfNotFound)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

        return type.GetProperty(propertyName, stringComparison, raiseIfNotFound ? () => new Exception(
                                                                                   $"Property \"{propertyName}\" not found in type \"{type.Name}\"") : default(Func<Exception>));
    }

    public static PropertyInfo GetProperty(this Type type, string propertyName, StringComparison stringComparison, Func<Exception> raiseIfNotFoundException)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

        var property = type.GetProperties().SingleOrDefault(p => p.Name.Equals(propertyName, stringComparison));

        if (property == null && raiseIfNotFoundException != null)
        {
            throw raiseIfNotFoundException();
        }

        return property;
    }

    public static MethodInfo GetMethod(this Type type, string methodName, bool raiseIfNotFound)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (methodName == null) throw new ArgumentNullException(nameof(methodName));

        return type.GetMethod(methodName, raiseIfNotFound ? () => new Exception($"Method \"{methodName}\" not found") : default(Func<Exception>));
    }

    public static MethodInfo GetMethod(this Type type, string methodName, Func<Exception> raiseIfNotFoundException)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (methodName == null) throw new ArgumentNullException(nameof(methodName));

        var method = type.GetMethod(methodName);

        if (method == null && raiseIfNotFoundException != null)
        {
            throw raiseIfNotFoundException();
        }

        return method;
    }

    public static MethodInfo GetMethod(this Type type, string methodName, BindingFlags bindingFlags, bool raiseIfNotFound)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (methodName == null) throw new ArgumentNullException(nameof(methodName));

        return type.GetMethod(methodName, bindingFlags, raiseIfNotFound ? () => new Exception($"Method \"{methodName}\" not found") : default(Func<Exception>));
    }

    public static MethodInfo GetMethod(this Type type, string methodName, BindingFlags bindingFlags, Func<Exception> raiseIfNotFoundException)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (methodName == null) throw new ArgumentNullException(nameof(methodName));

        var method = type.GetMethod(methodName, bindingFlags);

        if (method == null && raiseIfNotFoundException != null)
        {
            throw raiseIfNotFoundException();
        }

        return method;
    }



    public static PropertyInfo GetProperty(this Type type, string propertyName, BindingFlags bindingAttr, bool raise = false)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

        var property = type.GetProperty(propertyName, bindingAttr);

        return raise ? property.FromMaybe(() => new Exception($"Property \"{propertyName}\" not found in type:{type.Name}"))
                       : property;
    }

    public static PropertyInfo GetBaseProperty(this PropertyInfo property, bool byGet = true)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        var currentMethod = byGet ? (property.GetGetMethod() ?? property.GetGetMethod(true))
                                    : (property.GetSetMethod() ?? property.GetSetMethod(true));

        var baseMethod = currentMethod.Maybe(v => v.GetBaseDefinition());

        return baseMethod.Maybe(v => v.ReflectedType == property.ReflectedType
                                             ? null
                                             : v.ReflectedType.GetProperty(property.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));
    }

    public static IEnumerable<PropertyInfo> GetBaseProperties(this PropertyInfo property, bool byGet = true)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        return property.GetAllElements(p => p.GetBaseProperty(byGet));
    }


    public static PropertyInfo GetTopProperty(this PropertyInfo property, bool byGet = true)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        return property.GetBaseProperties().Last();
    }



    public static MethodInfo GetEqualityMethod(this Type type, bool withBaseTypes = false)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        if (withBaseTypes)
        {
            return type.GetAllElements(t => t.BaseType).Select(t => t.GetEqualityMethod()).FirstOrDefault(t => t != null);
        }
        else
        {
            return type.GetMethods(BindingFlags.Static | BindingFlags.Public).FirstOrDefault(m =>

                    m.ReturnType == typeof(bool) && m.Name == "op_Equality"
                                                 && m.GetParameters().Pipe(parameters =>

                                                                                   parameters.Length == 2 && parameters.All(parameter => parameter.ParameterType == type)));
        }
    }

    public static MethodInfo GetInequalityMethod(this Type type, bool withBaseTypes = false)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        if (withBaseTypes)
        {
            return type.GetAllElements(t => t.BaseType).Select(t => t.GetInequalityMethod()).FirstOrDefault(t => t != null);
        }
        else
        {
            return type.GetMethods(BindingFlags.Static | BindingFlags.Public).FirstOrDefault(m =>

                    m.ReturnType == typeof(bool) && m.Name == "op_Inequality"
                                                 && m.GetParameters().Pipe(parameters =>

                                                                                   parameters.Length == 2 && parameters.All(parameter => parameter.ParameterType == type)));
        }
    }
}
