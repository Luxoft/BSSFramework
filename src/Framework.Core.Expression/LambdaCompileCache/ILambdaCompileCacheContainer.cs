namespace Framework.Core;

public interface ILambdaCompileCacheContainer
{
    ILambdaCompileCache this[Type routeType1] { get; }

    ILambdaCompileCache this[Type routeType1, Type routeType2] { get; }



    ILambdaCompileCache Get<TRouteType1, TRouteType2>(TRouteType1 routeType1);

    ILambdaCompileCache Get<TRouteType1, TRouteType2>(TRouteType1 routeType1, TRouteType2 routeType2);
}
