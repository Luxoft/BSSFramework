using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using JetBrains.Annotations;

namespace Framework.Core
{
    /// <summary>
    /// Класс, для автоматического создания адаптера интерфейса посредством генерации его имплементации в рантайме
    /// </summary>
    public abstract class InterfaceImplementTypeBuilder : IAnonymousTypeBuilder<Type>
    {
        private const string CreateInstanceMethodName = "CreateInstance";

        private readonly ModuleBuilder moduleBuilder;

        private readonly Type baseType;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="moduleBuilder">Модуль, где будет генерировать анонимный тип</param>
        protected InterfaceImplementTypeBuilder([NotNull] ModuleBuilder moduleBuilder, [NotNull] Type baseType)
        {
            if (moduleBuilder == null) throw new ArgumentNullException(nameof(moduleBuilder));

            if (moduleBuilder is not null) this.moduleBuilder = moduleBuilder;
            this.baseType = baseType ?? throw new ArgumentNullException(nameof(baseType));
        }

        /// <summary>
        /// Получение фабричной функции для создания прокси-типа
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Func<Func<T>, T> GetCreateProxyFunc<T>()
        {
            return (Func<Func<T>, T>)this.GetCreateProxyFunc(typeof(T));
        }

        /// <summary>
        /// Получение фабричной функции для создания прокси-типа
        /// </summary>
        /// <param name="sourceType">Тип базого интерфейса</param>
        /// <returns></returns>
        public Delegate GetCreateProxyFunc([NotNull] Type sourceType)
        {
            if (sourceType == null) throw new ArgumentNullException(nameof(sourceType));

            var type = this.GetAnonymousType(sourceType);

            var funcType = typeof(Func<>).MakeGenericType(sourceType);

            var factoryMethod = type.GetMethod(CreateInstanceMethodName);

            return factoryMethod.ToDelegate(typeof(Func<,>).MakeGenericType(funcType, sourceType));
        }

