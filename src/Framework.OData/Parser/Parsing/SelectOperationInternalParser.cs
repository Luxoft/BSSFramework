using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Framework.Core;
using Framework.Parsing;
using Framework.QueryLanguage;

namespace Framework.OData;

internal class SelectOperationInternalParser : CharParsers
{
    private readonly NumberFormatInfo _numberFormatInfo;

    private readonly ParameterExpression _rootParameter = ParameterExpression.Default;

    private readonly LambdaExpressionInternalParser _rootLambdaExpressionParser;


    public SelectOperationInternalParser(NumberFormatInfo numberFormatInfo)
    {
        if (numberFormatInfo == null) throw new ArgumentNullException(nameof(numberFormatInfo));

        this._numberFormatInfo = numberFormatInfo;

        this._rootLambdaExpressionParser = new LambdaExpressionInternalParser(this._numberFormatInfo, this._rootParameter, this._rootParameter, new [] { this._rootParameter }.ToReadOnlyCollection());
    }


    private LambdaExpression CreateRootLambda(Expression body)
    {
        if (body == null) throw new ArgumentNullException(nameof(body));

        return new LambdaExpression(body, new[] { this._rootParameter });
    }


    public Parser<string, SelectOperation> MainParser
    {
        get
        {
            return from result in this.OfTable(

                                               this.GetElementParser("filter", this.GetLazy(() => this.FilterParser))
                                                   .ToRow(() => SelectOperation.Default.Filter),

                                               this.GetElementParser("orderby", this.GetLazy(() => this.OrdersParser))
                                                   .ToRow(() => SelectOperation.Default.Orders),

                                               this.GetElementParser("expand", this.GetLazy(() => this.ExpandsParser))
                                                   .ToRow(() => SelectOperation.Default.Expands),

                                               this.GetElementParser("select", this.GetLazy(() => this.SelectsParser))
                                                   .ToRow(() => SelectOperation.Default.Selects),

                                               this.GetElementParser("skip", this.Int32Parser)
                                                   .ToRow(() => SelectOperation.Default.SkipCount),

                                               this.GetElementParser("top", this.Int32Parser)
                                                   .ToRow(() => SelectOperation.Default.TakeCount),

                                               this.PreSpaces(this.Char('&')),

                                               (filter, orders, expands, selects, skipCount, takeCount) =>

                                                       new SelectOperation(filter, orders, expands, selects, skipCount, takeCount))

                   from _ in this.PreSpaces(this.Eof)

                   select result;
        }
    }



    private Parser<string, T> GetElementParser<T>(string name, Parser<string, T> itemParser)
    {
        return from _ in this.BetweenSpaces(this.Char('$'))

               from __ in this.StringIgnoreCase(name)

               from ___ in this.BetweenSpaces(this.Char('='))

               from item in itemParser

               select item;
    }



    private Parser<string, Expression> RootBodyParser
    {
        get { return this._rootLambdaExpressionParser.RootBodyParser; }
    }

    private Parser<string, LambdaExpression> FilterParser
    {
        get { return this.RootBodyParser.Select(this.CreateRootLambda); }
    }

    private Parser<string, SelectOrder> OrderParser
    {
        get
        {
            return from expr in this.RootBodyParser

                   from orderType in this.PreSpaces(this.OrderTypeParser).Or(this.Return(OrderType.Asc))

                   select new SelectOrder(this.CreateRootLambda(expr), orderType);
        }
    }

    private Parser<string, IEnumerable<SelectOrder>> OrdersParser
    {
        get { return this.SepBy1(this.OrderParser, ','); }
    }

    private Parser<string, IEnumerable<LambdaExpression>> ExpandsParser
    {
        get { return this.SepBy1(this.RootLambdaExpressionParser, ','); }
    }

    private Parser<string, IEnumerable<LambdaExpression>> SelectsParser
    {
        get { return this.SepBy1(this._rootLambdaExpressionParser.PropertyPathParser.Select(this.CreateRootLambda), ','); }
    }

    private Parser<string, LambdaExpression> RootLambdaExpressionParser
    {
        get { return this.RootBodyParser.Select(this.CreateRootLambda); }
    }


    private Parser<string, OrderType> OrderTypeParser
    {
        get
        {
            return this.StringIgnoreCase("desc").Select(_ => OrderType.Desc)
                       .Or(() => this.StringIgnoreCase("asc").Select(_ => OrderType.Asc));
        }
    }
}
