namespace Wealth.StrategyTracking.API.Services;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapGrpcServices(this IEndpointRouteBuilder app)
    {
        app.MapGrpcService<StrategiesServiceImpl>();
        return app;
    }
}