        /// <summary>
        /// Создание анонимного тип
        /// </summary>
        /// <param name="sourceType">Тип базого интерфейса</param>
        /// <returns></returns>
        public Type GetAnonymousType(Type sourceType)
        {
            if (sourceType == null) throw new ArgumentNullException(nameof(sourceType));
            if (!sourceType.IsInterface) throw new ArgumentException("Proxy Type must be interface");

            var lazyType = this.baseType.MakeGenericType(sourceType);
            var funcType = typeof(Func<>).MakeGenericType(sourceType);

            var name = $"{sourceType.ToCSharpFullName()}_{this.baseType.Name}Proxy";

            var typeBuilder = this.moduleBuilder.DefineType(name, TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Sealed, lazyType, new[] { sourceType });

            var getInstanceMethod = lazyType.GetProperty("Value", true).GetGetMethod();

            var implInterfaceMethodAttr = MethodAttributes.Private | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual | MethodAttributes.Final;

            foreach (var interfaceType in sourceType.GetAllInterfaces())
            {
                var methods = interfaceType.GetMethods().ToDictionary(method => method, method =>
                {
                    var methodBuilder = typeBuilder.DefineMethod(method.Name, implInterfaceMethodAttr, method.ReturnType, method.GetParameters().ToArray(p => p.ParameterType));

                    var generator = methodBuilder.GetILGenerator();

                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Call, getInstanceMethod);

                    if (method.ContainsGenericParameters)
                    {
                        var genericMap = method.GetGenericArguments().ZipStrong(methodBuilder.DefineGenericParameters(method.GetGenericArguments().ToArray(arg => arg.Name + "Impl")), (key, value) => key.ToKeyValuePair(value)).ToDictionary();

                        Func<Type, Type> fixGenericType = null;
                        fixGenericType = t =>
                        {
                            var genericOverride = genericMap.GetValueOrDefault(t);

                            if (genericOverride != null)
                            {
                                return genericOverride;
                            }
                            else if (t.IsGenericType)
                            {
                                var genericDefinition = t.GetGenericTypeDefinition();
                                var genericArgs = t.GetGenericArguments().ToArray(fixGenericType);

                                var fixedType = genericDefinition.MakeGenericType(genericArgs);

                                return fixedType;
                            }
                            else if (t.DeclaringType.Maybe(dec => interfaceType.IsGenericTypeImplementation(dec)))
                            {
                                var index = t.DeclaringType.GetGenericArguments().IndexOf(t);

                                return interfaceType.GetGenericArguments()[index];
                            }
                            else
                            {
                                return t;
                            }
                        };

                        foreach (var pair in genericMap)
                        {
                            if (pair.Key.ContainsGenericParameters)
                            {
                                // with explicit method implement not need contraints of class generics
                                var contraints = pair.Key.GetGenericParameterConstraints()
                                    .Select(fixGenericType)
                                    .Partial(t => t.IsInterface, (interfaceTypes, classTypes) => new { InterfaceTypes = interfaceTypes, ClassTypes = classTypes });

                                var baseTypeConstraint = contraints.ClassTypes.Aggregate(default(Type), (current, tryNested) =>
                                    current == null || current.IsAssignableFrom(tryNested) ? tryNested : current);

                                if (baseTypeConstraint != null && (baseTypeConstraint.DeclaringType == null || baseTypeConstraint.DeclaringType == typeBuilder))
                                {
                                    pair.Value.SetBaseTypeConstraint(baseTypeConstraint);
                                }

                                pair.Value.SetInterfaceConstraints(contraints.InterfaceTypes.ToArray());

                                pair.Value.SetGenericParameterAttributes(pair.Key.GenericParameterAttributes);
                            }
                        }
                    }

                    foreach (var pair in method.GetParameters().Select((parameter, index) => new { Parameter = parameter, Index = index + 1 }))
                    {
                        methodBuilder.DefineParameter(pair.Index, pair.Parameter.Attributes, pair.Parameter.Name);

                        generator.Emit(OpCodes.Ldarg, pair.Index);
                    }

                    generator.Emit(OpCodes.Callvirt, method);
                    generator.Emit(OpCodes.Ret);

                    typeBuilder.DefineMethodOverride(methodBuilder, method);

                    return methodBuilder;
                });

                foreach (var prop in interfaceType.GetProperties())
                {
                    var propertyBuilder = typeBuilder.DefineProperty(prop.Name, prop.Attributes, prop.PropertyType, prop.GetIndexParameters().ToArray(p => p.ParameterType));

                    var getMethod = prop.GetGetMethod();

                    if (getMethod != null)
                    {
                        propertyBuilder.SetGetMethod(methods[getMethod]);
                    }

                    var setMethod = prop.GetSetMethod();

                    if (setMethod != null)
                    {
                        propertyBuilder.SetSetMethod(methods[setMethod]);
                    }
                }

                foreach (var ev in interfaceType.GetEvents())
                {
                    var eventBuilder = typeBuilder.DefineEvent(ev.Name, ev.Attributes, ev.EventHandlerType);

                    eventBuilder.SetAddOnMethod(methods[ev.GetAddMethod()]);

                    eventBuilder.SetRemoveOnMethod(methods[ev.GetRemoveMethod()]);
                }
            }

            var ctorParamTypes = new[] { funcType };

            var baseCtor = lazyType.GetConstructor(ctorParamTypes);

            var ctorBuilder = typeBuilder.DefineConstructor(baseCtor.Attributes, baseCtor.CallingConvention, ctorParamTypes);
            {
                var generator = ctorBuilder.GetILGenerator();

                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldarg_1);
                generator.Emit(OpCodes.Call, baseCtor);
                generator.Emit(OpCodes.Ret);
            }

            var factoryMethodBuilder = typeBuilder.DefineMethod(CreateInstanceMethodName, MethodAttributes.Public | MethodAttributes.Static, sourceType, new[] { funcType });
            {
                var generator = factoryMethodBuilder.GetILGenerator();

                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Newobj, ctorBuilder);
                generator.Emit(OpCodes.Ret);
            }

            return typeBuilder.CreateTypeInfo();
        }
    }
}
