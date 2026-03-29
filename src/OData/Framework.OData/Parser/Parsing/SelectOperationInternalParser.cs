using System.Globalization;

using CommonFramework;

using Framework.OData.QueryLanguage;
using Framework.Parsing;

namespace Framework.OData.Parser.Parsing;

internal class SelectOperationInternalParser : CharParsers
{
    private readonly NumberFormatInfo numberFormatInfo;

    private readonly ParameterExpression rootParameter = ParameterExpression.Default;

    private readonly LambdaExpressionInternalParser rootLambdaExpressionParser;


    public SelectOperationInternalParser(NumberFormatInfo numberFormatInfo)
    {
        this.numberFormatInfo = numberFormatInfo;

        this.rootLambdaExpressionParser = new LambdaExpressionInternalParser(
            this.numberFormatInfo,
            this.rootParameter,
            this.rootParameter,
            new[] { this.rootParameter }.ToReadOnlyCollection());
    }


    private LambdaExpression CreateRootLambda(Expression body)
    {
        if (body == null) throw new ArgumentNullException(nameof(body));

        return new LambdaExpression(body, [this.rootParameter]);
    }


    public Parser<string, SelectOperation> MainParser =>
        from result in this.OfTable(

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

                new SelectOperation(filter, [.. orders], skipCount, takeCount) { Expands = [.. expands], Selects = [.. selects] })

        from _ in this.PreSpaces(this.Eof)

        select result;

    private Parser<string, T> GetElementParser<T>(string name, Parser<string, T> itemParser) =>
        from _ in this.BetweenSpaces(this.Char('$'))

        from __ in this.StringIgnoreCase(name)

        from ___ in this.BetweenSpaces(this.Char('='))

        from item in itemParser

        select item;

    private Parser<string, Expression> RootBodyParser => this.rootLambdaExpressionParser.RootBodyParser;

    private Parser<string, LambdaExpression> FilterParser => this.RootBodyParser.Select(this.CreateRootLambda);

    private Parser<string, SelectOrder> OrderParser =>
        from expr in this.RootBodyParser

        from orderType in this.PreSpaces(this.OrderTypeParser).Or(this.Return(OrderType.Asc))

        select new SelectOrder(this.CreateRootLambda(expr), orderType);

    private Parser<string, IEnumerable<SelectOrder>> OrdersParser => this.SepBy1(this.OrderParser, ',');

    private Parser<string, IEnumerable<LambdaExpression>> ExpandsParser => this.SepBy1(this.RootLambdaExpressionParser, ',');

    private Parser<string, IEnumerable<LambdaExpression>> SelectsParser => this.SepBy1(this.rootLambdaExpressionParser.PropertyPathParser.Select(this.CreateRootLambda), ',');

    private Parser<string, LambdaExpression> RootLambdaExpressionParser => this.RootBodyParser.Select(this.CreateRootLambda);

    private Parser<string, OrderType> OrderTypeParser =>
        this.StringIgnoreCase("desc").Select(_ => OrderType.Desc)
            .Or(() => this.StringIgnoreCase("asc").Select(_ => OrderType.Asc));
}
