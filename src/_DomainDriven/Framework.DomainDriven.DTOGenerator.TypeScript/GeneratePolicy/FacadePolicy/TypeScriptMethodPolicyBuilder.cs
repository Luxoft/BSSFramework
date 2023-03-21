using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Framework.Core;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.Facade;

/// <summary>
/// Политика использования генерируемых методов
/// </summary>
/// <typeparam name="TFacade">Тип фасада</typeparam>
public class TypeScriptMethodPolicyBuilder<TFacade> : ITypeScriptMethodPolicy
{
    private readonly HashSet<MethodInfo> usedMethods;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="allowAll">Разрешить все методы фасада</param>
    public TypeScriptMethodPolicyBuilder(bool allowAll = false)
    {
        this.usedMethods = allowAll ? typeof(TFacade).ExtractContractMethods().ToHashSet()
                                   : new HashSet<MethodInfo>();
    }

    public IEnumerable<MethodInfo> UsedMethods => this.usedMethods;

    /// <summary>
    /// Добавление метода для генерации
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="invokeMethodExpr">Вызов метода с параметрами по умолчанию</param>
    /// <returns></returns>
    public TypeScriptMethodPolicyBuilder<TFacade> Add<TResult>(Expression<Func<TFacade, TResult>> invokeMethodExpr)
    {
        this.usedMethods.Add(this.GetMethod(invokeMethodExpr));

        return this;
    }

    /// <summary>
    /// Добавление метода для генерации
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="invokeMethodExpr">Вызов метода с параметрами по умолчанию</param>
    public TypeScriptMethodPolicyBuilder<TFacade> Add(Expression<Action<TFacade>> invokeMethodExpr)
    {
        this.usedMethods.Add(this.GetMethod(invokeMethodExpr));

        return this;
    }

    /// <summary>
    /// Исключение метода из генерации
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="invokeMethodExpr">Вызов метода с параметрами по умолчанию</param>
    /// <returns></returns>
    public TypeScriptMethodPolicyBuilder<TFacade> Except<TResult>(Expression<Func<TFacade, TResult>> invokeMethodExpr)
    {
        this.usedMethods.Remove(this.GetMethod(invokeMethodExpr));

        return this;
    }

    /// <summary>
    /// Исключение метода из генерации
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="invokeMethodExpr">Вызов метода с параметрами по умолчанию</param>
    public TypeScriptMethodPolicyBuilder<TFacade> Except(Expression<Action<TFacade>> invokeMethodExpr)
    {
        this.usedMethods.Remove(this.GetMethod(invokeMethodExpr));

        return this;
    }



    private MethodInfo GetMethod(LambdaExpression invokeMethodExpr)
    {
        var methodRequest = from callExpr in (invokeMethodExpr.UpdateBodyBase(FixPropertySourceVisitor.Value).Body as MethodCallExpression).ToMaybe()

                            where callExpr.Object == invokeMethodExpr.Parameters.Single()

                            select callExpr.Method;

        return methodRequest.GetValue("invalid input expression");
    }

    bool ITypeScriptMethodPolicy.Used(MethodInfo methodInfo)
    {
        return this.usedMethods.Contains(methodInfo);
    }
}
